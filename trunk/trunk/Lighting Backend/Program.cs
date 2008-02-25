using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using ControlThink.ZWave;
using System.IO;

namespace Lighting_Backend
{
    class Program
    {
        static System.Threading.Timer timer;
        static object IsUpdating;
        static ZWaveController controller;
        static DateTime dbUpdated;
        static string database;

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
            temp = null;
            database = temp.Substring(temp.IndexOf("database") + 9, temp.IndexOf("\r", temp.IndexOf("database")) - temp.IndexOf("database") - 9).Trim();

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
    }
}
