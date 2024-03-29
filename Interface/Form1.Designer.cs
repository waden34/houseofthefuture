﻿namespace Lighting_Interface
{
    partial class frmMain
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
            this.btnActivities = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnDevices = new System.Windows.Forms.Button();
            this.btnLighting = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.timerPing = new System.Windows.Forms.Timer(this.components);
            this.timerSlider = new System.Windows.Forms.Timer(this.components);
            this.timerSlideBack = new System.Windows.Forms.Timer(this.components);
            this.panelSetup = new System.Windows.Forms.Panel();
            this.btnLightingSetup = new System.Windows.Forms.Button();
            this.btnActivitySetup = new System.Windows.Forms.Button();
            this.btnDeviceSetup = new System.Windows.Forms.Button();
            this.panelSetupDevices = new System.Windows.Forms.Panel();
            this.btnAddDevice = new System.Windows.Forms.Button();
            this.timerContentSlider = new System.Windows.Forms.Timer(this.components);
            this.panelActivities = new System.Windows.Forms.Panel();
            this.panelLighting = new System.Windows.Forms.Panel();
            this.panelDevices = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.timerSetupSlider = new System.Windows.Forms.Timer(this.components);
            this.timerLock = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editCommandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeEmitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panelSetup.SuspendLayout();
            this.panelSetupDevices.SuspendLayout();
            this.panelActivities.SuspendLayout();
            this.panelLighting.SuspendLayout();
            this.panelDevices.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnActivities
            // 
            this.btnActivities.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnActivities.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivities.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActivities.ForeColor = System.Drawing.Color.White;
            this.btnActivities.Location = new System.Drawing.Point(1, 7);
            this.btnActivities.Name = "btnActivities";
            this.btnActivities.Size = new System.Drawing.Size(150, 98);
            this.btnActivities.TabIndex = 0;
            this.btnActivities.Text = "Activities";
            this.btnActivities.UseVisualStyleBackColor = true;
            this.btnActivities.Click += new System.EventHandler(this.btnActivities_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSetup);
            this.panel1.Controls.Add(this.btnDevices);
            this.panel1.Controls.Add(this.btnLighting);
            this.panel1.Controls.Add(this.btnActivities);
            this.panel1.Location = new System.Drawing.Point(781, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(153, 442);
            this.panel1.TabIndex = 1;
            // 
            // btnSetup
            // 
            this.btnSetup.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetup.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetup.ForeColor = System.Drawing.Color.White;
            this.btnSetup.Location = new System.Drawing.Point(1, 319);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(150, 98);
            this.btnSetup.TabIndex = 3;
            this.btnSetup.Text = "Setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Visible = false;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // btnDevices
            // 
            this.btnDevices.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnDevices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDevices.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDevices.ForeColor = System.Drawing.Color.White;
            this.btnDevices.Location = new System.Drawing.Point(1, 215);
            this.btnDevices.Name = "btnDevices";
            this.btnDevices.Size = new System.Drawing.Size(150, 98);
            this.btnDevices.TabIndex = 2;
            this.btnDevices.Text = "Devices";
            this.btnDevices.UseVisualStyleBackColor = true;
            this.btnDevices.Click += new System.EventHandler(this.btnDevices_Click);
            // 
            // btnLighting
            // 
            this.btnLighting.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnLighting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLighting.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLighting.ForeColor = System.Drawing.Color.White;
            this.btnLighting.Location = new System.Drawing.Point(1, 111);
            this.btnLighting.Name = "btnLighting";
            this.btnLighting.Size = new System.Drawing.Size(150, 98);
            this.btnLighting.TabIndex = 1;
            this.btnLighting.Text = "Lighting";
            this.btnLighting.UseVisualStyleBackColor = true;
            this.btnLighting.Click += new System.EventHandler(this.btnLighting_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Cooper Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(691, 426);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 16);
            this.lblTime.TabIndex = 2;
            // 
            // timerPing
            // 
            this.timerPing.Enabled = true;
            this.timerPing.Tick += new System.EventHandler(this.timerPing_Tick);
            // 
            // timerSlider
            // 
            this.timerSlider.Interval = 10;
            this.timerSlider.Tick += new System.EventHandler(this.timerSlider_Tick);
            // 
            // timerSlideBack
            // 
            this.timerSlideBack.Interval = 10;
            this.timerSlideBack.Tick += new System.EventHandler(this.timerSlideBack_Tick);
            // 
            // panelSetup
            // 
            this.panelSetup.Controls.Add(this.btnLightingSetup);
            this.panelSetup.Controls.Add(this.btnActivitySetup);
            this.panelSetup.Controls.Add(this.btnDeviceSetup);
            this.panelSetup.Controls.Add(this.panelSetupDevices);
            this.panelSetup.Location = new System.Drawing.Point(-659, 1);
            this.panelSetup.Name = "panelSetup";
            this.panelSetup.Size = new System.Drawing.Size(658, 442);
            this.panelSetup.TabIndex = 3;
            this.panelSetup.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelSetup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // btnLightingSetup
            // 
            this.btnLightingSetup.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnLightingSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLightingSetup.ForeColor = System.Drawing.Color.White;
            this.btnLightingSetup.Location = new System.Drawing.Point(287, 11);
            this.btnLightingSetup.Name = "btnLightingSetup";
            this.btnLightingSetup.Size = new System.Drawing.Size(120, 23);
            this.btnLightingSetup.TabIndex = 2;
            this.btnLightingSetup.Text = "Lighting";
            this.btnLightingSetup.UseVisualStyleBackColor = true;
            // 
            // btnActivitySetup
            // 
            this.btnActivitySetup.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnActivitySetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivitySetup.ForeColor = System.Drawing.Color.White;
            this.btnActivitySetup.Location = new System.Drawing.Point(149, 11);
            this.btnActivitySetup.Name = "btnActivitySetup";
            this.btnActivitySetup.Size = new System.Drawing.Size(120, 23);
            this.btnActivitySetup.TabIndex = 1;
            this.btnActivitySetup.Text = "Activities";
            this.btnActivitySetup.UseVisualStyleBackColor = true;
            // 
            // btnDeviceSetup
            // 
            this.btnDeviceSetup.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnDeviceSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeviceSetup.ForeColor = System.Drawing.Color.White;
            this.btnDeviceSetup.Location = new System.Drawing.Point(11, 11);
            this.btnDeviceSetup.Name = "btnDeviceSetup";
            this.btnDeviceSetup.Size = new System.Drawing.Size(120, 23);
            this.btnDeviceSetup.TabIndex = 0;
            this.btnDeviceSetup.Text = "Devices";
            this.btnDeviceSetup.UseVisualStyleBackColor = true;
            this.btnDeviceSetup.Click += new System.EventHandler(this.btnDeviceSetup_Click);
            // 
            // panelSetupDevices
            // 
            this.panelSetupDevices.Controls.Add(this.btnAddDevice);
            this.panelSetupDevices.Location = new System.Drawing.Point(-659, 1);
            this.panelSetupDevices.Name = "panelSetupDevices";
            this.panelSetupDevices.Size = new System.Drawing.Size(658, 442);
            this.panelSetupDevices.TabIndex = 3;
            this.panelSetupDevices.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelSetupDevices.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // btnAddDevice
            // 
            this.btnAddDevice.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnAddDevice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddDevice.ForeColor = System.Drawing.Color.White;
            this.btnAddDevice.Location = new System.Drawing.Point(31, 31);
            this.btnAddDevice.Name = "btnAddDevice";
            this.btnAddDevice.Size = new System.Drawing.Size(120, 23);
            this.btnAddDevice.TabIndex = 0;
            this.btnAddDevice.Text = "Add Device";
            this.btnAddDevice.UseVisualStyleBackColor = true;
            this.btnAddDevice.Click += new System.EventHandler(this.btnAddDevice_Click);
            // 
            // timerContentSlider
            // 
            this.timerContentSlider.Interval = 10;
            this.timerContentSlider.Tick += new System.EventHandler(this.timerContentSlider_Tick);
            // 
            // panelActivities
            // 
            this.panelActivities.Controls.Add(this.panelLighting);
            this.panelActivities.Controls.Add(this.button1);
            this.panelActivities.Location = new System.Drawing.Point(1, 1);
            this.panelActivities.Name = "panelActivities";
            this.panelActivities.Size = new System.Drawing.Size(658, 442);
            this.panelActivities.TabIndex = 4;
            this.panelActivities.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelActivities.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // panelLighting
            // 
            this.panelLighting.Controls.Add(this.panelDevices);
            this.panelLighting.Controls.Add(this.button2);
            this.panelLighting.Location = new System.Drawing.Point(1, 1);
            this.panelLighting.Name = "panelLighting";
            this.panelLighting.Size = new System.Drawing.Size(658, 442);
            this.panelLighting.TabIndex = 5;
            this.panelLighting.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelLighting.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // panelDevices
            // 
            this.panelDevices.Controls.Add(this.button3);
            this.panelDevices.Location = new System.Drawing.Point(1, 1);
            this.panelDevices.Name = "panelDevices";
            this.panelDevices.Size = new System.Drawing.Size(658, 442);
            this.panelDevices.TabIndex = 5;
            this.panelDevices.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelDevices.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(406, 106);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(406, 106);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(406, 106);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // timerSetupSlider
            // 
            this.timerSetupSlider.Interval = 10;
            this.timerSetupSlider.Tick += new System.EventHandler(this.timerSetupSlider_Tick);
            // 
            // timerLock
            // 
            this.timerLock.Interval = 3000;
            this.timerLock.Tick += new System.EventHandler(this.timerLock_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editCommandsToolStripMenuItem,
            this.changeEmitterToolStripMenuItem,
            this.renameToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(160, 92);
            // 
            // editCommandsToolStripMenuItem
            // 
            this.editCommandsToolStripMenuItem.Name = "editCommandsToolStripMenuItem";
            this.editCommandsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.editCommandsToolStripMenuItem.Text = "Edit Commands";
            this.editCommandsToolStripMenuItem.Click += new System.EventHandler(this.editCommandsToolStripMenuItem_Click);
            // 
            // changeEmitterToolStripMenuItem
            // 
            this.changeEmitterToolStripMenuItem.Name = "changeEmitterToolStripMenuItem";
            this.changeEmitterToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.changeEmitterToolStripMenuItem.Text = "Change Emitter";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(784, 444);
            this.Controls.Add(this.panelActivities);
            this.Controls.Add(this.panelSetup);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            this.panel1.ResumeLayout(false);
            this.panelSetup.ResumeLayout(false);
            this.panelSetupDevices.ResumeLayout(false);
            this.panelActivities.ResumeLayout(false);
            this.panelLighting.ResumeLayout(false);
            this.panelDevices.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }













        #endregion

        private System.Windows.Forms.Button btnActivities;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnDevices;
        private System.Windows.Forms.Button btnLighting;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer timerPing;
        private System.Windows.Forms.Timer timerSlider;
        private System.Windows.Forms.Timer timerSlideBack;
        private System.Windows.Forms.Panel panelSetup;
        private System.Windows.Forms.Button btnLightingSetup;
        private System.Windows.Forms.Button btnActivitySetup;
        private System.Windows.Forms.Timer timerContentSlider;
        private System.Windows.Forms.Panel panelActivities;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelLighting;
        private System.Windows.Forms.Panel panelDevices;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panelSetupDevices;
        private System.Windows.Forms.Timer timerSetupSlider;
        private System.Windows.Forms.Button btnAddDevice;
        public System.Windows.Forms.Button btnDeviceSetup;
        private System.Windows.Forms.Timer timerLock;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editCommandsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeEmitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
    }
}

