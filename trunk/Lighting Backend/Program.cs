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

namespace House_of_the_Future
{
    class Program
    {
        static System.Threading.Timer timer;
        static object IsUpdating;
        static ZWaveController controller;
        static DateTime dbUpdated;
        static string database;
        static DiskStakkaManager manager;
        static string path;
        static USBSharp myUsb;
        static int id = 0;

        class state
        {
            public System.Threading.Timer timer;
        }
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "lighting.ini");
            string temp = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            sr = null;
            
            database = temp.Substring(temp.IndexOf("database") + 9, temp.IndexOf("\r", temp.IndexOf("database")) - temp.IndexOf("database") - 9).Trim();
            temp = null;
            myUsb = new USBSharp();

            find_stakka();
                        
            IsUpdating = new object();
            dbUpdated = DateTime.Now;
            state state = new state();
            System.Threading.TimerCallback timerDelegate = new System.Threading.TimerCallback(timer_tick);
            

            controller = new ZWaveController();
            controller.Connect();
            for (int i = 0; i < controller.Devices.Count; i++)
            {
                controller.Devices[i].PollEnabled = true;
                controller.Devices[i].LevelChanged += new ControlThink.ZWave.Devices.ZWaveDevice.LevelChangedEventHandler(Program_LevelChanged);
            }
            timer = new System.Threading.Timer(timerDelegate, state, 0, 100);
            state.timer = timer;
        start:
            string input = Console.ReadLine();
            if (input.ToLower() == "reconnect")
            {
                try
                {
                    if (controller.IsConnected)
                    {
                        controller.Disconnect();
                    }
                    controller.Connect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            if (input.ToLower() == "add")
            {
                try
                {
                    if (!controller.IsConnected)
                    {
                        controller.Connect();
                    }
                    Console.WriteLine("Press the connect button on the top of the device.");
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
            if (input.ToLower() == "remove")
            {
                try
                {
                    if (!controller.IsConnected)
                    {
                        controller.Connect();
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
            if (input.ToLower() != "exit")
            {
                goto start;
            }
            
            if (controller.IsConnected)
            {
                controller.Disconnect();
            }
            input = null;
        }

        static void Program_LevelChanged(object sender, ControlThink.ZWave.Devices.LevelChangedEventArgs e)
        {
            lock (IsUpdating)
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("update light_values set value = " + e.Level + " where node_id = " + ((ControlThink.ZWave.Devices.ZWaveDevice)sender).NodeID + ";", conn);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                da.Dispose();
                da = null;
                conn.Dispose();
                conn = null;
            }
        }
        static void timer_tick(Object State)
        {
            System.IO.FileInfo info = new System.IO.FileInfo(database);
            if (info.LastWriteTime > dbUpdated)
            {
                lock(IsUpdating)
                {
                    SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                    SQLiteDataAdapter da = new SQLiteDataAdapter("select * from light_values;", conn);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    da.Fill(dt);
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
                        if (!bFound)
                        {
                            da = new SQLiteDataAdapter("delete from light_values where node_id = " + dt.Rows[i]["node_id"].ToString() + ";", conn);
                            da.Fill(new System.Data.DataSet());
                        }
                    }
                    da = new SQLiteDataAdapter("select * from light_values;", conn);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        try
                        {
                            controller.Devices.GetByNodeID((byte)int.Parse(dt.Rows[i]["node_id"].ToString())).Level = (byte)int.Parse(dt.Rows[i]["value"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            ex = null;
                        }
                    }
                    da = new SQLiteDataAdapter("select * from pending_ejects;", conn);
                    dt = new System.Data.DataTable();
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        List<DiskStakka.Command> commands = new List<DiskStakka.Command>();
                        DiskStakka.Command command = new DiskStakka.Command();
                        command.command = 1;
                        command.opid = id;
                        command.slot = int.Parse(dt.Rows[i]["slot"].ToString());
                        commands.Add(command);
                        manager.setCommand(1, int.Parse(dt.Rows[i]["unit_id"].ToString()), commands);
                        id++;
                    }
                    da = new SQLiteDataAdapter("delete from pending_ejects;", conn);
                    da.Fill(dt);
                    dt.Dispose();
                    dt = null;
                    da.Dispose();
                    da = null;
                    conn.Dispose();
                    conn = null;
                }
                dbUpdated = info.LastWriteTime;
                info = null;
            }
        }

        static void find_stakka()
        {
            int my_device_count = 0;
            string my_device_path = string.Empty;

            using (USBSharp myUsb = new USBSharp())
            {
                myUsb.CT_HidGuid();
                myUsb.CT_SetupDiGetClassDevs();

                int result = -1;
                int device_count = 0;
                int size = 0;
                int requiredSize = 0;

                while (result != 0)
                {
                    result = myUsb.CT_SetupDiEnumDeviceInterfaces(device_count);
                    int resultb = myUsb.CT_SetupDiGetDeviceInterfaceDetail(ref requiredSize, 0);
                    size = requiredSize;
                    resultb = myUsb.CT_SetupDiGetDeviceInterfaceDetailx(ref requiredSize, size);

                    if (myUsb.DevicePathName.IndexOf("vid_0718&pid_d000") > 0)
                    {
                        my_device_count = device_count;
                        path = myUsb.DevicePathName;
                        break;
                    }
                    device_count++;
                }

                if (path == string.Empty)
                {
                    Exception devNotFound = new Exception(@"Device could not be found.");
                    throw (devNotFound);
                }
            }

            if (path != string.Empty)
            {

                myUsb.CT_CreateFile(path);
                int myPtrToPreparsedData = -1;
                if (myUsb.CT_HidD_GetPreparsedData(myUsb.HidHandle, ref myPtrToPreparsedData) != 0)
                {
                    int code = myUsb.CT_HidP_GetCaps(myPtrToPreparsedData);
                    int reportLength = myUsb.myHIDP_CAPS.InputReportByteLength;
                    manager = new DiskStakkaManager(path);
                    manager.database = database;
                    System.Threading.ThreadStart job = new System.Threading.ThreadStart(manager.start);
                    System.Threading.Thread t = new System.Threading.Thread(job);
                    t.Start();
                    SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                    SQLiteDataAdapter da = null;
                    for (int i = 0; i < 5; i++)
                    {
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
