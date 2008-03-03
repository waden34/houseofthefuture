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
    public partial class frmDiscInsert : Form
    {
        public frmDiscInsert()
        {
            InitializeComponent();
        }
        public string database;
        public string unit_id;
        public string pos;
        public frmMain frmMain;
        List<string> disc_ids;
        private void frmDiscInsert_Load(object sender, EventArgs e)
        {
            disc_ids = new List<string>();
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select disc_id, title from discs where ejected = 1;", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cbDiscs.Items.Add(dt.Rows[i]["title"].ToString());
                disc_ids.Add(dt.Rows[i]["disc_id"].ToString());
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
            if (rbAdd.Checked)
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("delete from pending_inserts where unit_id = " + unit_id + " and slot = " + pos + ";", conn);
                da.Fill(new DataTable());
                da.Dispose();
                da = null;
                conn.Dispose();
                conn = null;
                frmAddDisc frm = new frmAddDisc();
                frm.database = database;
                frm.unit_id = unit_id;
                frm.pos = pos;
                frm.ShowDialog();
            }
            else if (rbEjected.Checked)
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
                SQLiteDataAdapter da = new SQLiteDataAdapter("update discs set ejected = 0, unit_id = " + unit_id + ",slot = " + pos + " where disc_id = " + disc_ids[cbDiscs.SelectedIndex] + ";", conn);
                da.Fill(new DataTable());
                da = new SQLiteDataAdapter("delete from pending_inserts where unit_id = " + unit_id + " and slot = " + pos + ";", conn);
                da.Fill(new DataTable());
                da.Dispose();
                da = null;
                conn.Dispose();
                conn = null;
            }
            frmMain.isInserting = false;
            this.Dispose();
        }
    }
}
