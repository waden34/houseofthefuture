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
    public partial class frmCommands : Form
    {
        public string database;
        public int device_id;
        public frmCommands()
        {
            InitializeComponent();
        }

        private void frmCommands_Load(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("select distinct command_id, manufacturer, type, model, display_name, long_name, frequency, repeat, command "
                + "from devices "
                + "join device_type on device_type.type_id = devices.type_id "
                + "join manufacturer on manufacturer.manufacturer_id = devices.manufacturer_id "
                + "left outer join ir_commands on ir_commands.device_id = devices.device_id "
                + "where devices.device_id = " + device_id + ";", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            lblDevice.Text = dt.Rows[0]["manufacturer"].ToString() + " " + dt.Rows[0]["type"].ToString() + " - " + dt.Rows[0]["model"].ToString();
            lblDevice.Left = (this.Width - lblDevice.Width) / 2;
            int iTop = 80;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["command_id"].ToString() != "")
                {
                    Label lbl = new Label();
                    lbl.Text = dt.Rows[i]["long_name"].ToString();
                    lbl.Top = iTop;
                    
                    
                    lbl.Visible = true;
                    lbl.DoubleClick += new EventHandler(lbl_Click);
                    lbl.Name = "lblCommand" + dt.Rows[i]["command_id"].ToString();
                    lbl.ContextMenuStrip = contextMenuStrip1;
                    this.Controls.Add(lbl);
                    lbl.AutoSize = true;
                    if (i % 2 == 0)
                    {
                        lbl.Left = 20;
                    }
                    else
                    {
                        lbl.Left = (this.Width - lbl.Width) - 40;
                    }
                    if (i % 2 > 0)
                    {
                        iTop += 30;
                    }
                    if (lbl.Top > (this.Height - 20) && !this.Controls.ContainsKey("scrollup"))
                    {
                        Buttons button = new Buttons();
                        button.Type = Buttons.ButtonType.TriangleUp;
                        button.Width = 40;
                        button.Height = 20;
                        button.Left = ((this.Width - button.Width) / 2);
                        button.Top = 30;
                        button.Name = "scrollup";
                        button.Click += new EventHandler(ScrollPanel);
                        this.Controls.Add(button);

                        button = new Buttons();
                        button.Type = Buttons.ButtonType.TriangleDown;
                        button.Width = 40;
                        button.Height = 20;
                        button.Left = ((this.Width - button.Width) / 2);
                        button.Top = this.Height - 30;
                        button.Name = "scrolldown";
                        button.Click += new EventHandler(ScrollPanel);
                        this.Controls.Add(button);
                        button = null;
                    }
                    lbl = null;
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
            frmEditCommand frm = new frmEditCommand(this);
            frm.device_id = device_id;
            frm.database = database;
            frm.command = ((Label)sender).Text;
            frm.ShowDialog();
            frm = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAddCommand_Click(object sender, EventArgs e)
        {
            frmEditCommand frm = new frmEditCommand(this);
            frm.device_id = device_id;
            frm.command = "";
            frm.database = database;
            frm.ShowDialog();
            frm = null;
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("delete from ir_commands where device_id = " + device_id + " and long_name = \'" + ((Label)contextMenuStrip1.SourceControl).Text + "\';", conn);
            da.Fill(new DataTable());
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;

            frmCommands frm = new frmCommands();
            frm.device_id = device_id;
            frm.database = database;
            frm.ShowDialog();

            this.Dispose();

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
                    if (ctl.GetType() == typeof(Label))
                    {
                        ((Label)ctl).Top -= 60;
                    }
                }
            }
            else if (((Buttons)sender).Name == "scrolldown")
            {
                foreach (Control ctl in (((Buttons)sender).Parent.Controls))
                {
                    if (ctl.GetType() == typeof(Label))
                    {
                        ((Label)ctl).Top += 60;
                    }
                }
            }
        }
    }
}
