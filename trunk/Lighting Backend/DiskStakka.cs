/*
 * Copyright (C) 2008 Jeremiah Johnson
 * Derived and translated from CD Store Disk Stakka driver Copyright (C) 2005 Eddie Cornejo
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace House_of_the_Future
{
    public class DiskStakka
    {
        public string database;
        System.Threading.Mutex mutex;
        const uint VENDOR_IMATION = 0x0718;
        const uint PRODUCT_OPDICOM_DISC_STAKKA = 0xD000;
        const uint IMATION_OPDICOM_DISC_STAKKA_TYPE = (VENDOR_IMATION << 16) | PRODUCT_OPDICOM_DISC_STAKKA;
        const uint IMATION_OPDICOM_DISC_STAKKA_MAX_SLOTS = 100;


        byte[] prevLogState = new byte[8];

        public uint unitID;
        int serial;
        byte messageID;
        byte[] sentPacket = new byte[9];

        int timeoutCount;
        bool slotStateKnown;
        bool[] slotOccupied = new bool[IMATION_OPDICOM_DISC_STAKKA_MAX_SLOTS];
        List<int> pendingEjects;
        int maxopid;
        int version1;
        int version2;

        int state;



        const byte CODE_START1 = 0x0A;
        const byte CODE_VERSION1 = 0x1A;
        const byte CODE_VERSION2 = 0x1B;
        const byte CODE_SERIAL = 0x1C;
        const byte CODE_START2 = 0x01;

        const byte CODE_INIT = 0xCC;
        const byte CODE_MOVE = 0x04;
        const byte CODE_LED_RATE = 0x06;
        const byte CODE_NEW_CD_ACK = 0x1D;
        const byte CODE_CLEAR_ERROR = 0x14;

        const byte LED_UNKNOWN_SLOTS_ON = 3;
        const byte LED_UNKNOWN_SLOTS_PERIOD = 6;
        const byte LED_READY_ON = 100;
        const byte LED_READY_PERIOD = 100;

        const int ERR_OK = 0;
        const int ERR_NO_DISK = -11;

        enum state_t
        {
            STATE_INIT,
            STATE_NORMAL,
            STATE_INSERT,
            STATE_EJECT,
            STATE_CLEAR_ERRORS,
        };
        public struct Command
        {
            public int opid;
            public int slot;
            public int command;
        };
        DiskStakkaManager manager;
        //extern const byte VERBOSE_DEFAULT;
        //extern const byte LOG_DIR_DEFAULT;

        public DiskStakka(uint _unitID, DiskStakkaManager _manager)
        {
            unitID = _unitID;
            state = (int)state_t.STATE_INIT;
            serial = 0;
            messageID = 0xFF;
            timeoutCount = 0;
            slotStateKnown = false;
            //myLogger = null;
            version1 = 0;
            version2 = 0;
            maxopid = -1;
            mutex = new System.Threading.Mutex();
            manager = _manager;
            pendingEjects = new List<int>();
            for (int i = 0; i < IMATION_OPDICOM_DISC_STAKKA_MAX_SLOTS; i++)
            {
                slotOccupied[i] = false;
            }
        }
        public void dispose()
        {
            /*if (myLogger != NULL)
            {
                LOG(myLogger, "Disc Stakka offline. Serial = %08x", serial);
                delete myLogger;
                myLogger = NULL;
            }*/
        }
        public int getSerial()
        {
            return serial;
        }

        public bool hasTimedOut()
        {
            return timeoutCount++ > 10;
        }
        public void setUsedSlots(List<int> state)
        {
            //DBG_SIMPLE(myLogger, "I now know my used slots");

            if (slotStateKnown)
            {
                /*if (myLogger != NULL)
                {
                  ERR(myLogger, "Slot state already known!");
                }*/
                return;
            }

            for (int i = 0; i < state.Count; i++)
            {
                slotOccupied[state[i]] = true;
            }

            slotStateKnown = true;
            /*if (myLogger != NULL)
            {
              DBG_ALL(myLogger, "Slot state known.");
            }*/
        }

        public void setCommand(List<Command> command)
        {
            for (int i = 0; i < command.Count; i++)
            {
                if (command[i].opid > maxopid)
                {
                    maxopid = command[i].opid;
                    if (command[i].command == 1)
                    {
                        //LOG(myLogger, "Received request to eject slot %u.", command[i].slot + 1);
                        mutex.WaitOne();
                        pendingEjects.Add(command[i].slot + 1);
                        mutex.ReleaseMutex();
                    }
                    else
                    {
                        //LOG(myLogger, "Odd command code received! OPID = %u, Slot = %u, Command = %u", command[i].opid, command[i].slot, command[i].command);
                    }
                }
            }
        }

        public void process(byte[] buf)
        {
            timeoutCount = 0;

            if ((messageID != 0xFF) && (sentPacket[2] != buf[2]))
            {
                /*if (myLogger != NULL)
                {
                    LOG(myLogger, "Resending...");
                }*/
                doWrite(sentPacket[3], sentPacket[4], sentPacket[5],
                        sentPacket[6], sentPacket[7], sentPacket[8]);

                return;
            }

            messageID = buf[2];

            switch (state)
            {
                case (int)state_t.STATE_INIT:
                    //logState("INIT", buf);
                    process_Init(buf);
                    break;

                case (int)state_t.STATE_NORMAL:
                    //logState("NORMAL", buf);

                    if (isBusy(buf))
                    {
                        break;
                    }

                    // Service any cd's waiting to be inserted...
                    if (isAwaitingNewCDAck(buf))
                    {
                        doWrite(CODE_NEW_CD_ACK);
                        state = (int)state_t.STATE_INSERT;
                        break;
                    }

                    // Process any pending eject requests...
                    
                    if (pendingEjects.Count > 0)
                    {
                        state = (int)state_t.STATE_EJECT;
                        break;
                    }

                    moveToEmptySlot(buf);
                    break;

                case (int)state_t.STATE_INSERT:
                    //logState("INSERT", buf);
                    process_Insert(buf);
                    break;

                case (int)state_t.STATE_EJECT:
                    //logState("EJECT", buf);
                    process_Eject(buf);
                    break;

                case (int)state_t.STATE_CLEAR_ERRORS:
                    //logState("CLEAR_ERROR", buf);
                    if (buf[3] == CODE_CLEAR_ERROR)
                    {
                        if ((buf[5] == 0x02) || (buf[5] == 0x08))
                        {
                            move(buf[4] - 1);
                        }
                        state = (int)state_t.STATE_NORMAL;
                        break;
                    }

                    doWrite(CODE_CLEAR_ERROR);
                    break;
                default:
                    /*if (myLogger != NULL)
                    {
                        ERR(myLogger, "Unknown state! %d", state);
                    }*/
                    return;
            }
        }

        // Sequence should be 0A, 1A, 1B, 1C, 01, 04
        void process_Init(byte[] buf)
        {
            messageID = buf[2];
            byte command = buf[3];

            switch (command)
            {
                case CODE_INIT:
                    doWrite(CODE_START1); // 0x0A
                    break;
                case CODE_START1:
                    doWrite(CODE_VERSION1); // 0x1A
                    break;
                case CODE_VERSION1:
                    version1 = (buf[4] << 24) | (buf[5] << 16) | (buf[6] << 8) | buf[7];
                    doWrite(CODE_VERSION2); // 0x1B
                    break;
                case CODE_VERSION2:
                    version2 = (buf[4] << 24) | (buf[5] << 16) | (buf[6] << 8) | buf[7];
                    requestSerial(); // 0x1C
                    break;
                case CODE_SERIAL:
                    {
                        serial = (buf[4] << 24) | (buf[5] << 16) | (buf[6] << 8) | buf[7];

                        //const char* dir = config.getValue("LOG_FILE", LOG_DIR_DEFAULT);
                        //char* myLogFile = new char[strlen(dir) + 30];
                        //sprintf(myLogFile, "%s/discstakka.%08x", dir, serial);

                        //myLogger = new Logger(atoi(config.getValue("VERBOSE", VERBOSE_DEFAULT)), myLogFile);
                        //delete [] myLogFile;

                        //dbInterfaceManager->notifyStartup(IMATION_OPDICOM_DISC_STAKKA_TYPE, serial);
                        //LOG(myLogger, "Disc Stakka onlilne. Serial = %08x, Firmware = %02x.%02x.%04x", serial, (version2 >> 16) & 0xFF, (version1 & 0xFF), (version2 & 0xFFFF));

                        doWrite(CODE_START2);
                        break;
                    }
                case CODE_START2:
                    state = (int)state_t.STATE_NORMAL;
                    break;

                default:
                    break;
                //if (myLogger != NULL)
                //{
                //  ERR(myLogger, "Unsupported command: %02x", command);
                //}
            }
        }

        void process_Insert(byte[] buf)
        {
            if (isBusy(buf))
            {
                return;
            }

            byte pos = (byte)(buf[4] - 1);
            slotOccupied[pos] = true;
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("insert into pending_inserts values (" + unitID + "," + pos + ");", conn);
            da.Fill(new System.Data.DataTable());
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
            state = (int)state_t.STATE_CLEAR_ERRORS;
        }

        void process_Eject(byte[] buf)
        {
            if ((isBusy(buf)) || (isCDInMotion(buf)))
            {
                return;
            }

            if (isCDRemoved(buf))
            {
                mutex.WaitOne();
                pendingEjects.RemoveAt(0);
                mutex.ReleaseMutex();

                byte pos = (byte)(buf[4] - 1);
                slotOccupied[pos] = false;
                state = (int)state_t.STATE_CLEAR_ERRORS;
                return;
            }

            if (isCDNotPresent(buf))
            {
                mutex.WaitOne();
                pendingEjects.RemoveAt(0);
                mutex.ReleaseMutex();

                byte pos = (byte)(buf[4] - 1);
                slotOccupied[pos] = false;
                //dbInterfaceManager->setSlotState(IMATION_OPDICOM_DISC_STAKKA_TYPE, serial, pos, false);
                state = (int)state_t.STATE_CLEAR_ERRORS;
                return;
            }

            moveAndEject(pendingEjects[0]);
            
        }
        void doWrite(byte command)
        {
            /*if (myLogger != NULL)
            {
              LOG(myLogger, "Writing %02x %02x %02x %02x %02x %02x", command, arg1, arg2, arg3, arg4, arg5);
            }
            */
            byte id = 0x7F;
            long message = (messageID + 1) & id;
            sentPacket[0] = 0x01;
            sentPacket[1] = (byte)unitID;
            sentPacket[2] = (byte)message;
            sentPacket[3] = command;

            manager.write(sentPacket, sentPacket.Length);
        }
        void doWrite(byte command, byte arg1)
        {
            /*if (myLogger != NULL)
            {
              LOG(myLogger, "Writing %02x %02x %02x %02x %02x %02x", command, arg1, arg2, arg3, arg4, arg5);
            }
            */
            byte id = 0x7F;
            long message = (messageID + 1) & id;
            sentPacket[0] = 0x01;
            sentPacket[1] = (byte)unitID;
            sentPacket[2] = (byte)message;
            sentPacket[3] = command;
            sentPacket[4] = arg1;

            manager.write(sentPacket, sentPacket.Length);
        }
        void doWrite(byte command, byte arg1, byte arg2)
        {
            /*if (myLogger != NULL)
            {
              LOG(myLogger, "Writing %02x %02x %02x %02x %02x %02x", command, arg1, arg2, arg3, arg4, arg5);
            }
            */
            byte id = 0x7F;
            long message = (messageID + 1) & id;
            sentPacket[0] = 0x01;
            sentPacket[1] = (byte)unitID;
            sentPacket[2] = (byte)message;
            sentPacket[3] = command;
            sentPacket[4] = arg1;
            sentPacket[5] = arg2;

            manager.write(sentPacket, sentPacket.Length);
        }
        void doWrite(byte command, byte arg1, byte arg2, byte arg3)
        {
            /*if (myLogger != NULL)
            {
              LOG(myLogger, "Writing %02x %02x %02x %02x %02x %02x", command, arg1, arg2, arg3, arg4, arg5);
            }
            */
            byte id = 0x7F;
            long message = (messageID + 1) & id;
            sentPacket[0] = 0x01;
            sentPacket[1] = (byte)unitID;
            sentPacket[2] = (byte)message;
            sentPacket[3] = command;
            sentPacket[4] = arg1;
            sentPacket[5] = arg2;
            sentPacket[6] = arg3;

            manager.write(sentPacket, sentPacket.Length);
        }
        void doWrite(byte command, byte arg1, byte arg2, byte arg3, byte arg4)
        {
            /*if (myLogger != NULL)
            {
              LOG(myLogger, "Writing %02x %02x %02x %02x %02x %02x", command, arg1, arg2, arg3, arg4, arg5);
            }
            */
            byte id = 0x7F;
            long message = (messageID + 1) & id;
            sentPacket[0] = 0x01;
            sentPacket[1] = (byte)unitID;
            sentPacket[2] = (byte)message;
            sentPacket[3] = command;
            sentPacket[4] = arg1;
            sentPacket[5] = arg2;
            sentPacket[6] = arg3;
            sentPacket[7] = arg4;

            manager.write(sentPacket, sentPacket.Length);
        }
        void doWrite(byte command, byte arg1, byte arg2, byte arg3, byte arg4, byte arg5)
        {
            /*if (myLogger != NULL)
            {
              LOG(myLogger, "Writing %02x %02x %02x %02x %02x %02x", command, arg1, arg2, arg3, arg4, arg5);
            }
            */
            byte id = 0x7F;
            long message = (messageID + 1) & id;
            sentPacket[0] = 0x01;
            sentPacket[1] = (byte)unitID;
            sentPacket[2] = (byte)message;
            sentPacket[3] = command;
            sentPacket[4] = arg1;
            sentPacket[5] = arg2;
            sentPacket[6] = arg3;
            sentPacket[7] = arg4;
            sentPacket[8] = arg5;

            manager.write(sentPacket, sentPacket.Length);
        }

        void requestSerial()
        {
            doWrite(CODE_SERIAL);
        }

        void move(int pos)
        {
            doWrite(CODE_MOVE, (byte)pos);
        }

        void moveAndEject(int pos)
        {
            doWrite(CODE_MOVE, (byte)pos, 1);
        }

        void setLed(byte onTime, byte cycleTime)
        {
            doWrite(CODE_LED_RATE, onTime, cycleTime);
        }

        bool moveToEmptySlot(byte[] buf)
        {
            if (isBusy(buf))
            {
                return false;
            }

            byte command = buf[3];
            
            if (slotStateKnown == false)
            {
                if (command != CODE_LED_RATE)
                {
                    //setLed(LED_UNKNOWN_SLOTS_ON, LED_UNKNOWN_SLOTS_PERIOD);
                }
                //return false;
            }

            int optimalSlot = 0;
            for (int i = 0; i < IMATION_OPDICOM_DISC_STAKKA_MAX_SLOTS; i++)
            {
                if (slotOccupied[i] == false)
                {
                    optimalSlot = i + 1;
                    break;
                }
            }

            byte pos = buf[4];
            if (pos != optimalSlot)
            {
                move(optimalSlot);
                return false;
            }

            if (command != CODE_LED_RATE)
            {
                setLed(LED_READY_ON, LED_READY_PERIOD);
                return false;
            }

            return true;
        }

        bool isBusy(byte[] buf)
        {
            int var1 = (buf[5] & 0x80);
            int var2 = (buf[6] & 0x80);
            if ((buf[5] & 0x80) !=0 || (buf[6] & 0x80) !=0)
            {
                return true;
            }

            return false;
        }

        bool isCDNotPresent(byte[] buf)
        {
            if ((buf[5] & 0x40) !=0)
            {
                return true;
            }

            return false;
        }

        bool isAwaitingNewCDAck(byte[] buf)
        {
            if ((buf[5] & 0x08) != 0)
            {
                return true;
            }

            return false;
        }

        bool isCDInMotion(byte[] buf)
        {
            if ((buf[5] & 0x04) != 0)
            {
                return true;
            }

            return false;
        }

        bool isCDRemoved(byte[] buf)
        {
            if ((buf[5] & 0x02) != 0)
            {
                return true;
            }

            return false;
        }

//        void logState(byte[] state, byte[] buf)
//{
//  if (memcmp(buf, prevLogState, 8) == 0)
//  {
//    return;
//  }
//  memcpy(prevLogState, buf, 8);
  
//  if (myLogger != NULL)
//  {
//    LOG(myLogger, "%s unitid = %d. Buffer = %02x, %02x, %02x, %02x, %02x, %02x, %02x, %02x "
//                  + "Flags [%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s]",
//          state,
//          unitID, buf[0], buf[1], buf[2], buf[3],
//          buf[4], buf[5], buf[6], buf[7],
//          buf[5] & 0x80 ? "Moving":"",
//          buf[5] & 0x40 ? "CD not present? Error?":"",
//          buf[5] & 0x20 ? "Unit 0":"",
//          buf[5] & 0x10 ? "CD in slot":"",
//          buf[5] & 0x08 ? "CD in slot - needs ack":"",
//          buf[5] & 0x04 ? "Timeout? CD in motion?":"",
//          buf[5] & 0x02 ? "CD removed":"",
//          buf[5] & 0x01 ? "???":"",
//          buf[6] & 0x80 ? "Busy":"",
//          buf[6] & 0x40 ? "???":"",
//          buf[6] & 0x20 ? "???":"",
//          buf[6] & 0x10 ? "???":"",
//          buf[6] & 0x08 ? "???":"",
//          buf[6] & 0x04 ? "???":"",
//          buf[6] & 0x02 ? "???":"",
//          buf[6] & 0x01 ? "Ok?":""
//    );
//  }
//        }






    }
}