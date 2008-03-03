namespace Lighting_Interface
{
    partial class frmDiscInsert
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.rbAdd = new System.Windows.Forms.RadioButton();
            this.rbEjected = new System.Windows.Forms.RadioButton();
            this.cbDiscs = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(120, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "A Disc has Been Inserted";
            // 
            // rbAdd
            // 
            this.rbAdd.AutoSize = true;
            this.rbAdd.Font = new System.Drawing.Font("Cooper Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAdd.Location = new System.Drawing.Point(160, 76);
            this.rbAdd.Name = "rbAdd";
            this.rbAdd.Size = new System.Drawing.Size(127, 20);
            this.rbAdd.TabIndex = 1;
            this.rbAdd.Text = "Add New Disc";
            this.rbAdd.UseVisualStyleBackColor = true;
            // 
            // rbEjected
            // 
            this.rbEjected.AutoSize = true;
            this.rbEjected.Checked = true;
            this.rbEjected.Font = new System.Drawing.Font("Cooper Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbEjected.Location = new System.Drawing.Point(141, 113);
            this.rbEjected.Name = "rbEjected";
            this.rbEjected.Size = new System.Drawing.Size(164, 20);
            this.rbEjected.TabIndex = 2;
            this.rbEjected.TabStop = true;
            this.rbEjected.Text = "Return Ejected Disc";
            this.rbEjected.UseVisualStyleBackColor = true;
            // 
            // cbDiscs
            // 
            this.cbDiscs.FormattingEnabled = true;
            this.cbDiscs.Location = new System.Drawing.Point(63, 155);
            this.cbDiscs.Name = "cbDiscs";
            this.cbDiscs.Size = new System.Drawing.Size(320, 21);
            this.cbDiscs.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(186, 198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmDiscInsert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(447, 237);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbDiscs);
            this.Controls.Add(this.rbEjected);
            this.Controls.Add(this.rbAdd);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Cooper Black", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmDiscInsert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmDiscInsert_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbAdd;
        private System.Windows.Forms.RadioButton rbEjected;
        private System.Windows.Forms.ComboBox cbDiscs;
        private System.Windows.Forms.Button button1;
    }
}