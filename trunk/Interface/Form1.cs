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

namespace Lighting_Interface
{
    public partial class frmMain : Form
    {
        string activePanel;
        string database;
        bool lockUpper;
        bool lockLower;
        Label curLabel;
        public frmMain()
        {
            InitializeComponent();
        }

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
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "lighting.ini");
            string temp = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            sr = null;
            database = temp.Substring(temp.IndexOf("database") + 9, temp.IndexOf("\r", temp.IndexOf("database")) - temp.IndexOf("database") - 9).Trim();
            temp = null;
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
        void btnActivities_Click(object sender, System.EventArgs e)
        {
            activePanel = "panelActivities";
            timerContentSlider.Enabled = true;
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
                Label lbl = new Label();
                lbl.Text = dt.Rows[i]["manufacturer"].ToString() + " " + dt.Rows[i]["type"].ToString() + " - " + dt.Rows[i]["model"].ToString();
                lbl.Visible = true;
                lbl.Tag = dt.Rows[i]["device_id"].ToString();
                lbl.MouseClick += new MouseEventHandler(lbl_MouseClick);
                lbl.ContextMenuStrip = contextMenuStrip1;
                panelSetupDevices.Controls.Add(lbl);
                lbl.Top = 80 + ((i - (i % 2)) * 30);
                lbl.AutoSize = true;
                lbl.Left = ((i % 2)) * (this.Width / 2) + 30;
              
                lbl = null;
            }
            
            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;

        }

        void lbl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                curLabel = (Label)sender;
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

        void btnAddDevice_Click(object sender, System.EventArgs e)
        {
            frmAddDevice frm = new frmAddDevice();
            frm.database = database;
            frm.frm = this;
            frm.ShowDialog(this);
        }

        private void timerLock_Tick(object sender, EventArgs e)
        {
            lockUpper = false;
            lockLower = false;
            timerLock.Enabled = false;
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
            frm.device_id = int.Parse(((Label)contextMenuStrip1.SourceControl).Tag.ToString());
            frm.ShowDialog();
        }
    }
}
