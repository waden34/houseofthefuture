using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Runtime.InteropServices;

namespace Movie_Library
{
    public partial class Form1 : Form
    {

            [DllImport("user32.dll")]
            public static extern int RegisterWindowMessage(String strMessage);

        public Form1()
        {
            InitializeComponent();
        }
        List<string> ids;
        string database;
        byte[] cover;
        public int QueryCancelAutoPlay = 0;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (QueryCancelAutoPlay == 0)
            {
                QueryCancelAutoPlay = RegisterWindowMessage("QueryCancelAutoPlay");
            }
            if (m.Msg == QueryCancelAutoPlay)
            {
                m.Result = (IntPtr)1;
                return;
            }
            base.WndProc(ref m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtTitle.KeyPress += new KeyPressEventHandler(txtTitle_KeyPress);
            ids = new List<string>();
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            for (int i = 0; i < drives.Length; i++)
            {
                if (drives[i].IsReady && drives[i].DriveType == System.IO.DriveType.CDRom)
                {
                    comboBox1.Items.Add(drives[i].Name + drives[i].VolumeLabel);
                }
            }
            //comboBox1.SelectedIndex = 0;
            //timer1.Enabled = true;
            StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "lighting.ini");
            string temp = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            sr = null;
            database = temp.Substring(temp.IndexOf("database") + 9, temp.IndexOf("\r", temp.IndexOf("database")) - temp.IndexOf("database") - 9).Trim();
           // button1_Click(null, null);

            
        }

        void txtTitle_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1_Click(null, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            System.IO.DriveInfo di = new System.IO.DriveInfo(comboBox1.Items[comboBox1.SelectedIndex].ToString().Substring(0,1));
            string title;
            string temp;
            if (cbTitles.Text != "")
            {
                temp = client.DownloadString("http://www.imdb.com/title/tt" + ids[cbTitles.SelectedIndex] + "/");
                title = temp.Substring(temp.IndexOf("<title>") + 7, temp.IndexOf("</title>") - temp.IndexOf("<title>") - 7).Replace("&#233;", "é").Replace("&#38;", "&");
                cbTitles.Items.Clear();
                cbTitles.Text = "";
                ids = new List<string>();
                cbTitles.Visible = false;
                label3.Visible = false;
            }
            else if (di.IsReady && txtTitle.Text == "")
            {
                temp = client.DownloadString("http://www.imdb.com/find?q=" + di.VolumeLabel.Replace("_", "+") + ";s=tt");
                title = temp.Substring(temp.IndexOf("<title>") + 7, temp.IndexOf("</title>") - temp.IndexOf("<title>") - 7).Replace("&#233;", "é").Replace("&#38;", "&");
            }
            else 
            {
                temp = client.DownloadString("http://www.imdb.com/find?q=" + txtTitle.Text.Replace("_", "+") + ";s=tt");
                title = temp.Substring(temp.IndexOf("<title>") + 7, temp.IndexOf("</title>") - temp.IndexOf("<title>") - 7).Replace("&#233;", "é").Replace("&#38;", "&");

            }

            string id = temp.Substring(temp.IndexOf("link=/title/tt") + 15, temp.IndexOf("/", temp.IndexOf("link=/title/tt") + 15) - temp.IndexOf("link=/title/tt") - 15);

            txtTitle.Text = title;
            retry:
            if (title != "IMDb Title  Search" && title != "IMDb  Search")
            {
                string genre = "";
                int iStart = temp.IndexOf("a href", temp.IndexOf("Genre"));
                while (temp.Substring(iStart, 6) != "</div>")
                {
                    string t2 = temp.Substring(temp.IndexOf(">", iStart) + 1, temp.IndexOf("<", temp.IndexOf(">", iStart)) - temp.IndexOf(">", iStart) - 1) + ",";
                    if (t2 != " / ," && t2 != " ," && t2 != "more," && t2 != "\n,")
                    {
                        genre += t2;
                    }
                    iStart = temp.IndexOf("<", temp.IndexOf(">", iStart));
                }
                genre = genre.Substring(0, genre.Length - 1);
                string poster = temp.Substring(temp.IndexOf("src=", temp.IndexOf("<a name=\"poster")) + 4, temp.IndexOf("/>", temp.IndexOf("src=", temp.IndexOf("<a name=\"poster"))) - temp.IndexOf("src=", temp.IndexOf("<a name=\"poster")) - 4).Replace("\r", "").Replace("\n", "").Replace("\"", "").Trim();
                cover = client.DownloadData(poster);
                txtGenre.Text = genre;
                System.IO.MemoryStream ms = new System.IO.MemoryStream(cover);
                pbCover.Image = Image.FromStream(ms);
                ms.Close();
                ms.Dispose();
                ms = null;
                pbCover.Tag = id;
            }
            else
            {
                int iStart = temp.IndexOf("Titles");
                if (iStart == -1)
                {
                    MessageBox.Show("Can't find this movie.  Please enter the title and try again.");
                }
                else
                {
                    cbTitles.Items.Clear();
                    while (temp.Substring(iStart, 23) != "&#34;H&#246;&#246;k&#34")
                    {
                        string o = temp.Substring(iStart, 4);
                        string l = temp.Substring(temp.IndexOf(">", temp.IndexOf("<a", iStart)) + 1);
                        string t2 = temp.Substring(temp.IndexOf(">", temp.IndexOf("<a", iStart)) + 1, temp.IndexOf("</td", temp.IndexOf(">", temp.IndexOf("<a", iStart))) - temp.IndexOf(">", temp.IndexOf("<a", iStart)) - 1).Replace("</a>", "").Replace("#34", "\"").Replace("<small>", "").Replace("</small>", "").Replace("<br>", " ").Replace("&#160;", " ").Replace("<em>", "").Replace("</em>", "").Replace("&\";", "\"").Replace("&#233;", "é").Replace("&#38;", "&");
                        if (t2 == "&#34;H&#246;&#246;k&#34;" || t2.Contains("AKA Title Search for"))
                        {
                            break;
                        }
                        if (t2 != " / ," && t2 != " ," && t2 != "more," && t2 != "\n," && !t2.Contains("Displaying") && t2 != "," && !t2.Contains("<"))
                        {
                            cbTitles.Items.Add(t2);
                            ids.Add(temp.Substring(temp.IndexOf("title", iStart) + 8, temp.IndexOf("/", temp.IndexOf("title", iStart) + 8) - temp.IndexOf("title", iStart) - 8));
                        }
                        iStart = temp.IndexOf("</td", temp.IndexOf(">", temp.IndexOf("<a",iStart)));
                    }
                    MessageBox.Show("Found multiple results for this title.  Please select one from the list.");
                    label3.Visible = true;
                    cbTitles.Visible = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select * from movies where movie_id = " + pbCover.Tag.ToString() + ";", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                SQLiteCommand cmd = new SQLiteCommand("insert into movies (movie_id, title, cover) values (@movie_id, @title, @cover)", conn);
                SQLiteParameter c = new SQLiteParameter("@cover");
                cmd.Parameters.Add("@movie_id", DbType.Int32).Value = pbCover.Tag.ToString();
                cmd.Parameters.Add("@title", DbType.String, 100).Value = txtTitle.Text;
                c.Value = cover;
                cmd.Parameters.Add(c);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                c = null;
                da = new SQLiteDataAdapter("select * from genres;", conn);
                dt = new DataTable();
                da.Fill(dt);
                string[] gens = txtGenre.Text.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < gens.Length; i++)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j]["genre"].ToString() == gens[i])
                        {
                            da = new SQLiteDataAdapter("insert into movie_genres values (" + dt.Rows[j]["genre_id"].ToString() + "," + pbCover.Tag.ToString() + ");", conn);
                            da.Fill(new DataTable());
                            break;
                        }
                    }
                }
                gens = null;
                cmd.Dispose();
                cmd = null;
                MessageBox.Show("Successfully added " + txtTitle.Text + " to the database");
            }
            else
            {
                MessageBox.Show("Movie has already been added");
            }
            dt.Dispose();
            dt = null;
            da.Dispose();
            da = null;
            conn.Dispose();
            conn = null;
            txtTitle.Text = "";
            txtGenre.Text = "";
            pbCover.Image = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DriveInfo di = new DriveInfo(comboBox1.Text.Substring(0, 1));
            if (!di.IsReady)
            {
                comboBox1.Items.Remove(comboBox1.Text);
                comboBox1.Items.Add(di.Name);
                comboBox1.SelectedItem = di.Name;
            }
            else if ( di.IsReady && di.Name + di.VolumeLabel != comboBox1.Text)
            {
                comboBox1.Items.Remove(comboBox1.Text);
                comboBox1.Items.Add(di.Name + di.VolumeLabel);
                comboBox1.SelectedItem = di.Name + di.VolumeLabel;
                FileStream fs = new FileStream("D:\\VIDEO_TS\\VIDEO_TS.ifo", FileMode.Open, FileAccess.Read);
                fs.Seek(212, SeekOrigin.Begin);
                byte[] read = new byte[4];
                

                //byte[] all = new byte[fs.Length];
                //fs.Seek(0, SeekOrigin.Begin);
                //fs.Read(all, 0, (int)fs.Length);
                fs.Read(read, 0, 4);
                long offset = 0;
                offset = long.Parse(read[0].ToString() + read[1].ToString() + read[2].ToString() + read[3].ToString(), System.Globalization.NumberStyles.HexNumber);
                offset = offset * 2048;
                offset = offset + 27;
                fs.Seek(offset, SeekOrigin.Begin);
                byte[] textStart = new byte[1];
                fs.Read(textStart, 0, 1);
                offset = offset + 4;
                fs.Seek(offset, SeekOrigin.Begin);
                byte[] len = new byte[1];   
                fs.Read(len, 0, 1);
                offset = offset + 4 + (int.Parse(len[0].ToString()) - int.Parse(textStart[0].ToString()));
                fs.Seek(offset, SeekOrigin.Begin);
                int textLength = (int.Parse(len[0].ToString()) - int.Parse(textStart[0].ToString()));
                //offset = offset + (long.Parse(len[0].ToString()) - long.Parse(textStart[0].ToString()));
                //fs.Seek(offset, SeekOrigin.Begin);
                byte[] tit = new byte[int.Parse(textStart[0].ToString())];
                fs.Read(tit, 0, int.Parse(textStart[0].ToString()));
                txtTitle.Text = System.Text.Encoding.Default.GetString(tit).Trim().Replace("\t", "").Replace("\0", "");
                fs.Close();
                fs.Dispose();
                fs = null;
                tit = null;
                len = null;
                textStart = null;
                read = null;
                button1_Click(null, null);
            }
            
        }

        private void cbTitles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTitles.SelectedIndex > -1)
            {
                button1_Click(null, null);
            }
        }
    }
}