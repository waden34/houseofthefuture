using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Lighting_Interface
{
    public partial class frmAddDevice : Form
    {
        public string database;
        public frmMain frm;
        public frmAddDevice()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("insert into devices values (NULL,(select type_id from device_type where type = \'" + cbDeviceTypes.Text + "\'),(select manufacturer_id from manufacturer where manufacturer = \'" + cbManufacturer.Text + "\'),\'" + txtModel.Text + "\'," + (cbEmitter.SelectedIndex + 1) + ");", conn);
            da.Fill(new DataTable());
            frm.btnDeviceSetup_Click(null, null);
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
            this.Dispose();
        }

        private void frmAddDevice_Load(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("select * from device_type;", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cbDeviceTypes.Items.Add(dt.Rows[i]["type"].ToString());
            }
            da = new SQLiteDataAdapter("select * from manufacturer;", conn);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cbManufacturer.Items.Add(dt.Rows[i]["manufacturer"].ToString());
                
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
    }
}
