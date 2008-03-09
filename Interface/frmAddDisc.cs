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
    public partial class frmAddDisc : Form
    {
        public string database;
        public string unit_id;
        public string pos;
        List<string> disc_types;
        List<string> ids;
        byte[] cover;
        public frmAddDisc()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbTypes.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select a Disc Type");
                return;
            }
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteCommand cmd = new SQLiteCommand("Insert into discs values (null," + pbCover.Tag.ToString() + "," + disc_types[cbTypes.SelectedIndex] + ",\'" + txtTitle.Text.Replace("\'","\'\'") + "\',@Cover," + numericUpDown1.Value + "," + unit_id + "," + pos + ",0);", conn);
            SQLiteParameter c = new SQLiteParameter("@Cover");
            c.Value = cover;
            cmd.Parameters.Add(c);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            SQLiteDataAdapter da;
            DataTable dt;
            if (numericUpDown1.Value > 1)
            {
                da = new SQLiteDataAdapter("select max(disc_id) id from discs;", conn);
                dt = new DataTable();
                da.Fill(dt);
                int id = int.Parse(dt.Rows[0]["id"].ToString());
                for (int i = 2; i <= numericUpDown1.Value; i++)
                {
                    cmd = new SQLiteCommand("insert into discs values (" + (id + (i - 1)) + "," + pbCover.Tag.ToString() + "," + disc_types[cbTypes.SelectedIndex] + ",\'" + txtTitle.Text + " Disc " + i + "\',@Cover," + numericUpDown1.Value + "," + unit_id + "," + pos + ",1);", conn);
                    cmd.Parameters.Add(c);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                dt.Dispose();
                dt = null;
                da.Dispose();
                da = null;
            }

                da = new SQLiteDataAdapter("select * from genres;", conn);
                dt = new DataTable();
                da.Fill(dt);
                string[] gens = txtGenres.Text.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
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
                dt.Dispose();
                dt = null;
                da.Dispose();
                da = null;
            
            cmd.Dispose();
            cmd = null;
            
            
            c = null;
            
            conn.Dispose();
            conn = null;
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            string title = "";
            string temp = "";
            string id = "";
            System.Net.WebClient client = new System.Net.WebClient();

            if (cbTitles.Text != "")
            {
                if (cbTypes.Text == "Movie")
                {
                    temp = client.DownloadString("http://www.imdb.com/title/tt" + ids[cbTitles.SelectedIndex] + "/");
                    title = temp.Substring(temp.IndexOf("<title>") + 7, temp.IndexOf("</title>") - temp.IndexOf("<title>") - 7).Replace("&#233;", "é").Replace("&#38;", "&");
                }
                else if (cbTypes.Text == "Xbox 360")
                {
                    id = ids[cbTitles.SelectedIndex];
                    while (temp == "" || temp.Substring(0, 14) != "<!DOCTYPE html")
                    {
                        temp = client.DownloadString("http://xbox360.ign.com/objects/" + ids[cbTitles.SelectedIndex].Substring(0, 3) + "/" + ids[cbTitles.SelectedIndex] + ".html");
                        System.Threading.Thread.Sleep(1000);
                    }
                    title = temp.Substring(temp.IndexOf("IGN:") + 5, temp.IndexOf("</title>") - temp.IndexOf("IGN:") - 5).Trim();
                    
                }
                else if (cbTypes.Text == "PS3")
                {
                    id = ids[cbTitles.SelectedIndex];
                    while (temp == "" || temp.Substring(0, 14) != "<!DOCTYPE html")
                    {
                        temp = client.DownloadString("http://ps3.ign.com/objects/" + ids[cbTitles.SelectedIndex].Substring(0, 3) + "/" + ids[cbTitles.SelectedIndex] + ".html");
                        System.Threading.Thread.Sleep(4000);
                    }
                    title = temp.Substring(temp.IndexOf("IGN:") + 5, temp.IndexOf("</title>") - temp.IndexOf("IGN:") - 5).Trim();
                    
                }
                cbTitles.Items.Clear();
                cbTitles.Text = "";
                ids = new List<string>();
                cbTitles.Visible = false;
            }
            else
            {
                if (cbTypes.Text == "Movie")
                {
                    temp = client.DownloadString("http://www.imdb.com/find?q=" + txtTitle.Text.Replace("_", "+") + ";s=tt");
                    title = temp.Substring(temp.IndexOf("<title>") + 7, temp.IndexOf("</title>") - temp.IndexOf("<title>") - 7).Replace("&#233;", "é").Replace("&#38;", "&");
                }
                else if (cbTypes.Text == "Xbox 360")
                {
                    temp = client.DownloadString("http://search.ign.com/products?genNav=true&query=" + txtTitle.Text.Replace(" ","+") + "&platformSearch=Xbox%20360");

                }
                else if (cbTypes.Text == "PS3")
                {
                    temp = client.DownloadString("http://search.ign.com/products?genNav=true&query=" + txtTitle.Text.Replace(" ", "+") + "&platformSearch=PlayStation%203");
                }
                ids = new List<string>();
            }
            
            if (cbTypes.Text == "Movie")
            {
                id = temp.Substring(temp.IndexOf("link=/title/tt") + 15, temp.IndexOf("/", temp.IndexOf("link=/title/tt") + 15) - temp.IndexOf("link=/title/tt") - 15);
            }
            
            txtTitle.Text = title;
            if ((title != "IMDb Title  Search" && title != "IMDb  Search" && cbTypes.Text =="Movie") || (id != "" && (cbTypes.Text == "Xbox 360" || cbTypes.Text == "PS3")))
            {
                string genre = "";
                string poster = "";
                int iStart;
                if (cbTypes.Text == "Movie")
                {
                    iStart = temp.IndexOf("a href", temp.IndexOf("Genre"));
                    while (temp.Substring(iStart, 6) != "</div>")
                    {
                        string t2 = temp.Substring(temp.IndexOf(">", iStart) + 1, temp.IndexOf("<", temp.IndexOf(">", iStart)) - temp.IndexOf(">", iStart) - 1) + ",";
                        if (t2 != " / ," && t2 != " ," && t2 != "more," && t2 != "\n,")
                        {
                            genre += t2;
                        }
                        iStart = temp.IndexOf("<", temp.IndexOf(">", iStart));
                        t2 = null;
                    }
                    genre = genre.Substring(0, genre.Length - 1);
                    poster = temp.Substring(temp.IndexOf("src=", temp.IndexOf("<a name=\"poster")) + 4, temp.IndexOf("/>", temp.IndexOf("src=", temp.IndexOf("<a name=\"poster"))) - temp.IndexOf("src=", temp.IndexOf("<a name=\"poster")) - 4).Replace("\r", "").Replace("\n", "").Replace("\"", "").Trim();
                }
                else if (cbTypes.Text == "Xbox 360" || cbTypes.Text == "PS3")
                {
                    poster = temp.Substring(temp.IndexOf("src=", temp.IndexOf("objectDetailsBoxTop")) + 5, temp.IndexOf("\r", temp.IndexOf("src=", temp.IndexOf("objectDetailsBoxTop"))) - temp.IndexOf("src=", temp.IndexOf("objectDetailsBoxTop")) - 5).Replace("\"", "").Trim();
                    genre = temp.Substring(temp.IndexOf("<strong>", temp.IndexOf("Genre", temp.IndexOf("gameDetails"))) + 8, temp.IndexOf("</strong>", temp.IndexOf("<strong>", temp.IndexOf("Genre", temp.IndexOf("gameDetails")))) - temp.IndexOf("<strong>", temp.IndexOf("Genre", temp.IndexOf("gameDetails"))) - 8);
                }
                cover = client.DownloadData(poster);
                txtGenres.Text = genre;
                System.IO.MemoryStream ms = new System.IO.MemoryStream(cover);
                pbCover.Image = Image.FromStream(ms);
                ms.Close();
                ms.Dispose();
                ms = null;
                pbCover.Tag = id;
                poster = null;
                genre = null;

            }
            else
            {
                if (cbTypes.Text == "Movie")
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
                            t2 = null;
                            iStart = temp.IndexOf("</td", temp.IndexOf(">", temp.IndexOf("<a", iStart)));
                        }
                        MessageBox.Show("Found multiple results for this title.  Please select one from the list.");
                        label3.Visible = true;
                        cbTitles.Visible = true;
                    }
                }
                else if (cbTypes.Text == "Xbox 360" || cbTypes.Text == "PS3")
                {
                    int iStart = temp.IndexOf("searchResultTitle");
                    if (iStart == -1)
                    {
                        MessageBox.Show("Can't find this game.  Please check the title and try again.");
                    }
                    else
                    {
                        cbTitles.Items.Clear();
                        while (true)
                        {
                            string tempTitle = temp.Substring(temp.IndexOf(">", temp.IndexOf("<a", iStart)) + 1, temp.IndexOf("</a", temp.IndexOf(">", temp.IndexOf("<a", iStart))) - temp.IndexOf(">", temp.IndexOf("<a", iStart)) - 1).Replace("</a>", "").Replace("#34", "\"").Replace("<small>", "").Replace("</small>", "").Replace("<br>", " ").Replace("&#160;", " ").Replace("<em>", "").Replace("</em>", "").Replace("&\";", "\"").Replace("&#233;", "é").Replace("&#38;", "&");
                            cbTitles.Items.Add(tempTitle);
                            string lkl = temp.Substring(temp.IndexOf("/", temp.IndexOf("objects", iStart)) + 5);
                            ids.Add(temp.Substring(temp.IndexOf("/", temp.IndexOf("objects", iStart)) + 5, temp.IndexOf(".", temp.IndexOf("objects", iStart) + 5) - temp.IndexOf("/", temp.IndexOf("objects", iStart)) - 5));
                            tempTitle = null;
                            iStart = temp.IndexOf("searchResultTitle", iStart + 20);
                            if (iStart == -1)
                            {
                                break;
                            }
                        }
                        MessageBox.Show("Please select the correct game from the list below.");
                        cbTitles.Visible = true;
                        label3.Visible = true;
                    }
                }
            }
            id = null;
            temp = null;
            title = null;
        }

        private void cbTitles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTitles.SelectedIndex > -1)
            {
                btnInfo_Click(null, null);
            }
        }

        private void frmAddDisc_Load(object sender, EventArgs e)
        {
            disc_types = new List<string>();
            ids = new List<string>();
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + database);
            SQLiteDataAdapter da = new SQLiteDataAdapter("Select * from disc_types;", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cbTypes.Items.Add(dt.Rows[i]["type"].ToString());
                disc_types.Add(dt.Rows[i]["disc_type"].ToString());
                cbTypes.SelectedIndex = 0;
            }

            dt.Dispose();
            dt = null;
            da.Dispose();
            da  = null;
            conn.Dispose();
            conn = null;
        }
    }
}
