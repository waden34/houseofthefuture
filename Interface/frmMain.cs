﻿using System;
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
using Microsoft.Win32;

namespace HouseOfTheFuture
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            
            InitializeComponent();
        }
        #region Variables

        /// <summary>
        /// Which Panel is currently being viewed
        /// </summary>
        string activePanel;

        /// <summary>
        /// Path to the database file
        /// </summary>
        string database;

        /// <summary>
        /// Ip address for the Global Cache unit
        /// </summary>
        string irCommander;

        /// <summary>
        /// Socket connection to the Global Cache unit
        /// </summary>
        Socket gcSocket;

        /// <summary>
        /// True if the upper left corner of the screen has been pressed
        /// </summary>
        bool setupUpper;

        /// <summary>
        /// True if the lower left corner of the screen has been pressed
        /// </summary>
        bool setupLower;

        /// <summary>
        /// Is the button still being held down?
        /// </summary>
        bool holding;

        /// <summary>
        /// Which device is the current action for
        /// </summary>
        string activeDevice;

        /// <summary>
        /// Display Name for the command being sent
        /// </summary>
        string activeDisplayName;

        /// <summary>
        /// # of times the command has been sent this sequence
        /// </summary>
        int iCount;

        /// <summary>
        /// Message ID sent to the Global Cache unit
        /// </summary>
        int id;

        /// <summary>
        /// Lock to prevent multiple threads from sending IR commands
        /// </summary>
        object lockIR;

        /// <summary>
        /// Prevents multiple threads to insert into the database
        /// </summary>
        public bool isInserting;

        /// <summary>
        /// How much information will be logged, 2 = Error Only, 1 = Errors & Warnings, 0 = All debug info
        /// </summary>
        int logLevel;

        /// <summary>
        /// Void passed as an argument when loading Activities
        /// </summary>
        delegate void buttonClick(object sender, MouseEventArgs e);

        /// <summary>
        /// Color to use for all text
        /// </summary>
        public Color foreColor;

        /// <summary>
        /// Color to use for all backgrounds
        /// </summary>
        public Color backColor;
        #endregion

        #region Timers
        /// <summary>
        /// Reads the database and sets the Light Values on the gui to db levels.  
        /// Checks the pending_inserts table and prompts the user when a disc is inserted into a Disk Stakka.
        /// Updates the current time
        /// </summary>
        private void timerPing_Tick(object sender, EventArgs e)
        {
            //Read the Light Values from the database
            System.Data.DataTable dt = new DataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < this.Controls.Count; j++)
                {
                    if (this.Controls[j].GetType() == typeof(TrackBar))
                    {
                        //Set the trackbar value = the db level
                        if (((TrackBar)this.Controls[j]).Name == "tbNode" + dt.Rows[i]["node_id"].ToString())
                        {
                            ((TrackBar)this.Controls[j]).Value = int.Parse(dt.Rows[i]["value"].ToString());
                        }
                    }
                }
            }
            //Flash the : on the clock
            if (DateTime.Now.Second % 2 == 0)
            {
                lblTime.Text = DateTime.Now.ToShortTimeString();
            }
            else
            {
                lblTime.Text = DateTime.Now.ToShortTimeString().Replace(":", " ");
            }
            //Readjust the clock position
            lblTime.Left = 631 + (panel1.Width - lblTime.Width) / 2;
           if (!isInserting)
           {
               //Check the pending_inserts table to see if any discs have been inserted
                isInserting = true;
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("Select * from pending_inserts;", conn);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //Show the Disc Inserted form
                    frmDiscInsert frm = new frmDiscInsert();
                    frm.database = database;
                    frm.unit_id = dt.Rows[0]["unit_id"].ToString();
                    frm.pos = dt.Rows[0]["slot"].ToString();
                    frm.frmMain = this;
                    frm.ForeColor = foreColor;
                    frm.BackColor = backColor;
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

        /// <summary>
        /// Slides the main button panel onto the screen
        /// </summary>
        private void timerSlider_Tick(object sender, EventArgs e)
        {
            if (panel1.Left == 631)
            {
                timerSlider.Enabled = false;
                return;
            }
            panel1.Left -= 30;
        }

        /// <summary>
        /// Slides the main button panel off screen
        /// </summary>
        private void timerSlideBack_Tick(object sender, EventArgs e)
        {
            if (panel1.Left == 781)
            {
                timerSlideBack.Enabled = false;
                return;
            }
            panel1.Left += 30;
        }

        /// <summary>
        /// Slides the Active Panel onscreen and non-Active Panels off screen
        /// </summary>
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

        /// <summary>
        /// Slides the Active Panel within the Setup Panel onscreen and all others offscreen
        /// </summary>
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

        /// <summary>
        /// Resets the setup lock
        /// </summary>
        private void timerLock_Tick(object sender, EventArgs e)
        {
            setupUpper = false;
            setupLower = false;
            timerLock.Enabled = false;
        }
        #endregion

        /// <summary>
        /// Setup variables and get things rolling
        /// </summary>
        private void frmMain_Load(object sender, EventArgs e)
        {
            //Reposition buttons
            this.btnActivitySetup.Left = ((this.panelSetup.Width - this.btnActivitySetup.Width) / 3);
            this.btnGeneralSetup.Left = (((this.panelSetup.Width - this.btnGeneralSetup.Width) / 3) * 2);
            this.btnLightingSetup.Left = (this.panelSetup.Width - this.btnLightingSetup.Width - 11);
            this.btnDeviceSetup.Left = 11;
            this.btnAddDevice.Left = (this.panelSetupDevices.Width - this.btnAddDevice.Width) / 2;
            this.btnAddActivity.Left = (this.panelSetupActivities.Width - this.btnAddActivity.Width) / 2;

            //Initialize variables
            lockIR = new object();
            isInserting = false;
            id = 1;

            
           
            //Setup and connect to the Global Cache unit
            gcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //gcSocket.Connect(irCommander, 4998);
            }
            catch
            {
                Console.WriteLine("Unable to connect to the Global Cache unit.\r\nIR Commands will be unavailable");
            }
            ToolTip t = new ToolTip();
            t.SetToolTip(lblTime, DateTime.Now.ToShortDateString());

            //Create the event handler for mouse movement.  Used to trigger animation sliders
            this.MouseMove += new MouseEventHandler(frmMain_MouseMove);
        }

        /// <summary>
        /// Start the Main Button panel animation if the mouse is far enough to the right.
        /// Start the Main Button panel reverse animation if the mouse isn't far enough to the right
        /// </summary>
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

        /// <summary>
        /// Start the Setup Panel animation
        /// </summary>
        void btnSetup_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelSetup";
            timerContentSlider.Enabled = true;
        }

        /// <summary>
        /// Start the Lighting Panel animation
        /// </summary>
        void btnLighting_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelLighting";
            timerContentSlider.Enabled = true;
        }

        /// <summary>
        /// Start the Device Panel animation and load Device list from the database
        /// </summary>
        void btnDevices_Click(object sender, System.EventArgs e)
        {
            //Start the animation
            activePanel = "panelDevices";
            timerContentSlider.Enabled = true;

            LoadDevices(panelDevices, contextMenuStrip1, new buttonClick(device_Click));
        }

        private void LoadDevices(Panel curPanel, ContextMenuStrip menu, buttonClick onClick)
        {
            //Clean up the panel by removing all old controls
            for (int i = 0; i < curPanel.Controls.Count; i++)
            {
                curPanel.Controls.RemoveAt(i);
                i--;
            }

            //Read all the devices from the database
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select manufacturer, type, model, emitter_id, device_id from devices join device_type on device_type.type_id = devices.type_id join manufacturer on manufacturer.manufacturer_id = devices.manufacturer_id;", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iTop = 30;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Create a button for each device
                Button btn = new Button();
                btn.Text = dt.Rows[i]["manufacturer"].ToString() + " " + dt.Rows[i]["type"].ToString() + " - " + dt.Rows[i]["model"].ToString();
                btn.Top = iTop;
                btn.Tag = dt.Rows[i]["device_id"].ToString();
                btn.Font = new Font("Cooper Black", 10, FontStyle.Regular);
                btn.Width = 30;
                btn.FlatStyle = FlatStyle.Flat;
                btn.ForeColor = foreColor;
                btn.FlatAppearance.BorderColor = backColor;
                btn.Visible = true;
                if (onClick != null)
                {
                    btn.MouseClick += new MouseEventHandler(onClick);
                }
                btn.Name = "btnDevice" + dt.Rows[i]["device_id"].ToString();
                //Add the context menu to each button
                btn.ContextMenuStrip = menu;

                curPanel.Controls.Add(btn);
                btn.AutoSize = true;
                btn.AutoEllipsis = false;
                if (i % 2 == 0)
                {
                    btn.Left = 10;
                }
                else
                {
                    btn.Left = curPanel.Width - btn.Width - 10;
                }
                if (i % 2 > 0)
                {
                    iTop += ((curPanel.Height - 90) / ((dt.Rows.Count - 1) / 2));
                }
                //btn.Top = 80 + ((i - (i % 2)) * 30);
                //btn.AutoSize = true;
                //if (i % 2 == 0)
                //{
                //    btn.Left = 30;
                //}
                //else
                //{
                //    btn.Left = (panelSetupDevices.Width - btn.Width - 30);
                //}
                //Add scroll buttons if there are too many activities
                if (btn.Top > (curPanel.Height - 20) && !curPanel.Controls.ContainsKey("scrollup"))
                {
                    Buttons button = new Buttons();
                    button.Type = Buttons.ButtonType.TriangleUp;
                    button.Width = 40;
                    button.Height = 20;
                    button.Left = ((curPanel.Width - button.Width) / 2);
                    button.Top = 30;
                    button.Name = "scrollup";
                    button.Click += new EventHandler(ScrollPanel);
                    curPanel.Controls.Add(button);

                    button = new Buttons();
                    button.Type = Buttons.ButtonType.TriangleDown;
                    button.Width = 40;
                    button.Height = 20;
                    button.Left = ((curPanel.Width - button.Width) / 2);
                    button.Top = curPanel.Height - 30;
                    button.Name = "scrolldown";
                    button.Click += new EventHandler(ScrollPanel);
                    curPanel.Controls.Add(button);
                    button = null;
                }
            }
            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
        }

        /// <summary>
        /// Bring up the actions associated with the device
        /// </summary>
        void device_Click(object sender, EventArgs e)
        {
            
            string device = ((Button)sender).Name.Substring(9).Trim();
            string model = ((Button)sender).Text;

            //Clean up the Device panel by removing all old controls
            for (int i = 0; i < panelDevices.Controls.Count; i++)
            {
                panelDevices.Controls.RemoveAt(i);
                i--;
            }

            //Read the IR commands for this device from the database
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select distinct display_name from ir_commands where device_id = " + device + ";", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            //Add a label naming the selected device
            Label lbl = new Label();
            lbl.Top = 20;
            lbl.Text = model;
            lbl.Font = new Font("Cooper Black", 10, FontStyle.Regular);
            lbl.ForeColor = foreColor;
            panelDevices.Controls.Add(lbl);
            lbl.AutoSize = true;
            lbl.Left = ((panelDevices.Width - lbl.Width) / 2);
            lbl = null;
            int iTop = 50;

            //Add a button for each Command
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Button btn = new Button();
                btn.Text = dt.Rows[i]["display_name"].ToString();
                btn.Top = iTop;
                btn.Font = new Font("Cooper Black", 10, FontStyle.Regular);
                btn.Width = 300;
                btn.FlatStyle = FlatStyle.Flat;
                btn.ForeColor = foreColor;
                btn.FlatAppearance.BorderColor = backColor;
                //Store the device in the Tag property to retrieve when the command is clicked
                btn.Tag = device;
                btn.Visible = true;

                //Use MouseDown + MouseUp instead of MouseClick in order to emulate holding a button down for a length of time.
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
                //Add scroll buttons if there are too many activities
                if (btn.Top > (panelDevices.Height - 20) && !panelDevices.Controls.ContainsKey("scrollup"))
                {
                    Buttons button = new Buttons();
                    button.Type = Buttons.ButtonType.TriangleUp;
                    button.Width = 40;
                    button.Height = 20;
                    button.Left = ((panelDevices.Width - button.Width) / 2);
                    button.Top = 30;
                    button.Name = "scrollup";
                    button.Click += new EventHandler(ScrollPanel);
                    panelDevices.Controls.Add(button);

                    button = new Buttons();
                    button.Type = Buttons.ButtonType.TriangleDown;
                    button.Width = 40;
                    button.Height = 20;
                    button.Left = ((panelDevices.Width - button.Width) / 2);
                    button.Top = panelDevices.Height - 30;
                    button.Name = "scrolldown";
                    button.Click += new EventHandler(ScrollPanel);
                    panelDevices.Controls.Add(button);
                    button = null;
                }
                btn = null;
            }

            model = null;
            device = null;
            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
        }

        /// <summary>
        /// When a button is released, set holding to false to stop sending the command
        /// </summary>
        void btn_MouseUp(object sender, MouseEventArgs e)
        {
            holding = false;
        }

        /// <summary>
        /// When a button is pressed, start sending the command
        /// </summary>
        void btn_MouseDown(object sender, MouseEventArgs e)
        {
            iCount = 0;
            activeDevice = ((Button)sender).Tag.ToString() ;
            activeDisplayName = ((Button)sender).Text;
            holding = true;

            //Start the thread to send the command
            System.Threading.ThreadStart job = new System.Threading.ThreadStart(CommandClick);
            System.Threading.Thread t = new System.Threading.Thread(job);
            t.Start();

        }

        /// <summary>
        /// Sends the command to the Global Cache unit
        /// </summary>
        private void CommandClick()
        {
            //Locks the routine so only one thread can access it
            lock (lockIR)
            {
                //Read the information for this command from teh database
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("Select command, repeat, frequency, emitter_id, hold, delay from ir_commands join devices on devices.device_id = ir_commands.device_id where devices.device_id = " + activeDevice + " and display_name = \'" + activeDisplayName + "\';", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                try
                {
                    //If we are holding the button down or we haven't sent the command at all yet
                    while (holding || iCount == 0)
                    {
                        iCount++;
                        //If the command isn't repeatable, don't allow it to repeat
                        if (dt.Rows[0]["hold"].ToString() == "False")
                        {
                            holding = false;
                        }

                        //Check if we've lost connection to the Global Cache unit
                        if (!gcSocket.Connected)
                        {
                            gcSocket.Connect(irCommander, 4998);
                        }

                        //Set up the byte array for the command sendir,[block]:[emitter],[msg id],[frequency],[times to repeat][command]\r
                        byte[] bytestoSend;
                        bytestoSend = System.Text.Encoding.Default.GetBytes("sendir,2:" + dt.Rows[0]["emitter_id"].ToString() + "," + id + "," + dt.Rows[0]["frequency"].ToString() + "," + dt.Rows[0]["repeat"].ToString() + "," + dt.Rows[0]["command"].ToString().Replace("\r", "") + "\r");
                        gcSocket.Send(bytestoSend);
                        bytestoSend = null;
                        id++;
                        if (gcSocket.Available > 0)
                        {
                            //Receive the confirmation from the Global Cache unit
                            byte[] receivedBytes = new byte[gcSocket.Available];
                            gcSocket.Receive(receivedBytes);
                            receivedBytes = null;
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

        /// <summary>
        /// Start the Activity Panel animation and list all Activities in the database
        /// </summary>
        void btnActivities_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelActivities";
            timerContentSlider.Enabled = true;
            //Clean up the panel by removing old controls
            for (int i = 0; i < panelActivitiesContent.Controls.Count; i++)
            {

                    panelActivitiesContent.Controls.RemoveAt(i);
                    i--;
            }
            for (int i = 0; i < panelActivities.Controls.Count; i++)
            {
                if (panelActivities.Controls[i].Name != "panelActivitiesContent")
                {
                    panelActivities.Controls.RemoveAt(i);
                    i--;
                }
            }
            //Read all activities from the database
            LoadActivities(panelActivitiesContent, contextMenuStrip1, new buttonClick(activity_click));

        }

        /// <summary>
        /// Starts the selected activity and loads its button layout
        /// </summary>
        void activity_click(object sender, EventArgs e)
        {
            string activity = ((Button)sender).Name.Substring(11);
            //Clean up the panel by removing all old controls
            for (int i = 0; i < panelActivitiesContent.Controls.Count; i++)
            {
                panelActivitiesContent.Controls.RemoveAt(i);
                i--;
            }

            //Read the startup commands for this activity from the database
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select distinct activity_startup.device_id, activity_startup.command_id, display_name from activity_startup join ir_commands on ir_commands.command_id = activity_startup.command_id and ir_commands.device_id = activity_startup.device_id where activity_id = " + activity + ";", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            //Send each command to the Global Cache unit
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

            //Read the button layout for this activity
            da = new SQLiteDataAdapter("select x,y,width,height,type, ifnull(display_name,\' \') as display_name, ir_commands.device_id, activity_buttons.command_id, command from activity_buttons left outer join ir_commands on ir_commands.command_id = activity_buttons.command_id where activity_id = " + activity + ";", conn);
            dt = new DataTable();
            da.Fill(dt);
            
            //Layout each button on the panel
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Buttons button = new Buttons();
                button.Caption = dt.Rows[i]["display_name"].ToString();
                button.Type = (Buttons.ButtonType)(int.Parse(dt.Rows[i]["type"].ToString()));
                button.Left = int.Parse(dt.Rows[i]["x"].ToString());
                button.Top = int.Parse(dt.Rows[i]["y"].ToString());
                //Set the device id in the tag so we can access it when the button is pressed
                button.Tag = dt.Rows[i]["device_id"].ToString();
                button.Width = int.Parse(dt.Rows[i]["width"].ToString());
                button.Height = int.Parse(dt.Rows[i]["height"].ToString());
                if (dt.Rows[i]["command"].ToString() == "-99")
                {
                    //Handles "special" buttons
                    button.Click += new EventHandler(button_Click);
                    button.Name = dt.Rows[i]["command"].ToString();
                }
                else
                {
                    //Handles the commands for the button
                    button.MouseDown += new MouseEventHandler(button_MouseDown);
                    button.MouseUp += new MouseEventHandler(button_MouseUp);
                }
                panelActivitiesContent.Controls.Add(button);
                button = null;
            }
            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
        }

        /// <summary>
        /// Start the activity for the "special" button
        /// </summary>
        void button_Click(object sender, EventArgs e)
        {
            //Clean up the panel by removing all old Buttons
            foreach (Control ctl in panelActivitiesContent.Controls)
            {
                //if (ctl.GetType() != typeof(Buttons))
                //{
                panelActivitiesContent.Controls.Remove(ctl);
                //}
            }
            //Disc Browser
            if (((Buttons)sender).Name == "-99")
            {
                //Read all discs from the database
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("Select distinct disc_id, movie_id, title, cover, num_discs, unit_id, slot, ejected from discs where disc_type = " + ((Buttons)sender).Tag + ";", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int x = 10;
                int y = 0;

                //Show the cover and title for each disc
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PictureBox pb = new PictureBox();
                    MemoryStream ms = new MemoryStream((byte[])dt.Rows[i]["cover"]);
                    pb.Image = Image.FromStream(ms);
                    panelActivitiesContent.Controls.Add(pb);
                    pb.Left = x;
                    pb.Top = y;
                    //Store the unit & slot info for the disc
                    pb.Tag = dt.Rows[i]["unit_id"].ToString() + "," + dt.Rows[i]["slot"].ToString();
                    pb.Width = 100;
                    pb.Height = 140;
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    //Store the ejected state in the Name property
                    pb.Name = dt.Rows[i]["ejected"].ToString();
                    pb.Click += new EventHandler(eject_disc);
                    Label lbl = new Label();
                    lbl.ForeColor = foreColor;
                    lbl.Font = new Font("Verdana", 10);
                    lbl.Text = dt.Rows[i]["title"].ToString();
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Left = x;
                    lbl.Top = y + 150;

                    //Wrap the label text if it's longer than 10 characters
                    if (lbl.Text.Length > 10)
                    {
                        string temp = lbl.Text.Substring(0, 10);
                        for (int j = 10; j < lbl.Text.Length; j++)
                        {
                            if (lbl.Text.Substring(j, 1) == " ")
                            {
                                temp += "\r\n";
                                if (Math.Abs(10 - j) + 20 > lbl.Text.Length)
                                {
                                    temp += lbl.Text.Substring(j + 1);
                                    break;
                                }
                                else
                                {
                                    temp += lbl.Text.Substring(j, 11);
                                    j += 10;
                                }
                            }
                            else
                            {
                                temp += lbl.Text.Substring(j, 1);
                            }
                        }
                        lbl.Text = temp;
                        temp = null;
                    }
                    lbl.AutoSize = true;
                    panelActivitiesContent.Controls.Add(lbl);

                    //Add scroll buttons if there are too many activities
                    if (pb.Top > (panelActivitiesContent.Height - (pb.Height + lbl.Height)) && !panelActivitiesContent.Controls.ContainsKey("scrollup"))
                    {
                        Buttons button = new Buttons();
                        button.Type = Buttons.ButtonType.TriangleUp;
                        button.Width = 40;
                        button.Height = 20;
                        button.Left = ((panelActivities.Width - button.Width) / 2);
                        button.Top = 5;
                        button.Name = "scrollup";
                        button.Click += new EventHandler(ScrollPanel);
                        panelActivities.Controls.Add(button);
                        button.BringToFront();
                        button = new Buttons();
                        button.Type = Buttons.ButtonType.TriangleDown;
                        button.Width = 40;
                        button.Height = 20;
                        button.Left = ((panelActivities.Width - button.Width) / 2);
                        button.Top = panelActivities.Height - 25;
                        button.Name = "scrolldown";
                        button.Click += new EventHandler(ScrollPanel);
                        panelActivities.Controls.Add(button);
                        button = null;
                    }
                    if (x + pb.Width + 10 > panelActivitiesContent.Width)
                    {
                        x = 10;
                        y += 180;
                    }
                    else
                    {
                        x = x + pb.Width + 10;
                    }
                    lbl = null;
                    pb = null;
                    ms.Close();
                    ms.Dispose();
                    ms = null;


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
        /// Update the database to tell the Disk Stakka listener that we want a disc
        /// </summary>
        void eject_disc(object sender, EventArgs e)
        {
            //If the disc has already been ejected, notify the user
            if (((PictureBox)sender).Name == "1")
            {
                MessageBox.Show("Disc has already been ejected.\r\nPlease find it.");
                return;
            }

            //Retrieve the unit & pos info from the Tag property
            string unit_id = ((PictureBox)sender).Tag.ToString();
            unit_id = unit_id.Substring(0,unit_id.IndexOf(","));
            string pos = ((PictureBox)sender).Tag.ToString();
            pos = pos.Substring(pos.IndexOf(",") + 1);

            //Insert this disc into the pending_ejects table
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("insert into pending_ejects values (" + unit_id + "," + pos + ");", conn);
            da.Fill(new DataTable());
            da = new SQLiteDataAdapter("update discs set ejected = 1 where unit_id = " + unit_id + " and slot = " + pos + ";", conn);
            da.Fill(new DataTable());
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
            button_Click(null, null);
        }

        /// <summary>
        /// When a button is released, set holding to false to stop sending the command
        /// </summary>
        void button_MouseUp(object sender, MouseEventArgs e)
        {
            holding = false;
        }

        /// <summary>
        /// When a button is pressed, start sending the command
        /// </summary>
        void button_MouseDown(object sender, MouseEventArgs e)
        {
            holding = true;
            iCount = 0;
            activeDevice = ((Buttons)sender).Tag.ToString();
            activeDisplayName = ((Buttons)sender).Caption;
            holding = true;
            System.Threading.ThreadStart job = new System.Threading.ThreadStart(CommandClick);
            System.Threading.Thread t = new System.Threading.Thread(job);
            t.Start();
        }

        /// <summary>
        /// Start the Device Setup animation and load device info from the database
        /// </summary>
        public void btnDeviceSetup_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelSetupDevices";
            timerSetupSlider.Enabled = true;

            //Load all devices from the database
            LoadDevices(panelSetupDevicesContent, contextMenuStrip1, null);

        }
        
        /// <summary>
        /// Start the Activity Setup animation and load activity info from the database
        /// </summary>
        public void btnActivitySetup_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelSetupActivities";
            timerSetupSlider.Enabled = true;

            //Read all activities from the database
            LoadActivities(panelSetupActivitiesContent, null, new buttonClick(editActivity));


        }

        /// <summary>
        /// Loads all Activities from the database and displays them on the Panel
        /// </summary>
        /// <param name="curPanel">Panel to display the loaded Activites onto</param>
        /// <param name="menu">ContextMenuStrip to associate with the buttons</param>
        /// <param name="onClick">Function to call when the button is pressed</param>
        private void LoadActivities(Panel curPanel, ContextMenuStrip menu, buttonClick onClick)
        {
            SQLiteConnection conn;
            SQLiteDataAdapter da;
            DataTable dt;
            //Clean up the panel by removing old controls
            for (int i = 0; i < curPanel.Controls.Count; i++)
            {
                if (curPanel.Controls[i].GetType() == typeof(Button) && ((Button)curPanel.Controls[i]).Text != "Add Activity")
                {
                    curPanel.Controls.RemoveAt(i);
                }
            }
            //Load the activities from the database
            conn = new SQLiteConnection("Data Source=" + database);
            da = new SQLiteDataAdapter("select name, activity_id from activities;", conn);
            dt = new DataTable();
            da.Fill(dt);
            
            //Create a button for each activity
            int iTop = 90;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Button btn = new Button();

                btn.Text = dt.Rows[i]["name"].ToString();
                btn.Visible = true;
                btn.Tag = dt.Rows[i]["activity_id"].ToString();
                btn.Font = new Font("Cooper Black", 10, FontStyle.Regular);
                btn.Width = 30;
                btn.Name = "btnActivity" + dt.Rows[i]["activity_id"].ToString();
                btn.FlatStyle = FlatStyle.Flat;
                btn.ForeColor = foreColor;
                btn.FlatAppearance.BorderColor = backColor;
                btn.MouseClick += new MouseEventHandler(onClick);
                btn.ContextMenuStrip = menu;
                curPanel.Controls.Add(btn);
                if ((float)((dt.Rows.Count - 1) / 2) > 0)
                {
                    iTop += (int)((curPanel.Height - 90) / (float)((dt.Rows.Count - 1) / 2));
                }
                
                btn.Top = 80 + ((i - (i % 2)) * 30);
                btn.AutoSize = true;
                if (i % 2 == 0)
                {
                    btn.Left = 30;
                }
                else
                {
                    btn.Left = (curPanel.Width - btn.Width - 30);
                }
                
                //Add scroll buttons if there are too many activities
                if (btn.Top > (curPanel.Height - 20) && !curPanel.Controls.ContainsKey("scrollup"))
                {
                    Buttons button = new Buttons();
                    button.Type = Buttons.ButtonType.TriangleUp;
                    button.Width = 40;
                    button.Height = 20;
                    button.Left = ((curPanel.Width - button.Width) / 2);
                    button.Top = 30;
                    button.Name = "scrollup";
                    button.Click += new EventHandler(ScrollPanel);
                    curPanel.Controls.Add(button);

                    button = new Buttons();
                    button.Type = Buttons.ButtonType.TriangleDown;
                    button.Width = 40;
                    button.Height = 20;
                    button.Left = ((curPanel.Width - button.Width) / 2);
                    button.Top = curPanel.Height - 30;
                    button.Name = "scrolldown";
                    button.Click += new EventHandler(ScrollPanel);
                    curPanel.Controls.Add(button);
                    button = null;
                }
                btn = null;
            }
        }

        /// <summary>
        /// Scroll the controls on a panel
        /// </summary>
        void ScrollPanel(object sender, EventArgs e)
        {
            if (((Buttons)sender).Name == "scrollup")
            {
                foreach (Control ctl in (((Buttons)sender).Parent.Controls))
                {
                    if (ctl.GetType() == typeof(Button))
                    {
                        ((Button)ctl).Top += 60;
                    }
                    else if (ctl.GetType() == typeof(Panel))
                    {
                        foreach (Control childCtl in (((Panel)ctl)).Controls)
                        {
                            if (childCtl.GetType() == typeof(PictureBox))
                            {
                                ((PictureBox)childCtl).Top += 60;
                            }
                            else if (childCtl.GetType() == typeof(Label))
                            {
                                ((Label)childCtl).Top += 60;
                            }
                        }
                    }
                }
            }
            else if (((Buttons)sender).Name == "scrolldown")
            {
                foreach (Control ctl in (((Buttons)sender).Parent.Controls))
                {
                    if (ctl.GetType() == typeof(Button))
                    {
                        ((Button)ctl).Top -= 60;
                    }
                    else if (ctl.GetType() == typeof(Panel))
                    {
                        foreach (Control childCtl in (((Panel)ctl)).Controls)
                        {
                            if (childCtl.GetType() == typeof(PictureBox))
                            {
                                ((PictureBox)childCtl).Top -= 60;
                            }
                            else if (childCtl.GetType() == typeof(Label))
                            {
                                ((Label)childCtl).Top -= 60;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Show the Edit Activity form for the selected Activity
        /// </summary>
        void editActivity(object sender, MouseEventArgs e)
        {
            
        }

        /// <summary>
        /// Show the Add new Device form
        /// </summary>
        void btnAddDevice_Click(object sender, System.EventArgs e)
        {
            frmAddDevice frm = new frmAddDevice();
            frm.database = database;
            frm.frm = this;
            frm.ForeColor = foreColor;
            frm.BackColor = backColor;
            frm.ShowDialog(this);
        }
       
        /// <summary>
        /// Show the Add new Activity form
        /// </summary>
        void btnAddActivity_Click(object sender, System.EventArgs e)
        {
            frmAddActivity frm = new frmAddActivity();
            frm.database = database;
            frm.frm = this;
            frm.ForeColor = foreColor;
            frm.BackColor = backColor;
            frm.ShowDialog(this);
        }

        /// <summary>
        /// Start the Setup unlock routine
        /// </summary>
        void frmMain_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //If the top left corner was pressed
            if (e.X < 80 && e.Y < 60)
            {
                setupUpper = true;
                timerLock.Enabled = true;
            }
            //If the lower left corner was pressed
            else if (e.X < 80 && e.Y > 420)
            {
                setupLower = true;
                timerLock.Enabled = true;
            }
            //If both the upper left and lower left corners were pressed in time, enable the Setup button
            if (setupLower && setupUpper && btnSetup.Visible == false)
            {
                btnSetup.Visible = true;
            }
            else
            {
                btnSetup.Visible = false;
            }
        }

        /// <summary>
        /// Show the Edit Command form for the selected command
        /// </summary>
        private void editCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCommands frm = new frmCommands();
            frm.database = database;
            frm.device_id = int.Parse(((Button)contextMenuStrip1.SourceControl).Tag.ToString());
            frm.ForeColor = foreColor;
            frm.BackColor = backColor;
            frm.ShowDialog();
        }

        /// <summary>
        /// Show the General Setup options
        /// </summary>
        void btnSetupGeneral_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelSetupGeneral";
            timerSetupSlider.Enabled = true;

        }

        /// <summary>
        /// Set the back color for all controls
        /// </summary>
        void btnColorBack_Click(object sender, System.EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                backColor = dlg.Color;
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\House of the Future", true);
                key.SetValue("backColor", backColor.ToArgb().ToString());
                key.Close();
                key = null;
                this.BackColor = backColor;
                this.Invalidate();
                ColorControls();
            }
            dlg.Dispose();
            dlg = null;
        }

        /// <summary>
        /// Change the colors on all controls
        /// </summary>
        private void ColorControls()
        {
            foreach (Control ctl in this.Controls)
            {
                if (ctl.GetType() == typeof(Panel))
                {
                    foreach (Control child in ((Panel)ctl).Controls)
                    {
                        if (child.GetType() == typeof(Panel))
                        {
                            foreach (Control childchild in ((Panel)child).Controls)
                            {
                                childchild.ForeColor = foreColor;
                                childchild.BackColor = backColor;
                                if (childchild.GetType() == typeof(Button))
                                {
                                    
                                    if (((Button)childchild).Name == "btnColorFore")
                                    {
                                        ((Button)childchild).BackColor = foreColor;
                                    }
                                    else if (((Button)childchild).Name == "btnColorBack")
                                    {
                                    }
                                    else
                                    {
                                        ((Button)childchild).FlatAppearance.BorderColor = backColor;
                                    }
                                }
                            }
                        }
                        else
                        {
                            child.ForeColor = foreColor;
                            child.BackColor = backColor;
                            if (child.GetType() == typeof(Button))
                            {
                                ((Button)child).FlatAppearance.BorderColor = backColor;
                            }
                        }
                    }
                }
                else
                {
                    ctl.ForeColor = foreColor;
                    ctl.BackColor = backColor;
                    if (ctl.GetType() == typeof(Button))
                    {
                        ((Button)ctl).FlatAppearance.BorderColor = backColor;
                    }
                }
            }
        }

        /// <summary>
        /// Set the text color for all controls
        /// </summary>
        void btnColorFore_Click(object sender, System.EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foreColor = dlg.Color;
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\House of the Future", true);
                key.SetValue("foreColor", foreColor.ToArgb().ToString());
                key.Close();
                key = null;
                this.ForeColor = foreColor;
                this.Invalidate();
                ColorControls();
            }
            dlg.Dispose();
            dlg = null;
        }

        void txtIrCommander_TextChanged(object sender, System.EventArgs e)
        {
            irCommander = txtIrCommander.Text;
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\House of the Future", true);
            key.SetValue("irCommander", irCommander);
            key.Close();
            key = null;
        }

        void txtDatabase_TextChanged(object sender, System.EventArgs e)
        {
            database = txtDatabase.Text;
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\House of the Future", true);
            key.SetValue("database", database);
            key.Close();
            key = null;
        }

        void numLogLevel_ValueChanged(object sender, System.EventArgs e)
        {
            logLevel = (int)numLogLevel.Value;
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\House of the Future", true);
            key.SetValue("logLevel", logLevel);
            key.Close();
            key = null;
        }
    }
}
