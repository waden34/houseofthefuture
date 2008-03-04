using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace HouseOfTheFuture
{
    public partial class frmAddActivity : Form
    {
        public string database;
        public frmMain frm;
        int activeDevice;
        List<string> device_ids;
        List<string> devices;
        string activity_id;
        RectTracker CSharpTracker;
        public frmAddActivity()
        {
            InitializeComponent();
        }

        private void frmAddActivity_Load(object sender, EventArgs e)
        {
            devices = new List<string>();
            device_ids = new List<string>();
            SQLiteConnection conn = new SQLiteConnection("Data Source = " + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select manufacturer, type, model, device_id from devices "
                + "join manufacturer on manufacturer.manufacturer_id = devices.manufacturer_id "
                + "join device_type on device_type.type_id = devices.type_id;", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CheckBox chk = new CheckBox();

                chk.Text = dt.Rows[i]["manufacturer"].ToString() + " " + dt.Rows[i]["type"].ToString() + " - " + dt.Rows[i]["model"].ToString();
                chk.Visible = true;
                chk.Tag = dt.Rows[i]["device_id"].ToString();
                chk.Font = new Font("Cooper Black", (float)8.75, FontStyle.Regular);
                chk.Width = 200;
                chk.FlatStyle = FlatStyle.Flat;
                chk.ForeColor = Color.White;
                chk.FlatAppearance.BorderColor = Color.SteelBlue;
                this.panel1.Controls.Add(chk);
                chk.Top = 80 + ((i - (i % 2)) * 30);
                chk.AutoSize = true;
                chk.AutoEllipsis = false;
                chk.Left = ((i % 2)) * (this.Width / 2) + 30;

                chk = null;
            }
            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (btnOk.Text == "Next")
            {
                if (txtActivityName.Text == "")
                {
                    MessageBox.Show("Please enter a name for this activity");
                    return;
                }
                foreach (Control ctl in this.panel1.Controls)
                {
                    if (ctl.GetType() == typeof(CheckBox) && ((CheckBox)ctl).Checked)
                    {
                        devices.Add(((CheckBox)ctl).Text);
                        device_ids.Add(((CheckBox)ctl).Tag.ToString());
                    }
                }
                if (devices.Count == 0)
                {
                    MessageBox.Show("Please select at least one device to include");
                    return;
                }
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("insert into activities values (null,\'" + txtActivityName.Text + "\');", conn);
                da.Fill(new DataTable());
                da = new SQLiteDataAdapter("select max(activity_id) from activities;", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                activity_id = dt.Rows[0][0].ToString();
                dt.Dispose();
                dt = null;
                da.Dispose();
                da = null;
                conn.Dispose();
                conn = null;
                activeDevice = 0;
                btnOk.Text = "Next Device";
                GetCommands();
                
            }
            else if (btnOk.Text == "Next Device")
            {
                foreach (Control ctl in this.Controls)
                {
                    if (ctl.GetType() == typeof(CheckBox) && ((CheckBox)ctl).Checked)
                    {
                        SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                        SQLiteDataAdapter da = new SQLiteDataAdapter("insert into activity_startup values (" + activity_id + "," + device_ids[activeDevice] + "," + ((CheckBox)ctl).Tag.ToString() + ");", conn);
                        da.Fill(new DataTable());
                        da.Dispose();
                        da = null;
                        conn.Dispose();
                        conn = null;
                    }
                }

                activeDevice++;
                GetCommands();

            }
            else if (btnOk.Text == "Setup Buttons")
            {
                for (int j = 0; j < this.Controls.Count; j++)
                {
                    if (this.Controls[j].GetType() != typeof(Button))
                    {
                        this.Controls.RemoveAt(j);
                        j--;
                    }
                }
                btnOk.Text = "Ok";
                MessageBox.Show("Right click the window to select a button type to add to the layout.");
                this.Width = 800;
                this.Height = 480;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.ContextMenuStrip = contextMenuStrip1;
            }
            else
            {
                foreach (Control ctl in this.Controls)
                {
                    if (ctl.GetType() == typeof(Buttons))
                    {
                        SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                        SQLiteDataAdapter da = new SQLiteDataAdapter("insert into activity_buttons values (" + activity_id + "," + ((Buttons)ctl).Tag.ToString().Substring(0,((Buttons)ctl).Tag.ToString().IndexOf(",")).Trim() + "," + ((Buttons)ctl).Tag.ToString().Substring(((Buttons)ctl).Tag.ToString().IndexOf(",") + 1).Trim() + "," + ((Buttons)ctl).Left.ToString() + "," + ((Buttons)ctl).Top.ToString() + "," + ((Buttons)ctl).Height.ToString() + "," + ((Buttons)ctl).Width.ToString() + "," + (int)((Buttons)ctl).Type + ");", conn);
                        da.Fill(new DataTable());
                        da.Dispose();
                        da = null;
                        conn.Dispose();
                        conn = null;

                    }
                }
                this.Dispose();
            }
        }

        private void GetCommands()
        {
            if (activeDevice == devices.Count)
            {
                btnOk.Text = "Setup Buttons";
            }
            else if (btnOk.Text == "Next Device")
            {
                for (int j = 0; j < this.panel1.Controls.Count; j++)
                {
                    if (this.panel1.Controls[j].GetType() != typeof(Button))
                    {
                        this.panel1.Controls.RemoveAt(j);
                        j--;
                    }
                }
                for (int j = 0; j < this.Controls.Count; j++)
                {
                    if (this.Controls[j].GetType() != typeof(Button) && this.Controls[j].GetType() != typeof(Panel))
                    {
                        this.Controls.RemoveAt(j);
                        j--;
                    }
                }
                Label lbl = new Label();
                lbl.Font = new Font("Cooper Black", (float)8.75, FontStyle.Regular);
                lbl.ForeColor = Color.White;
                lbl.Text = "What commands should be sent to " + devices[activeDevice] + " at startup?";
                this.Controls.Add(lbl);
                lbl.AutoSize = true;
                lbl.AutoEllipsis = false;
                lbl.Left = (this.Width - lbl.Width) / 2;
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("Select display_name, command_id from ir_commands where device_id = " + device_ids[activeDevice] + ";", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CheckBox chk = new CheckBox();
                    
                    chk.Text = dt.Rows[i]["display_name"].ToString();
                    chk.Visible = true;
                    chk.Tag = dt.Rows[i]["command_id"].ToString();
                    chk.Font = new Font("Cooper Black", (float)8.75, FontStyle.Regular);
                    chk.Width = 200;
                    chk.FlatStyle = FlatStyle.Flat;
                    chk.ForeColor = Color.White;
                    chk.FlatAppearance.BorderColor = Color.SteelBlue;
                    this.panel1.Controls.Add(chk);
                    chk.AutoSize = true;
                    chk.AutoEllipsis = false;
                    chk.Top = 80 + ((i - (i % 2)) * 30);
                    chk.Left = ((i % 2)) * (this.Width / 2) + 30;

                    chk = null;
                }
                dt.Dispose();
                dt = null;
                da.Dispose();
                da = null;
                conn.Dispose();
                conn = null;
                if ((activeDevice + 1) == devices.Count)
                {
                    btnOk.Text = "Setup Buttons";
                }
            }
        }

        private void longButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddButton(Buttons.ButtonType.Long);

        }

        private void AddButton(Buttons.ButtonType type)
        {
            Buttons button = new Buttons();
            button.Type = type;

            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select manufacturer, type, model, devices.device_id from devices join manufacturer on manufacturer.manufacturer_id = devices.manufacturer_id join device_type on device_type.type_id = devices.type_id", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> devices = new List<string>();
            List<string> items = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                items.Add(dt.Rows[i]["manufacturer"].ToString() + " " + dt.Rows[i]["type"].ToString() + " - " + dt.Rows[i]["model"].ToString());
                devices.Add(dt.Rows[i]["device_id"].ToString());
            }
            InputResult result = inputBox.getInput("What device is this button for?", items, devices);
            button.Tag = result.device_id;

            devices = new List<string>();

            items = new List<string>();
            da = new SQLiteDataAdapter("Select long_name, command_id, display_name from ir_commands where device_id = " + result.device_id + ";", conn);
            dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                items.Add(dt.Rows[i]["long_name"].ToString());
                devices.Add(dt.Rows[i]["command_id"].ToString());
            }
            result = inputBox.getInput("What command?", items, devices);
            button.Tag += "," + result.device_id;
            button.Caption = result.Text;
            button.MouseDown += new MouseEventHandler(Buttons_MouseDown);
            this.Controls.Add(button);
            items = null;
            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
        }

        protected virtual void Buttons_MouseDown(object sender, MouseEventArgs e)
        {
            ((Buttons)sender).BringToFront();
            ((Buttons)sender).Capture = false;
            if (this.Controls.Contains(CSharpTracker))
                this.Controls.Remove(CSharpTracker);
            CSharpTracker = new RectTracker(((Buttons)sender));

            this.Controls.Add(CSharpTracker);
            CSharpTracker.BringToFront();
            CSharpTracker.Draw();

        }

        private void roundButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddButton(Buttons.ButtonType.Round);
        }

        private void tallButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddButton(Buttons.ButtonType.Tall);
        }

        private void discBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Buttons button = new Buttons();
            button.Type = Buttons.ButtonType.Long;
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select disc_type, type from disc_types;", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> types = new List<string>();
            List<string> ids = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                types.Add(dt.Rows[i]["type"].ToString());
                ids.Add(dt.Rows[i]["disc_type"].ToString());
            }
            InputResult result = inputBox.getInput("What type of disc will you browse for?", types, ids);
            button.Tag = "-99," + result.device_id;

            result = inputBox.getInput("What caption do you want on the button?");
            button.Caption = result.Text;
            button.MouseDown += new MouseEventHandler(Buttons_MouseDown);
            da = new SQLiteDataAdapter("insert into ir_commands values (null," + button.Tag.ToString().Substring(button.Tag.ToString().IndexOf(",") + 1) + ",\'" + result.Text + "\',\'" + result.Text + "\',-99,0,0,0,0);", conn);
            da.Fill(new DataTable());
            da = new SQLiteDataAdapter("select max(command_id) as id from ir_commands;", conn);
            dt = new DataTable();
            da.Fill(dt);
            button.Tag = "-99," + dt.Rows[0]["id"].ToString();
            this.Controls.Add(button);
        }
    }
}
