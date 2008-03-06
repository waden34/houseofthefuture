namespace HouseOfTheFuture
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
            this.btnColorBack = new System.Windows.Forms.Button();
            this.btnColorFore = new System.Windows.Forms.Button();
            this.panelSetupDevices = new System.Windows.Forms.Panel();
            this.btnAddDevice = new System.Windows.Forms.Button();
            this.panelSetupDevicesContent = new System.Windows.Forms.Panel();
            this.panelSetupActivities = new System.Windows.Forms.Panel();
            this.btnAddActivity = new System.Windows.Forms.Button();
            this.panelSetupActivitiesContent = new System.Windows.Forms.Panel();
            this.panelSetupGeneral = new System.Windows.Forms.Panel();
            this.btnGeneralSetup = new System.Windows.Forms.Button();
            this.timerContentSlider = new System.Windows.Forms.Timer(this.components);
            this.panelActivities = new System.Windows.Forms.Panel();
            this.panelLighting = new System.Windows.Forms.Panel();
            this.panelDevices = new System.Windows.Forms.Panel();
            this.timerSetupSlider = new System.Windows.Forms.Timer(this.components);
            this.timerLock = new System.Windows.Forms.Timer(this.components);
            this.lblColorBack = new System.Windows.Forms.Label();
            this.lblColorFore = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editCommandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeEmitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panel1.SuspendLayout();
            this.panelSetup.SuspendLayout();
            this.panelSetupDevices.SuspendLayout();
            this.panelSetupActivities.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnActivities
            // 
            this.btnActivities.FlatAppearance.BorderColor = this.backColor;
            this.btnActivities.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivities.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActivities.ForeColor = this.foreColor;
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
            this.panel1.Size = new System.Drawing.Size(153, 480);
            this.panel1.TabIndex = 1;
            // 
            // btnSetup
            // 
            this.btnSetup.FlatAppearance.BorderColor = this.backColor;
            this.btnSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetup.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetup.ForeColor = this.foreColor;
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
            this.btnDevices.FlatAppearance.BorderColor = this.backColor;
            this.btnDevices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDevices.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDevices.ForeColor = this.foreColor;
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
            this.btnLighting.FlatAppearance.BorderColor = this.backColor;
            this.btnLighting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLighting.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLighting.ForeColor = this.foreColor;
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
            this.lblTime.ForeColor = this.foreColor;
            this.lblTime.Location = new System.Drawing.Point(691, 456);
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
            this.panelSetup.Controls.Add(this.panelSetupActivities);
            this.panelSetup.Controls.Add(this.panelSetupGeneral);
            this.panelSetup.Controls.Add(this.btnGeneralSetup);
            this.panelSetup.Location = new System.Drawing.Point(-659, 1);
            this.panelSetup.Name = "panelSetup";
            this.panelSetup.Size = new System.Drawing.Size(658, 480);
            this.panelSetup.TabIndex = 3;
            this.panelSetup.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelSetup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // btnLightingSetup
            // 
            this.btnLightingSetup.AutoSize = true;
            this.btnLightingSetup.FlatAppearance.BorderColor = this.backColor;
            this.btnLightingSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLightingSetup.Font = new System.Drawing.Font("Cooper Black", 10F);
            this.btnLightingSetup.ForeColor = this.foreColor;
            this.btnLightingSetup.Location = new System.Drawing.Point(527, 11);
            this.btnLightingSetup.Name = "btnLightingSetup";
            this.btnLightingSetup.Size = new System.Drawing.Size(120, 30);
            this.btnLightingSetup.TabIndex = 2;
            this.btnLightingSetup.Text = "Lighting";
            this.btnLightingSetup.UseVisualStyleBackColor = true;
            // 
            // btnActivitySetup
            // 
            this.btnActivitySetup.AutoSize = true;
            this.btnActivitySetup.FlatAppearance.BorderColor = this.backColor;
            this.btnActivitySetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivitySetup.Font = new System.Drawing.Font("Cooper Black", 10F);
            this.btnActivitySetup.ForeColor = this.foreColor;
            this.btnActivitySetup.Location = new System.Drawing.Point(258, 11);
            this.btnActivitySetup.Name = "btnActivitySetup";
            this.btnActivitySetup.Size = new System.Drawing.Size(120, 30);
            this.btnActivitySetup.TabIndex = 1;
            this.btnActivitySetup.Text = "Activities";
            this.btnActivitySetup.UseVisualStyleBackColor = true;
            this.btnActivitySetup.Click += new System.EventHandler(this.btnActivitySetup_Click);
            // 
            // btnDeviceSetup
            // 
            this.btnDeviceSetup.AutoSize = true;
            this.btnDeviceSetup.FlatAppearance.BorderColor = this.backColor;
            this.btnDeviceSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeviceSetup.Font = new System.Drawing.Font("Cooper Black", 10F);
            this.btnDeviceSetup.ForeColor = this.foreColor;
            this.btnDeviceSetup.Location = new System.Drawing.Point(11, 11);
            this.btnDeviceSetup.Name = "btnDeviceSetup";
            this.btnDeviceSetup.Size = new System.Drawing.Size(120, 30);
            this.btnDeviceSetup.TabIndex = 0;
            this.btnDeviceSetup.Text = "Devices";
            this.btnDeviceSetup.UseVisualStyleBackColor = true;
            this.btnDeviceSetup.Click += new System.EventHandler(this.btnDeviceSetup_Click);
            // 
            // panelSetupDevices
            // 
            this.panelSetupDevices.Controls.Add(this.btnAddDevice);
            this.panelSetupDevices.Controls.Add(this.panelSetupDevicesContent);
            this.panelSetupDevices.Location = new System.Drawing.Point(-659, 1);
            this.panelSetupDevices.Name = "panelSetupDevices";
            this.panelSetupDevices.Size = new System.Drawing.Size(658, 480);
            this.panelSetupDevices.TabIndex = 3;
            this.panelSetupDevices.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelSetupDevices.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // btnAddDevice
            // 
            this.btnAddDevice.AutoSize = true;
            this.btnAddDevice.FlatAppearance.BorderColor = this.backColor;
            this.btnAddDevice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddDevice.Font = new System.Drawing.Font("Cooper Black", 10F);
            this.btnAddDevice.ForeColor = this.foreColor;
            this.btnAddDevice.Location = new System.Drawing.Point(20, 46);
            this.btnAddDevice.Name = "btnAddDevice";
            this.btnAddDevice.Size = new System.Drawing.Size(120, 30);
            this.btnAddDevice.TabIndex = 0;
            this.btnAddDevice.Text = "Add Device";
            this.btnAddDevice.UseVisualStyleBackColor = true;
            this.btnAddDevice.Click += new System.EventHandler(this.btnAddDevice_Click);
            // 
            // panelSetupDevicesContent
            // 
            this.panelSetupDevicesContent.Location = new System.Drawing.Point(0, 80);
            this.panelSetupDevicesContent.Name = "panelSetupDevicesContent";
            this.panelSetupDevicesContent.Size = new System.Drawing.Size(654, 400);
            this.panelSetupDevicesContent.TabIndex = 1;
            this.panelSetupDevicesContent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelSetupDevicesContent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // panelSetupActivities
            // 
            this.panelSetupActivities.Controls.Add(this.btnAddActivity);
            this.panelSetupActivities.Controls.Add(this.panelSetupActivitiesContent);
            this.panelSetupActivities.Location = new System.Drawing.Point(-659, 1);
            this.panelSetupActivities.Name = "panelSetupActivities";
            this.panelSetupActivities.Size = new System.Drawing.Size(658, 480);
            this.panelSetupActivities.TabIndex = 55;
            this.panelSetupActivities.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelSetupActivities.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // btnAddActivity
            // 
            this.btnAddActivity.AutoSize = true;
            this.btnAddActivity.FlatAppearance.BorderColor = this.backColor;
            this.btnAddActivity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddActivity.Font = new System.Drawing.Font("Cooper Black", 10F);
            this.btnAddActivity.ForeColor = this.foreColor;
            this.btnAddActivity.Location = new System.Drawing.Point(2, 46);
            this.btnAddActivity.Name = "btnAddActivity";
            this.btnAddActivity.Size = new System.Drawing.Size(120, 30);
            this.btnAddActivity.TabIndex = 0;
            this.btnAddActivity.Text = "Add Activity";
            this.btnAddActivity.UseVisualStyleBackColor = true;
            this.btnAddActivity.Click += new System.EventHandler(this.btnAddActivity_Click);
            // 
            // panelSetupActivitiesContent
            // 
            this.panelSetupActivitiesContent.Location = new System.Drawing.Point(0, 80);
            this.panelSetupActivitiesContent.Name = "panelSetupActivitiesContent";
            this.panelSetupActivitiesContent.Size = new System.Drawing.Size(654, 400);
            this.panelSetupActivitiesContent.TabIndex = 1;
            this.panelSetupActivitiesContent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelSetupActivitiesContent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // panelSetupGeneral
            // 
            this.panelSetupGeneral.Controls.Add(this.lblColorBack);
            this.panelSetupGeneral.Controls.Add(this.lblColorFore);
            this.panelSetupGeneral.Controls.Add(this.btnColorBack);
            this.panelSetupGeneral.Controls.Add(this.btnColorFore);
            this.panelSetupGeneral.Location = new System.Drawing.Point(-659, 1);
            this.panelSetupGeneral.Name = "panelSetupGeneral";
            this.panelSetupGeneral.Size = new System.Drawing.Size(658, 480);
            this.panelSetupGeneral.TabIndex = 3;
            this.panelSetupGeneral.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelSetupGeneral.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // btnGeneralSetup
            // 
            this.btnGeneralSetup.FlatAppearance.BorderColor = this.backColor;
            this.btnGeneralSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGeneralSetup.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGeneralSetup.ForeColor = this.foreColor;
            this.btnGeneralSetup.Location = new System.Drawing.Point(1, 11);
            this.btnGeneralSetup.Name = "btnGeneralSetup";
            this.btnGeneralSetup.Size = new System.Drawing.Size(120, 30);
            this.btnGeneralSetup.TabIndex = 0;
            this.btnGeneralSetup.Text = "General";
            this.btnGeneralSetup.UseVisualStyleBackColor = true;
            this.btnGeneralSetup.Click += new System.EventHandler(this.btnSetupGeneral_Click);
            this.btnGeneralSetup.BringToFront();
            //
            // lblColorFore
            //
            this.lblColorFore.AutoSize = true;
            this.lblColorFore.Font = new System.Drawing.Font("Cooper Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorFore.ForeColor = this.foreColor;
            this.lblColorFore.Location = new System.Drawing.Point(30, 41);
            this.lblColorFore.Name = "lblColorFore";
            this.lblColorFore.Text = "Text Color";
            this.lblColorFore.Size = new System.Drawing.Size(0, 16);
            this.lblColorFore.TabIndex = 2;
            //
            // lblColorBack
            //
            this.lblColorBack.AutoSize = true;
            this.lblColorBack.Font = new System.Drawing.Font("Cooper Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorBack.ForeColor = this.foreColor;
            this.lblColorBack.Location = new System.Drawing.Point(100, 41);
            this.lblColorBack.Name = "lblColorBack";
            this.lblColorBack.Text = "Background Color";
            this.lblColorBack.Size = new System.Drawing.Size(0, 16);
            this.lblColorBack.TabIndex = 2;
            //
            // btnColorFore
            //
            this.btnColorFore.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnColorFore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColorFore.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnColorFore.ForeColor = this.foreColor;
            this.btnColorFore.BackColor = this.foreColor;
            this.btnColorFore.Location = new System.Drawing.Point(3, 41);
            this.btnColorFore.Name = "btnColorFore";
            this.btnColorFore.Size = new System.Drawing.Size(60, 60);
            this.btnColorFore.TabIndex = 0;
            this.btnColorFore.UseVisualStyleBackColor = true;
            this.btnColorFore.Click += new System.EventHandler(btnColorFore_Click);
            //
            // btnColorBack
            //
            this.btnColorBack.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnColorBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColorBack.Font = new System.Drawing.Font("Cooper Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnColorBack.ForeColor = this.foreColor;
            this.btnColorBack.BackColor = this.backColor;
            this.btnColorBack.Location = new System.Drawing.Point(140, 41);
            this.btnColorBack.Name = "btnColorBack";
            this.btnColorBack.Size = new System.Drawing.Size(60, 60);
            this.btnColorBack.TabIndex = 0;
            this.btnColorBack.UseVisualStyleBackColor = true;
            this.btnColorBack.Click += new System.EventHandler(btnColorBack_Click);
            // 
            // timerContentSlider
            // 
            this.timerContentSlider.Interval = 10;
            this.timerContentSlider.Tick += new System.EventHandler(this.timerContentSlider_Tick);
            // 
            // panelActivities
            // 
            this.panelActivities.Location = new System.Drawing.Point(1, 1);
            this.panelActivities.Name = "panelActivities";
            this.panelActivities.Size = new System.Drawing.Size(658, 480);
            this.panelActivities.TabIndex = 4;
            this.panelActivities.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelActivities.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // panelLighting
            // 
            this.panelLighting.Location = new System.Drawing.Point(-659, 1);
            this.panelLighting.Name = "panelLighting";
            this.panelLighting.Size = new System.Drawing.Size(658, 480);
            this.panelLighting.TabIndex = 5;
            this.panelLighting.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelLighting.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            // 
            // panelDevices
            // 
            this.panelDevices.Location = new System.Drawing.Point(-659, 1);
            this.panelDevices.Name = "panelDevices";
            this.panelDevices.Size = new System.Drawing.Size(658, 480);
            this.panelDevices.TabIndex = 12;
            this.panelDevices.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.panelDevices.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(160, 70);
            // 
            // editCommandsToolStripMenuItem
            // 
            this.editCommandsToolStripMenuItem.Name = "editCommandsToolStripMenuItem";
            this.editCommandsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.editCommandsToolStripMenuItem.Text = "Commands";
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
            this.BackColor = this.backColor;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.panelActivities);
            this.Controls.Add(this.panelSetup);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelDevices);
            this.Controls.Add(this.panelLighting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            this.panel1.ResumeLayout(false);
            this.panelSetup.ResumeLayout(false);
            this.panelSetup.PerformLayout();
            this.panelSetupDevices.ResumeLayout(false);
            this.panelSetupDevices.PerformLayout();
            this.panelSetupActivities.ResumeLayout(false);
            this.panelSetupActivities.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

 















        #endregion
        private System.Windows.Forms.Button btnGeneralSetup;
        private System.Windows.Forms.Panel panelSetupGeneral;
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
        private System.Windows.Forms.Panel panelLighting;
        private System.Windows.Forms.Panel panelDevices;
        private System.Windows.Forms.Panel panelSetupDevices;
        private System.Windows.Forms.Panel panelSetupDevicesContent;
        private System.Windows.Forms.Panel panelSetupActivities;
        private System.Windows.Forms.Panel panelSetupActivitiesContent;
        private System.Windows.Forms.Timer timerSetupSlider;
        private System.Windows.Forms.Button btnAddDevice;
        private System.Windows.Forms.Button btnAddActivity;
        private System.Windows.Forms.Button btnDeviceSetup;
        private System.Windows.Forms.Timer timerLock;
        private System.Windows.Forms.Label lblColorBack;
        private System.Windows.Forms.Label lblColorFore;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editCommandsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeEmitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btnColorBack;
        private System.Windows.Forms.Button btnColorFore;
    }
}

