using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using System.Net;
using System.Net.Sockets;

namespace Lighting_Interface
{
    public partial class frmMain : Form
    {
        string activePanel;
        string database;
        string irCommander;
        Socket gcSocket;
        bool lockUpper;
        bool lockLower;
        Label curLabel;
        bool holding;
        string activeDevice;
        string activeDisplayName;
        int iCount;
        int id;
        object lockIR;
        object lockInserts;
        public bool isInserting;
        public frmMain()
        {
            InitializeComponent();
        }

        #region Timers

        private void timerPing_Tick(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new DataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < this.Controls.Count; j++)
                {
                    if (this.Controls[j].GetType() == typeof(TrackBar))
                    {
                        if (((TrackBar)this.Controls[j]).Name == "tbNode" + dt.Rows[i]["node_id"].ToString())
                        {
                            ((TrackBar)this.Controls[j]).Value = int.Parse(dt.Rows[i]["value"].ToString());
                        }
                    }
                }
            }
            if (DateTime.Now.Second % 2 == 0)
            {
                lblTime.Text = DateTime.Now.ToShortTimeString();
            }
            else
            {
                lblTime.Text = DateTime.Now.ToShortTimeString().Replace(":", " ");
            }
            lblTime.Left = 631 + (panel1.Width - lblTime.Width) / 2;
           if (!isInserting)
           {
                isInserting = true;
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("Select * from pending_inserts;", conn);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    frmDiscInsert frm = new frmDiscInsert();
                    frm.database = database;
                    frm.unit_id = dt.Rows[0]["unit_id"].ToString();
                    frm.pos = dt.Rows[0]["slot"].ToString();
                    frm.frmMain = this;
                    frm.ShowDialog();
                }
                else
                {
                    isInserting = false;
                }
                dt.Dispose();
                dt = null;
                da.Dispose();
                da = null;
                conn.Dispose();
                conn = null;
            }
        }

        #region Animation Timers

        private void timerSlider_Tick(object sender, EventArgs e)
        {
            if (panel1.Left == 631)
            {
                timerSlider.Enabled = false;
                return;
            }
            panel1.Left -= 30;
        }

        private void timerSlideBack_Tick(object sender, EventArgs e)
        {
            if (panel1.Left == 781)
            {
                timerSlideBack.Enabled = false;
                return;
            }
            panel1.Left += 30;
        }

        private void timerContentSlider_Tick(object sender, EventArgs e)
        {
            foreach (Control ctl in this.Controls)
            {
                if (ctl.GetType() == typeof(Panel))
                {
                    if (((Panel)ctl).Name != activePanel && ((Panel)ctl).Name != "panel1")
                    {
                        if (((Panel)ctl).Left > -691)
                        {
                            ((Panel)ctl).Left -= 30;
                        }
                    }
                    else if (((Panel)ctl).Name == activePanel)
                    {
                        if (((Panel)ctl).Left < 1)
                        {
                            ((Panel)ctl).Left += 30;
                        }
                        else
                        {
                            timerContentSlider.Enabled = false;
                        }
                    }
                }
            }
        }

        private void timerSetupSlider_Tick(object sender, EventArgs e)
        {
            foreach (Control ctl in this.panelSetup.Controls)
            {
                if (ctl.GetType() == typeof(Panel))
                {
                    if (((Panel)ctl).Name != activePanel && ((Panel)ctl).Name != "panel1")
                    {
                        if (((Panel)ctl).Left > -691)
                        {
                            ((Panel)ctl).Left -= 30;
                        }
                    }
                    else if (((Panel)ctl).Name == activePanel)
                    {
                        if (((Panel)ctl).Left < 1)
                        {
                            ((Panel)ctl).Left += 30;
                        }
                        else
                        {
                            timerSetupSlider.Enabled = false;
                        }
                    }
                }
            }
        }

        #endregion

        private void timerLock_Tick(object sender, EventArgs e)
        {
            lockUpper = false;
            lockLower = false;
            timerLock.Enabled = false;
        }
        #endregion

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.btnActivitySetup.Left = ((this.panelSetup.Width - this.btnActivitySetup.Width) / 2);
            this.btnLightingSetup.Left = (this.panelSetup.Width - this.btnLightingSetup.Width - 11);
            this.btnDeviceSetup.Left = 11;
            this.btnAddDevice.Left = (this.panelSetupDevices.Width - this.btnAddDevice.Width) / 2;
            this.btnAddActivity.Left = (this.panelSetupActivities.Width - this.btnAddActivity.Width) / 2;
            
            lockIR = new object();
            lockInserts = new object();
            isInserting = false;
            StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "lighting.ini");
            string temp = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            sr = null;
            database = temp.Substring(temp.IndexOf("database") + 9, temp.IndexOf("\r", temp.IndexOf("database")) - temp.IndexOf("database") - 9).Trim();
            irCommander = temp.Substring(temp.IndexOf("irCommander") + 12, temp.IndexOf("\r", temp.IndexOf("irCommander")) - temp.IndexOf("irCommander") - 12).Trim();
            temp = null;
            id = 1;
            gcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            gcSocket.Connect(irCommander, 4998);
  
            ToolTip t = new ToolTip();
            t.SetToolTip(lblTime, DateTime.Now.ToShortDateString());
            this.MouseMove += new MouseEventHandler(frmMain_MouseMove);
        }

        void frmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X > 730 && timerSlideBack.Enabled == false)
            {
                timerSlider.Enabled = true;
            }
            else if (timerSlider.Enabled == false)
            {
                timerSlideBack.Enabled = true;
            }
        }


        void btnSetup_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelSetup";
            timerContentSlider.Enabled = true;
        }
        void btnLighting_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelLighting";
            timerContentSlider.Enabled = true;
        }
        void btnDevices_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelDevices";
            timerContentSlider.Enabled = true;
            for (int i = 0; i < panelDevices.Controls.Count; i++)
            {
                panelDevices.Controls.RemoveAt(i);
                i--;
            }
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select manufacturer, type, model, device_id from devices join device_type on device_type.type_id = devices.type_id join manufacturer on manufacturer.manufacturer_id = devices.manufacturer_id;", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iTop = 30;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Button btn = new Button();
                btn.Text = dt.Rows[i]["manufacturer"].ToString() + " " + dt.Rows[i]["type"].ToString() + " - " + dt.Rows[i]["model"].ToString();
                btn.Top = iTop;

                btn.Font = new Font("Cooper Black", 10, FontStyle.Regular);
                btn.Width = 30;
                btn.FlatStyle = FlatStyle.Flat;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.SteelBlue;
                btn.Visible = true;
                btn.Click += new EventHandler(lbl_Click);
                btn.Name = "btnDevice" + dt.Rows[i]["device_id"].ToString();
                btn.ContextMenuStrip = contextMenuStrip1;
                
                panelDevices.Controls.Add(btn);
                btn.AutoSize = true;
                btn.AutoEllipsis = false;
                if (i % 2 == 0)
                {
                    btn.Left = 10;
                }
                else
                {
                    btn.Left = panelDevices.Width - btn.Width - 10;
                }
                if (i % 2 > 0)
                {
                    iTop += ((panelDevices.Height - 90) / ((dt.Rows.Count - 1) / 2));
                }

            }
            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
        }

        void lbl_Click(object sender, EventArgs e)
        {
            string device = ((Button)sender).Name.Substring(9).Trim();
            string model = ((Button)sender).Text;
            for (int i = 0; i < panelDevices.Controls.Count; i++)
            {
                panelDevices.Controls.RemoveAt(i);
                i--;
            }
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select distinct display_name from ir_commands where device_id = " + device + ";", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Label lbl = new Label();
            lbl.Top = 20;
            lbl.Text = model;
            lbl.Font = new Font("Cooper Black", 10, FontStyle.Regular);
            lbl.ForeColor = Color.White;
            
            panelDevices.Controls.Add(lbl);
            lbl.AutoSize = true;
            lbl.Left = ((panelDevices.Width - lbl.Width) / 2);

            int iTop = 50;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Button btn = new Button();
                btn.Text = dt.Rows[i]["display_name"].ToString();
                btn.Top = iTop;
                btn.Font = new Font("Cooper Black", 10, FontStyle.Regular);
                btn.Width = 300;
                btn.FlatStyle = FlatStyle.Flat;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.SteelBlue;
                btn.Tag = device;
                btn.Visible = true;
                btn.MouseDown += new MouseEventHandler(btn_MouseDown);
                btn.MouseUp += new MouseEventHandler(btn_MouseUp);
                panelDevices.Controls.Add(btn);
                btn.AutoSize = true;
                btn.AutoEllipsis = false;
                if (i % 2 == 0)
                {
                    btn.Left = 10;
                }
                else
                {
                    btn.Left = panelDevices.Width - btn.Width - 10;
                }

                if (i % 2 > 0)
                {
                    iTop += ((panelDevices.Height - 90) / ((dt.Rows.Count - 1) / 2));
                }
            }
            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
        }

        void btn_MouseUp(object sender, MouseEventArgs e)
        {
            holding = false;
        }

        void btn_MouseDown(object sender, MouseEventArgs e)
        {
            iCount = 0;
            activeDevice = ((Button)sender).Tag.ToString() ;
            activeDisplayName = ((Button)sender).Text;
            holding = true;
            System.Threading.ThreadStart job = new System.Threading.ThreadStart(CommandClick);
            System.Threading.Thread t = new System.Threading.Thread(job);
            t.Start();

        }

        private void CommandClick()
        {
            lock (lockIR)
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("Select command, repeat, frequency, emitter_id, hold, delay from ir_commands join devices on devices.device_id = ir_commands.device_id where devices.device_id = " + activeDevice + " and display_name = \'" + activeDisplayName + "\';", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                try
                {
                    while (holding || iCount == 0)
                    {
                        iCount++;
                        if (dt.Rows[0]["hold"].ToString() == "False")
                        {
                            holding = false;
                        }
                        if (!gcSocket.Connected)
                        {
                            gcSocket.Connect(irCommander, 4998);
                        }
                        byte[] bytestoSend;
                        bytestoSend = System.Text.Encoding.Default.GetBytes("sendir,2:" + dt.Rows[0]["emitter_id"].ToString() + "," + id + "," + dt.Rows[0]["frequency"].ToString() + "," + dt.Rows[0]["repeat"].ToString() + "," + dt.Rows[0]["command"].ToString().Replace("\r", "") + "\r");
                        gcSocket.Send(bytestoSend);
                        id++;
                        if (gcSocket.Available > 0)
                        {
                            byte[] receivedBytes = new byte[gcSocket.Available];
                            gcSocket.Receive(receivedBytes);
                            string temp = System.Text.Encoding.Default.GetString(receivedBytes);
                        }
                        System.Threading.Thread.Sleep(int.Parse(dt.Rows[0]["delay"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                dt.Dispose();
                dt = null;
                da.Dispose();
                da = null;
                conn.Dispose();
                conn = null;
            }
        }


        void btnActivities_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelActivities";
            timerContentSlider.Enabled = true;
            for (int i = 0; i < panelActivities.Controls.Count; i++)
            {
                panelActivities.Controls.RemoveAt(i);
                i--;
            }
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select name, activity_id from activities;", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iTop = 30;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Button btn = new Button();
                btn.Text = dt.Rows[i]["name"].ToString();
                btn.Top = iTop;
                btn.Font = new Font("Cooper Black", 10, FontStyle.Regular);
                btn.Width = 30;
                btn.FlatStyle = FlatStyle.Flat;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.SteelBlue;
                btn.Visible = true;
                btn.Click += new EventHandler(activity_click);
                btn.Name = "btnActivity" + dt.Rows[i]["activity_id"].ToString();
                btn.ContextMenuStrip = contextMenuStrip1;

                panelActivities.Controls.Add(btn);
                btn.AutoSize = true;
                btn.AutoEllipsis = false;
                if (i % 2 == 0)
                {
                    btn.Left = 10;
                }
                else
                {
                    btn.Left = panelActivities.Width - btn.Width - 10;
                }
                if (i % 2 > 0)
                {
                    int l = (panelActivities.Height - 90);
                    int ll = ((dt.Rows.Count - 1) / 2);
                    iTop += (int)((panelActivities.Height - 90) / (float)((dt.Rows.Count - 1) / 2));
                }

            }
            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
        }

        void activity_click(object sender, EventArgs e)
        {
            string activity = ((Button)sender).Name.Substring(11);
            for (int i = 0; i < panelActivities.Controls.Count; i++)
            {
                panelActivities.Controls.RemoveAt(i);
                i--;
            }
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select distinct activity_startup.device_id, activity_startup.command_id, display_name from activity_startup join ir_commands on ir_commands.command_id = activity_startup.command_id and ir_commands.device_id = activity_startup.device_id where activity_id = " + activity + ";", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lock (lockIR)
                {
                    iCount = 0;
                    activeDevice = dt.Rows[i]["activity_startup.device_id"].ToString();
                    activeDisplayName = dt.Rows[i]["display_name"].ToString();
                    holding = true;
                    System.Threading.ThreadStart job = new System.Threading.ThreadStart(CommandClick);
                    System.Threading.Thread t = new System.Threading.Thread(job);
                    //t.Start();
                    System.Threading.Thread.Sleep(1000);
                }
            }
            da = new SQLiteDataAdapter("select x,y,width,height,type,display_name, ir_commands.device_id, ir_commands.command_id from activity_buttons join ir_commands on ir_commands.command_id = activity_buttons.command_id where activity_id = " + activity + ";", conn);
            dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Buttons button = new Buttons();
                button.Caption = dt.Rows[i]["display_name"].ToString();
                button.Type = (Buttons.ButtonType)(int.Parse(dt.Rows[i]["type"].ToString()));
                button.Left = int.Parse(dt.Rows[i]["x"].ToString());
                button.Top = int.Parse(dt.Rows[i]["y"].ToString());
                
                button.Tag = dt.Rows[i]["device_id"].ToString();
                button.Width = int.Parse(dt.Rows[i]["width"].ToString());
                button.Height = int.Parse(dt.Rows[i]["height"].ToString());
                button.MouseDown += new MouseEventHandler(button_MouseDown);
                button.MouseUp += new MouseEventHandler(button_MouseUp);
                panelActivities.Controls.Add(button);
                button.Invalidate();
            }
            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
        }

        void button_MouseUp(object sender, MouseEventArgs e)
        {
            holding = false;
        }

        void button_MouseDown(object sender, MouseEventArgs e)
        {
            holding = true;
            activeDevice = ((Buttons)sender).Tag.ToString();
            activeDisplayName = ((Buttons)sender).Caption;
            holding = true;
            System.Threading.ThreadStart job = new System.Threading.ThreadStart(CommandClick);
            System.Threading.Thread t = new System.Threading.Thread(job);
            t.Start();
        }

        public void btnDeviceSetup_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelSetupDevices";
            timerSetupSlider.Enabled = true;
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("select manufacturer, type, model, emitter_id, device_id from devices "
            + "join device_type on device_type.type_id = devices.type_id "
            + "join manufacturer on manufacturer.manufacturer_id = devices.manufacturer_id;", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Button btn = new Button();

                btn.Text = dt.Rows[i]["manufacturer"].ToString() + " " + dt.Rows[i]["type"].ToString() + " - " + dt.Rows[i]["model"].ToString();
                btn.Visible = true;
                btn.Tag = dt.Rows[i]["device_id"].ToString();
                btn.Font = new Font("Cooper Black", 10, FontStyle.Regular);
                btn.Width = 30;
                btn.FlatStyle = FlatStyle.Flat;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.SteelBlue;

                btn.MouseClick += new MouseEventHandler(lbl_MouseClick);
                btn.ContextMenuStrip = contextMenuStrip1;
                panelSetupDevices.Controls.Add(btn);
                btn.Top = 80 + ((i - (i % 2)) * 30);
                btn.AutoSize = true;
                if (i % 2 == 0)
                {
                    btn.Left = 30;
                }
                else
                {
                    btn.Left = (panelSetupDevices.Width - btn.Width - 30);
                }

                btn = null;
            }
            
                dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;

        }
        public void btnActivitySetup_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelSetupActivities";
            timerSetupSlider.Enabled = true;
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("select name, activity_id from activities;", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Button btn = new Button();

                btn.Text = dt.Rows[i]["name"].ToString();
                btn.Visible = true;
                btn.Tag = dt.Rows[i]["activity_id"].ToString();
                btn.Font = new Font("Cooper Black", 10, FontStyle.Regular);
                btn.Width = 30;
                btn.FlatStyle = FlatStyle.Flat;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.SteelBlue;

                btn.MouseClick += new MouseEventHandler(editActivity);
                //btn.ContextMenuStrip = contextMenuStrip1;
                panelSetupActivities.Controls.Add(btn);
                btn.Top = 80 + ((i - (i % 2)) * 30);
                btn.AutoSize = true;
                if (i % 2 == 0)
                {
                    btn.Left = 30;
                }
                else
                {
                    btn.Left = (panelSetupActivities.Width - btn.Width - 30);
                }
                btn = null;
            }

            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;

        }

        void editActivity(object sender, MouseEventArgs e)
        {
            
        }
        void lbl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                curLabel = (Label)sender;
            }
        }



        void btnAddDevice_Click(object sender, System.EventArgs e)
        {
            frmAddDevice frm = new frmAddDevice();
            frm.database = database;
            frm.frm = this;
            frm.ShowDialog(this);
        }
        void btnAddActivity_Click(object sender, System.EventArgs e)
        {
            frmAddActivity frm = new frmAddActivity();
            frm.database = database;
            frm.frm = this;
            frm.ShowDialog(this);
        }

        void frmMain_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.X < 80 && e.Y < 60)
            {
                lockUpper = true;
                timerLock.Enabled = true;
            }
            else if (e.X < 80 && e.Y > 420)
            {
                lockLower = true;
                timerLock.Enabled = true;
            }
            if (lockLower && lockUpper && btnSetup.Visible == false)
            {
                btnSetup.Visible = true;
            }
            else
            {
                btnSetup.Visible = false;
            }
        }

        private void editCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCommands frm = new frmCommands();
            frm.database = database;
            frm.device_id = int.Parse(((Button)contextMenuStrip1.SourceControl).Tag.ToString());
            frm.ShowDialog();
        }


    }
}
