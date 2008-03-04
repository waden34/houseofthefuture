/*
 * Copyright (C) 2008 Jeremiah Johnson
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
using System.Linq;
using System.Text;
using System.Data.SQLite;
using ControlThink.ZWave;
using System.IO;
using Microsoft.Win32;

namespace HouseOfTheFuture
{
    class Program
    {
        #region Variables
        /// <summary>
        /// Timer to check the current light levels and pending ejects
        /// </summary>
        static System.Threading.Timer timer;
        /// <summary>
        /// Used to allow only one thread to update the database at a time
        /// </summary>
        static object IsUpdating;
        /// <summary>
        /// ZWave controller
        /// </summary>
        static ZWaveController controller;
        /// <summary>
        /// Time the database was last updated
        /// </summary>
        static DateTime dbUpdated;
        /// <summary>
        /// Full path to the database file
        /// </summary>
        static string database;
        /// <summary>
        /// Main Disk Stakka listener
        /// </summary>
        static DiskStakkaManager manager;
        /// <summary>
        /// HID path for the Disk Stakka
        /// </summary>
        static string path;
        /// <summary>
        /// Used to allow only one thread to process pending ejects
        /// </summary>
        static object ejecting;
        /// <summary>
        /// # of communications with the Disk Stakka
        /// </summary>
        static int id = 0;
        /// <summary>
        /// How much information will be logged, 2 = Error Only, 1 = Errors & Warnings, 0 = All debug info
        /// </summary>
        static int logLevel = 0;
        /// <summary>
        /// State object for the timer 
        /// </summary>
        class state
        {
            public System.Threading.Timer timer;
        }
        #endregion

        /// <summary>
        /// Main Program Loop
        /// </summary>
        /// <param name="args">Not processed, will always return null</param>
        static void Main(string[] args)
        {
            //Read the registry to get config info

            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\House of the Future", true);
            if (key == null)
            {
                Console.WriteLine("Creating Registry keys");
                key = Registry.LocalMachine.CreateSubKey("Software\\House of the Future");
                key = Registry.LocalMachine.OpenSubKey("Software\\House of the Future", true);
                key.SetValue("database", AppDomain.CurrentDomain.BaseDirectory + "\\jHome.jdb");
                System.Net.IPAddress[] ips = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());
                key.SetValue("irCommander", ips[0].ToString());
                key.SetValue("logLevel", "2");
                ips = null;
            }
            if (logLevel < 2)
            {
                Console.WriteLine("Reading Registry keys");
            }
            database = key.GetValue("database").ToString();
            logLevel = int.Parse(key.GetValue("logLevel").ToString());
            key = null;

            //Find the Disk Stakka and start the listener
            find_stakka();
            ejecting = true;
            
            IsUpdating = new object();
            //Set the database last update time = now
            dbUpdated = DateTime.Now;
            
            //Create and connect the ZWave controller
            controller = new ZWaveController();
            try
            {
                if (logLevel == 0)
                {
                    Console.WriteLine("Connecting to ZWave controller");
                }
                controller.Connect();
                if (logLevel < 2)
                {
                    Console.WriteLine("Successfully connected to ZWave controller");
                }
                for (int i = 0; i < controller.Devices.Count; i++)
                {
                    //Set each ZWave device (excluding the controller) to Poll and create the event handler to respond to Level changes
                    controller.Devices[i].PollEnabled = true;
                    controller.Devices[i].LevelChanged += new ControlThink.ZWave.Devices.ZWaveDevice.LevelChangedEventHandler(ZWaveDevice_LevelChanged);
                    if (logLevel == 0)
                    {
                        Console.WriteLine("Successfully enabled Device: " + controller.Devices[i].NodeID.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Set up the Timer
            state state = new state();
            System.Threading.TimerCallback timerDelegate = new System.Threading.TimerCallback(timer_tick);

            timer = new System.Threading.Timer(timerDelegate, state, 0, 100);
            state.timer = timer;
            if (logLevel == 0)
            {
                Console.WriteLine("Started Timer");
            }
        start:
            //Wait for user input
            string input = Console.ReadLine();
            //Reconnect the ZWave controller
            if (input.ToLower() == "reconnect")
            {
                try
                {
                    
                    if (controller.IsConnected)
                    {
                        if (logLevel < 2)
                        {
                            Console.WriteLine("Disconnecting ZWave controller");
                        }
                        controller.Disconnect();
                        if (logLevel == 0)
                        {
                            Console.WriteLine("ZWave controller disconnected successfully");
                        }
                    }
                    if (logLevel < 2)
                    {
                        Console.WriteLine("Reconnecting ZWave controller");
                    }
                    controller.Connect();
                    if (logLevel == 0)
                    {
                        Console.WriteLine("ZWave controller reconnected successfully");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //Add a new ZWave device to the controller
            else if (input.ToLower() == "add")
            {
                try
                {
                    if (!controller.IsConnected)
                    {
                        if (logLevel < 2)
                        {
                            Console.WriteLine("Reconnecting ZWave controller");
                        }
                        controller.Connect();
                        if (logLevel == 0)
                        {
                            Console.WriteLine("ZWave controller reconnected successfully");
                        }
                    }
                    Console.WriteLine("Press the connect button on the top of the device.");
                    //Process the new ZWave device
                    ControlThink.ZWave.Devices.ZWaveDevice device = controller.AddDevice();
                    device.PollEnabled = true;
                    Console.WriteLine("Node " + device.NodeID.ToString() + " added successfully");
                    device.Dispose();
                    device = null;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //Remove a ZWave device from the controller
            else if (input.ToLower() == "remove")
            {
                try
                {
                    if (!controller.IsConnected)
                    {
                        if (logLevel < 2)
                        {
                            Console.WriteLine("Reconnecting ZWave controller");
                        }
                        controller.Connect();
                        if (logLevel == 0)
                        {
                            Console.WriteLine("ZWave controller reconnected successfully");
                        }
                    }
                    Console.WriteLine("Press the connect button on the top of the device.");
                    ControlThink.ZWave.Devices.ZWaveDevice device = controller.RemoveDevice();
                    device.PollEnabled = true;
                    Console.WriteLine("Node " + device.NodeID.ToString() + " removed successfully");
                    device.Dispose();
                    device = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //Restart the input loop if we didn't receive an Exit command
            else if (input.ToLower() != "exit")
            {
                Console.WriteLine("Unknown Command: " + input);
                goto start;
            }

            if (controller.IsConnected)
            {
                if (logLevel < 2)
                {
                    Console.WriteLine("Disconnecting ZWave controller");
                }
                controller.Disconnect();
                if (logLevel == 0)
                {
                    Console.WriteLine("ZWave controller disconnected successfully");
                }
            }
            input = null;
        }

        /// <summary>
        /// Fires when a ZWave device level gets changed
        /// </summary>
        /// <param name="sender">ZWave Device that had its level changed</param>
        /// <param name="e">Event Args holding level change info</param>
        static void ZWaveDevice_LevelChanged(object sender, ControlThink.ZWave.Devices.LevelChangedEventArgs e)
        {
            //Lock the routine so only one thread can access it
            lock (IsUpdating)
            {
                //Update the database with the new Level for this Device
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("update light_values set value = " + e.Level + " where node_id = " + ((ControlThink.ZWave.Devices.ZWaveDevice)sender).NodeID + ";", conn);
                System.Data.DataTable dt = new System.Data.DataTable();
                if (logLevel < 2)
                {
                    Console.WriteLine("Setting Device: " + ((ControlThink.ZWave.Devices.ZWaveDevice)sender).NodeID.ToString() + " to " + e.Level.ToString() + " in the database");
                }
                da.Fill(dt);
                if (logLevel == 0)
                {
                    Console.WriteLine("Update Successful");
                }
                dt.Dispose();
                dt = null;
                da.Dispose();
                da = null;
                conn.Dispose();
                conn = null;
            }
        }
        
        /// <summary>
        /// Timer that checks the database for changes
        /// </summary>
        /// <param name="State"></param>
        static void timer_tick(Object State)
        {
            //Get the Last Modified Date of the database file
            System.IO.FileInfo info = new System.IO.FileInfo(database);
            //Lock the routine so only one thread can access it
            lock (IsUpdating)
            {
                //If the Last Modified Date is greater than the last time we checked, continue
                if (info.LastWriteTime > dbUpdated)
                {
                    if (logLevel < 2)
                    {
                        Console.WriteLine("Pulling light levels from the database");
                    }
                    //Get the list of all light values from the database
                    SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                    SQLiteDataAdapter da = new SQLiteDataAdapter("select * from light_values;", conn);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    da.Fill(dt);
                    //Check for removed Devices that are still in the database.  Remove them from the database if found
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        bool bFound = false;
                        for (int j = 0; j < controller.Devices.Count; j++)
                        {
                            if (controller.Devices[j].NodeID == (byte)int.Parse(dt.Rows[i]["node_id"].ToString()))
                            {
                                bFound = true;
                                break;
                            }
                        }
                        if (!bFound && controller.IsConnected)
                        {
                            Console.WriteLine("Removing Device: " + dt.Rows[i]["node_id"].ToString() + " from the database");
                            da = new SQLiteDataAdapter("delete from light_values where node_id = " + dt.Rows[i]["node_id"].ToString() + ";", conn);
                            da.Fill(new System.Data.DataSet());
                            if (logLevel == 0)
                            {
                                Console.WriteLine("Successfully removed Device: " + dt.Rows[i]["node_id"].ToString() + " from the database");
                            }
                        }
                    }
                    //Get the clean list of all light values from the database
                    da = new SQLiteDataAdapter("select * from light_values;", conn);
                    da.Fill(dt);
                    if (logLevel < 2)
                    {
                        Console.WriteLine("Updating the light levels on the Devices");
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        try
                        {
                            //Set the Level of the Device to what is in the database
                            controller.Devices.GetByNodeID((byte)int.Parse(dt.Rows[i]["node_id"].ToString())).Level = (byte)int.Parse(dt.Rows[i]["value"].ToString());
                            if (logLevel == 0)
                            {
                                Console.WriteLine("Successfully updated Device: " + dt.Rows[i]["node_id"].ToString() + " to " + dt.Rows[i]["value"].ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            ex = null;
                        }
                    }
                    //Process any Pending Ejects, lock so only one thread can access it
                    lock (ejecting)
                    {
                        if (logLevel < 2)
                        {
                            Console.WriteLine("Processing Pending Ejects");
                        }
                        if (manager != null)
                        {
                            da = new SQLiteDataAdapter("select * from pending_ejects;", conn);
                            dt = new System.Data.DataTable();
                            da.Fill(dt);
                            //Create a command for each pending eject and send it to the Disk Stakka listener
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                List<DiskStakka.Command> commands = new List<DiskStakka.Command>();
                                DiskStakka.Command command = new DiskStakka.Command();
                                command.command = 1;
                                command.opid = id;
                                command.slot = int.Parse(dt.Rows[i]["slot"].ToString());
                                commands.Add(command);
                                manager.setCommand(1, int.Parse(dt.Rows[i]["unit_id"].ToString()), commands);
                                if (logLevel == 0)
                                {
                                    Console.WriteLine("Ejecting slot " + command.slot.ToString() + " from unit " + dt.Rows[i]["unit_id"].ToString());
                                }
                                id++;
                            }
                            //Clear the pending ejects table since we've processed them
                            if (logLevel < 2)
                            {
                                Console.WriteLine("Clearing the pending_ejects table");
                            }
                            da = new SQLiteDataAdapter("delete from pending_ejects;", conn);
                            da.Fill(dt);
                            if (logLevel == 0)
                            {
                                Console.WriteLine("Successfully cleared the pending_ejects table");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Can't process Pending Ejects.  Not connected to a Disk Stakka");
                        }
                    }
                    dt.Dispose();
                    dt = null;
                    da.Dispose();
                    da = null;
                    conn.Dispose();
                    conn = null;
                    //Set our Last Modified variable to the Last Modified Time of the database 
                    dbUpdated = info.LastWriteTime;
                    if (logLevel == 0)
                    {
                        Console.WriteLine("Database Last Write Time = " + dbUpdated.ToString());
                    }
                    info = null;
                }
            }
        }
        
        /// <summary>
        /// Find the HID handle for the Disk Stakka tower
        /// </summary>
        static void find_stakka()
        {
            //Create a new USB Controller
            using (USBSharp myUsb = new USBSharp())
            {
                if (logLevel < 2)
                {
                    Console.WriteLine("Finding Disk Stakka");
                }
                myUsb.CT_HidGuid();
                //Get all HID devices connected
                myUsb.CT_SetupDiGetClassDevs();

                int result = -1;
                int device_count = 0;
                int size = 0;
                int requiredSize = 0;
                //Enumerate through the HID Devices to find the Stakka
                while (result != 0)
                {
                    result = myUsb.CT_SetupDiEnumDeviceInterfaces(device_count);
                    int resultb = myUsb.CT_SetupDiGetDeviceInterfaceDetail(ref requiredSize, 0);
                    size = requiredSize;
                    resultb = myUsb.CT_SetupDiGetDeviceInterfaceDetailx(ref requiredSize, size);
                    //Stakka Vid = 718, Pid = d000
                    if (myUsb.DevicePathName.IndexOf("vid_0718&pid_d000") > 0)
                    {
                        //Set the path = Path for the found Stakka
                        if (logLevel == 0)
                        {
                            Console.WriteLine("Found Disk Stakka at " + myUsb.DevicePathName);
                        }
                        path = myUsb.DevicePathName;
                        break;
                    }
                    device_count++;
                }

                if (path == null)
                {
                    Console.WriteLine("No Disk Stakka found");
                }
                else 
                {
                    //Create a File Path for the HID device
                    myUsb.CT_CreateFile(path);
                    int myPtrToPreparsedData = -1;
                    //If we know about the device and it is ready to go...
                    if (myUsb.CT_HidD_GetPreparsedData(myUsb.HidHandle, ref myPtrToPreparsedData) != 0)
                    {
                        //Create and start a new Disk Stakka listner for the HID device
                        if (logLevel < 2)
                        {
                            Console.WriteLine("Starting Disk Stakka listener");
                        }
                        manager = new DiskStakkaManager(path);
                        manager.database = database;
                        System.Threading.ThreadStart job = new System.Threading.ThreadStart(manager.start);
                        System.Threading.Thread t = new System.Threading.Thread(job);
                        t.Start();
                        //Get the slot states from the database and inform each of the Stakkas
                        if (logLevel < 2)
                        {
                            Console.WriteLine("Setting slot info for each Stakka");
                        }
                        SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                        SQLiteDataAdapter da = null;
                        for (int i = 0; i < 5; i++)
                        {
                            if (logLevel == 0)
                            {
                                Console.WriteLine("Sending slot info to Stakka " + ((int)(i + 1)).ToString());
                            }
                            da = new SQLiteDataAdapter("select slot from discs where unit_id = " + i + " and ejected = 0;", conn);
                            List<int> state = new List<int>();
                            System.Data.DataTable dt = new System.Data.DataTable();
                            da.Fill(dt);
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                state.Add(int.Parse(dt.Rows[j]["slot"].ToString()));
                            }
                            manager.setUsedSlots(i, state);
                            dt.Dispose();
                            dt = null;
                        }
                        da.Dispose();
                        da = null;
                        conn.Dispose();
                        conn = null;

                    }
                }
            }
        }

    }
}
