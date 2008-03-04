using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HouseOfTheFuture
{
    public partial class inputBox : Form
    {
       
        public inputBox()
        {
            InitializeComponent();
        }
        
        public static InputResult getInput(string prompt, List<string> items)
        {
            using (inputBox frm = new inputBox())
            {
                frm.lblTitle.Text = prompt;
                frm.lblTitle.Left = (frm.Width - frm.lblTitle.Width) / 2;
                for (int i = 0; i < items.Count; i++)
                {
                    frm.comboBox1.Items.Add(items[i]);
                }
                frm.comboBox1.Visible = true;
                DialogResult result = frm.ShowDialog();
                InputResult retVal = new InputResult();
                if (result == DialogResult.OK)
                {
                    retVal.Text = frm.comboBox1.Text;
                }
                else
                {
                    retVal.Text = "";
                }
                return retVal;
            }
           
        }
        public static InputResult getInput(string prompt, List<string> items, List<string> devices)
        {
            using (inputBox frm = new inputBox())
            {
                frm.lblTitle.Text = prompt;
                frm.lblTitle.Left = (frm.Width - frm.lblTitle.Width) / 2;
                for (int i = 0; i < items.Count; i++)
                {
                    frm.comboBox1.Items.Add(items[i]);
                }
                frm.comboBox1.Visible = true;
                DialogResult result = frm.ShowDialog();
                InputResult retVal = new InputResult();
                if (result == DialogResult.OK)
                {
                    retVal.Text = frm.comboBox1.Text;
                    retVal.device_id = devices[frm.comboBox1.SelectedIndex];
                }
                else
                {
                    retVal.Text = "";
                }
                return retVal;
            }

        }
        public static InputResult getInput(string prompt)
        {
            using (inputBox frm = new inputBox())
            {
                frm.lblTitle.Text = prompt;
                frm.txtInput.Visible = true;
                frm.lblTitle.Left = (frm.Width - frm.lblTitle.Width) / 2;

                DialogResult result = frm.ShowDialog();
                InputResult retVal = new InputResult();
                if (result == DialogResult.OK)
                {
                    retVal.Text = frm.txtInput.Text;
                }
                else
                {
                    retVal.Text = "";
                }
                return retVal;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
    public class InputResult
    {
        public string Text;
        public string device_id;
    }
}
