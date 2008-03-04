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
    public partial class frmEditCommand : Form
    {
        public int device_id;
        public string command;
        public string database;
        frmCommands form;
        public frmEditCommand(frmCommands frm)
        {
            InitializeComponent();
            form = frm;
        }

        private void frmEditCommand_Load(object sender, EventArgs e)
        {
            string where;
            if (command != "")
            {
                where = " and long_name = \'" + command + "\'";
            }
            else
            {
                where = "";
            }
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("select distinct command_id, manufacturer, type, model, display_name, long_name, frequency, repeat, command, delay "
                    + "from devices "
                    + "join device_type on device_type.type_id = devices.type_id "
                    + "join manufacturer on manufacturer.manufacturer_id = devices.manufacturer_id "
                    + "left outer join ir_commands on ir_commands.device_id = devices.device_id "
                    + "where devices.device_id = " + device_id + where + ";", conn);
                DataTable dt = new DataTable();

                da.Fill(dt);
                lblDevice.Text = dt.Rows[0]["manufacturer"].ToString() + " " + dt.Rows[0]["type"].ToString() + " - " + dt.Rows[0]["model"].ToString();
                lblDevice.Left = (this.Width - lblDevice.Width) / 2;
                if (command != "")
                {
                    txtLongName.Text = command;
                    txtDisplayName.Text = dt.Rows[0]["display_name"].ToString();
                    txtRepeat.Text = dt.Rows[0]["repeat"].ToString();
                    txtFrequency.Text = dt.Rows[0]["frequency"].ToString();
                    txtCommand.Text = dt.Rows[0]["command"].ToString();
                    txtDelay.Text = dt.Rows[0]["delay"].ToString();
                    
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
            
            if (txtCommand.Text == "" || txtDisplayName.Text == "" || txtFrequency.Text == "" || txtLongName.Text == "" || txtRepeat.Text == "")
            {
                MessageBox.Show("Please enter all information before submitting");
                return;
            }
            SQLiteConnection conn;
            SQLiteDataAdapter da;
            if (command != "")
            {
                conn = new SQLiteConnection("Data Source=" + database);
                da = new SQLiteDataAdapter("update ir_commands set command = \'" + txtCommand.Text + "\', "
                    + "long_name = \'" + txtLongName.Text + "\', display_name = \'" + txtDisplayName.Text + "\', frequency = " + txtFrequency.Text
                    + ", repeat = " + txtRepeat.Text + ", delay = " + txtDelay.Text + " where device_id = " + device_id + " and long_name = \'" + command + "\';", conn);
                da.Fill(new DataTable());
                
            }
            else
            {
                conn = new SQLiteConnection("Data Source=" + database);
                da = new SQLiteDataAdapter("insert into ir_commands values (null," + device_id + ", \'" + txtLongName.Text + "\',\'"
                  + txtDisplayName.Text + "\',\'" + txtCommand.Text + "\'," + txtFrequency.Text + "," + txtRepeat.Text + "," + txtDelay.Text + ");", conn);
                da.Fill(new DataTable());

            }
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
            form.Dispose();
            frmCommands frm = new frmCommands();
            frm.database = database;
            frm.device_id = device_id;
            frm.ShowDialog();
            this.Dispose();
        }


    }
}
