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
using System.IO;

namespace House_of_the_Future
{
    public class DiskStakkaManager
    {
        public string database;
        const uint DISCSTAKKA_MAX_STACK_HEIGHT = 5;
        private System.Threading.Mutex mutex;
        private DiskStakka[] devices = new DiskStakka[DISCSTAKKA_MAX_STACK_HEIGHT];
        protected FileStream fs;
        //extern Logger* generalLogger;

        public DiskStakkaManager(string path)
        {
            mutex = new System.Threading.Mutex();
            for (int i = 0; i < DISCSTAKKA_MAX_STACK_HEIGHT; i++)
            {
                devices[i] = null;
            }
            USBSharp myUsb = new USBSharp();
            myUsb.CT_CreateFile(path);
                int myPtrToPreparsedData = -1;
                if (myUsb.CT_HidD_GetPreparsedData(myUsb.HidHandle, ref myPtrToPreparsedData) != 0)
                {
                    int code = myUsb.CT_HidP_GetCaps(myPtrToPreparsedData);
                    int reportLength = myUsb.myHIDP_CAPS.InputReportByteLength;
                    fs = new FileStream(new Microsoft.Win32.SafeHandles.SafeFileHandle((IntPtr)myUsb.HidHandle, false), FileAccess.ReadWrite, reportLength, true);
                }
            //LOG(generalLogger, "Imation Disc Stakka driver registered.");
        }
        public void dispose()
        {
            for (int i = 0; i < DISCSTAKKA_MAX_STACK_HEIGHT; i++)
            {
                if (devices[i] != null)
                {
                    devices[i].dispose();
                    devices[i] = null;
                }
            }
        }
        public void start()
        {
            byte[] buf = new byte[8];
            while (true)
            {
                int retval = fs.Read(buf, 0, buf.Length);
                //int retval = usbInterface.read(buf, buf.Length);
                if ((retval == 8) && (buf[0] == 0x01))
                {
                    uint unitid = buf[1];
                    if (unitid >= DISCSTAKKA_MAX_STACK_HEIGHT)
                    {
                        //LOG(generalLogger, "Invalid unit id!! unitid = %d.", unitid);
                        continue;
                    }

                    if (devices[unitid] == null)
                    {
                        devices[unitid] = new DiskStakka(unitid, this);
                        devices[unitid].database = database;
                    }
                    else
                    {
                        devices[unitid].process(buf);
                    }

                    for (int i = 0; i < DISCSTAKKA_MAX_STACK_HEIGHT; i++)
                    {
                        if (devices[i] != null)
                        {
                            if (devices[i].hasTimedOut() == true)
                            {
                                //for (int j = i; j < DISCSTAKKA_MAX_STACK_HEIGHT; j++)
                                //{
                                    devices[i].dispose();
                                    devices[i] = null;
                                //}
                                break;
                            }
                        }
                    }
                }
                else if ((retval == 0) || (retval == -116))
                {
                    // Probably a timeout. We don't care
                }
                else if (retval == -19)
                {
                    throw new Exception("Device gone away.");
                }
                else
                {
                    //ERR(generalLogger, "Unsupported retval = %d", retval);
                }
            }
        }

        public void setUsedSlots(int serial, List<int> state)
        {
            mutex.WaitOne();
            for (int i = 0; i < DISCSTAKKA_MAX_STACK_HEIGHT; i++)
            {
                if (devices[i] == null)
                {
                    break;
                }

                if (devices[i].getSerial() == serial)
                {
                    devices[i].setUsedSlots(state);
                    break;
                }
            }
            mutex.ReleaseMutex();
        }

        public void setCommand(int type, int serial, List<DiskStakka.Command> command)
        {
            mutex.WaitOne();
            for (int i = 0; i < DISCSTAKKA_MAX_STACK_HEIGHT; i++)
            {
                if (devices[i] == null)
                {
                    break;
                }

                if (devices[i].unitID == serial)
                {
                    devices[i].setCommand(command);
                    break;
                }
            }
            mutex.ReleaseMutex();

        }

        public void write(byte[] buf, int size)
        {
            fs.Write(buf, 0, size);
        }
    }

}