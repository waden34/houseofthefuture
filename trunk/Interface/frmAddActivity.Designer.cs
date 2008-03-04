namespace HouseOfTheFuture
{
    partial class frmAddActivity
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtActivityName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.specialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tVGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.discBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightingControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roundButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.squareButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.triangleButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.longButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tallButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Cooper Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(387, 389);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOk
            // 
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Cooper Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(191, 389);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(107, 23);
            this.btnOk.TabIndex = 16;
            this.btnOk.Text = "Next";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cooper Black", 10F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(149, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "Activity Name:";
            // 
            // txtActivityName
            // 
            this.txtActivityName.Location = new System.Drawing.Point(271, 18);
            this.txtActivityName.Name = "txtActivityName";
            this.txtActivityName.Size = new System.Drawing.Size(233, 20);
            this.txtActivityName.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Cooper Black", 10F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(221, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 16);
            this.label2.TabIndex = 20;
            this.label2.Text = "Which Devices will be Used?";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.specialToolStripMenuItem,
            this.roundButtonToolStripMenuItem,
            this.squareButtonToolStripMenuItem,
            this.triangleButtonToolStripMenuItem,
            this.longButtonToolStripMenuItem,
            this.tallButtonToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(157, 158);
            // 
            // specialToolStripMenuItem
            // 
            this.specialToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tVGuideToolStripMenuItem,
            this.discBrowserToolStripMenuItem,
            this.lightingControlToolStripMenuItem});
            this.specialToolStripMenuItem.Name = "specialToolStripMenuItem";
            this.specialToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.specialToolStripMenuItem.Text = "Special";
            // 
            // tVGuideToolStripMenuItem
            // 
            this.tVGuideToolStripMenuItem.Name = "tVGuideToolStripMenuItem";
            this.tVGuideToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.tVGuideToolStripMenuItem.Text = "TV Guide";
            // 
            // discBrowserToolStripMenuItem
            // 
            this.discBrowserToolStripMenuItem.Name = "discBrowserToolStripMenuItem";
            this.discBrowserToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.discBrowserToolStripMenuItem.Text = "Disc Browser";
            this.discBrowserToolStripMenuItem.Click += new System.EventHandler(this.discBrowserToolStripMenuItem_Click);
            // 
            // lightingControlToolStripMenuItem
            // 
            this.lightingControlToolStripMenuItem.Name = "lightingControlToolStripMenuItem";
            this.lightingControlToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.lightingControlToolStripMenuItem.Text = "Lighting Control";
            // 
            // roundButtonToolStripMenuItem
            // 
            this.roundButtonToolStripMenuItem.Name = "roundButtonToolStripMenuItem";
            this.roundButtonToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.roundButtonToolStripMenuItem.Text = "Round Button";
            this.roundButtonToolStripMenuItem.Click += new System.EventHandler(this.roundButtonToolStripMenuItem_Click);
            // 
            // squareButtonToolStripMenuItem
            // 
            this.squareButtonToolStripMenuItem.Name = "squareButtonToolStripMenuItem";
            this.squareButtonToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.squareButtonToolStripMenuItem.Text = "Square Button";
            // 
            // triangleButtonToolStripMenuItem
            // 
            this.triangleButtonToolStripMenuItem.Name = "triangleButtonToolStripMenuItem";
            this.triangleButtonToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.triangleButtonToolStripMenuItem.Text = "Triangle Button";
            // 
            // longButtonToolStripMenuItem
            // 
            this.longButtonToolStripMenuItem.Name = "longButtonToolStripMenuItem";
            this.longButtonToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.longButtonToolStripMenuItem.Text = "Long Button";
            this.longButtonToolStripMenuItem.Click += new System.EventHandler(this.longButtonToolStripMenuItem_Click);
            // 
            // tallButtonToolStripMenuItem
            // 
            this.tallButtonToolStripMenuItem.Name = "tallButtonToolStripMenuItem";
            this.tallButtonToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.tallButtonToolStripMenuItem.Text = "Tall Button";
            this.tallButtonToolStripMenuItem.Click += new System.EventHandler(this.tallButtonToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(628, 312);
            this.panel1.TabIndex = 21;
            // 
            // frmAddActivity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(652, 436);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtActivityName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Cooper Black", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmAddActivity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmAddActivity_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtActivityName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem roundButtonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem squareButtonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem triangleButtonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem longButtonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tallButtonToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem specialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tVGuideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem discBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lightingControlToolStripMenuItem;

    }
}