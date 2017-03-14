namespace RobotNamespace
{
    partial class FormRobot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRobot));
            this.timerSteps = new System.Windows.Forms.Timer(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBoxBoard = new System.Windows.Forms.GroupBox();
            this.pictureBoxRobot = new System.Windows.Forms.PictureBox();
            this.listBoxSteps = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerPlay = new System.Windows.Forms.Timer(this.components);
            this.groupBoxHistory = new System.Windows.Forms.GroupBox();
            this.buttonReverse = new System.Windows.Forms.Button();
            this.timerReverse = new System.Windows.Forms.Timer(this.components);
            this.buttonAddPath = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonRecord = new System.Windows.Forms.Button();
            this.treeViewPath = new System.Windows.Forms.TreeView();
            this.groupBoxMap = new System.Windows.Forms.GroupBox();
            this.textBoxCurrentPath = new System.Windows.Forms.TextBox();
            this.labelCurrentPath = new System.Windows.Forms.Label();
            this.textBoxSelectedPath = new System.Windows.Forms.TextBox();
            this.labelSelectedPath = new System.Windows.Forms.Label();
            this.timerOriginalStraight = new System.Windows.Forms.Timer(this.components);
            this.timerOriginalReverse = new System.Windows.Forms.Timer(this.components);
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelLight = new System.Windows.Forms.Label();
            this.timerBlinkLight = new System.Windows.Forms.Timer(this.components);
            this.groupBoxBoard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRobot)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBoxHistory.SuspendLayout();
            this.groupBoxMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerSteps
            // 
            this.timerSteps.Interval = 5;
            this.timerSteps.Tick += new System.EventHandler(this.timerSteps_Tick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "pause");
            this.imageList1.Images.SetKeyName(1, "play");
            this.imageList1.Images.SetKeyName(2, "stop");
            this.imageList1.Images.SetKeyName(3, "record");
            this.imageList1.Images.SetKeyName(4, "refresh");
            this.imageList1.Images.SetKeyName(5, "grayLight");
            this.imageList1.Images.SetKeyName(6, "redLight");
            this.imageList1.Images.SetKeyName(7, "greenLight");
            // 
            // groupBoxBoard
            // 
            this.groupBoxBoard.Controls.Add(this.pictureBoxRobot);
            this.groupBoxBoard.Location = new System.Drawing.Point(5, 42);
            this.groupBoxBoard.Name = "groupBoxBoard";
            this.groupBoxBoard.Size = new System.Drawing.Size(449, 320);
            this.groupBoxBoard.TabIndex = 1;
            this.groupBoxBoard.TabStop = false;
            this.groupBoxBoard.Text = "The Board";
            // 
            // pictureBoxRobot
            // 
            this.pictureBoxRobot.Image = global::RobotNamespace.Properties.Resources.robot;
            this.pictureBoxRobot.Location = new System.Drawing.Point(195, 133);
            this.pictureBoxRobot.Name = "pictureBoxRobot";
            this.pictureBoxRobot.Size = new System.Drawing.Size(44, 40);
            this.pictureBoxRobot.TabIndex = 0;
            this.pictureBoxRobot.TabStop = false;
            // 
            // listBoxSteps
            // 
            this.listBoxSteps.ForeColor = System.Drawing.SystemColors.WindowText;
            this.listBoxSteps.FormattingEnabled = true;
            this.listBoxSteps.Location = new System.Drawing.Point(6, 19);
            this.listBoxSteps.Name = "listBoxSteps";
            this.listBoxSteps.ScrollAlwaysVisible = true;
            this.listBoxSteps.Size = new System.Drawing.Size(126, 121);
            this.listBoxSteps.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 367);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(701, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(73, 17);
            this.toolStripStatusLabel1.Text = "Time Per Step";
            // 
            // labelTime
            // 
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(13, 17);
            this.labelTime.Text = "0";
            // 
            // timerPlay
            // 
            this.timerPlay.Interval = 5;
            this.timerPlay.Tick += new System.EventHandler(this.timerPlay_Tick);
            // 
            // groupBoxHistory
            // 
            this.groupBoxHistory.Controls.Add(this.listBoxSteps);
            this.groupBoxHistory.Location = new System.Drawing.Point(461, 42);
            this.groupBoxHistory.Name = "groupBoxHistory";
            this.groupBoxHistory.Size = new System.Drawing.Size(138, 155);
            this.groupBoxHistory.TabIndex = 5;
            this.groupBoxHistory.TabStop = false;
            this.groupBoxHistory.Text = "History";
            // 
            // buttonReverse
            // 
            this.buttonReverse.Enabled = false;
            this.buttonReverse.Location = new System.Drawing.Point(182, 13);
            this.buttonReverse.Name = "buttonReverse";
            this.buttonReverse.Size = new System.Drawing.Size(75, 23);
            this.buttonReverse.TabIndex = 6;
            this.buttonReverse.Text = "Reverse";
            this.buttonReverse.UseVisualStyleBackColor = true;
            this.buttonReverse.Click += new System.EventHandler(this.buttonReverse_Click);
            // 
            // timerReverse
            // 
            this.timerReverse.Interval = 5;
            this.timerReverse.Tick += new System.EventHandler(this.timerReverse_Tick);
            // 
            // buttonAddPath
            // 
            this.buttonAddPath.Enabled = false;
            this.buttonAddPath.Location = new System.Drawing.Point(263, 13);
            this.buttonAddPath.Name = "buttonAddPath";
            this.buttonAddPath.Size = new System.Drawing.Size(75, 23);
            this.buttonAddPath.TabIndex = 7;
            this.buttonAddPath.Text = "Add Path";
            this.buttonAddPath.UseVisualStyleBackColor = true;
            this.buttonAddPath.Click += new System.EventHandler(this.buttonAddPath_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRefresh.ImageKey = "refresh";
            this.buttonRefresh.ImageList = this.imageList1;
            this.buttonRefresh.Location = new System.Drawing.Point(373, 13);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(81, 23);
            this.buttonRefresh.TabIndex = 8;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Enabled = false;
            this.buttonPlay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonPlay.ImageKey = "play";
            this.buttonPlay.ImageList = this.imageList1;
            this.buttonPlay.Location = new System.Drawing.Point(101, 12);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(75, 23);
            this.buttonPlay.TabIndex = 4;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonRecord
            // 
            this.buttonRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRecord.ImageIndex = 2;
            this.buttonRecord.ImageList = this.imageList1;
            this.buttonRecord.Location = new System.Drawing.Point(4, 13);
            this.buttonRecord.Name = "buttonRecord";
            this.buttonRecord.Size = new System.Drawing.Size(90, 23);
            this.buttonRecord.TabIndex = 0;
            this.buttonRecord.Text = "Record";
            this.buttonRecord.UseVisualStyleBackColor = true;
            this.buttonRecord.Click += new System.EventHandler(this.buttonRecord_Click);
            // 
            // treeViewPath
            // 
            this.treeViewPath.Location = new System.Drawing.Point(9, 25);
            this.treeViewPath.Name = "treeViewPath";
            this.treeViewPath.Size = new System.Drawing.Size(123, 128);
            this.treeViewPath.TabIndex = 9;
            this.treeViewPath.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewPath_NodeMouseDoubleClick);
            // 
            // groupBoxMap
            // 
            this.groupBoxMap.Controls.Add(this.treeViewPath);
            this.groupBoxMap.Location = new System.Drawing.Point(461, 203);
            this.groupBoxMap.Name = "groupBoxMap";
            this.groupBoxMap.Size = new System.Drawing.Size(138, 159);
            this.groupBoxMap.TabIndex = 10;
            this.groupBoxMap.TabStop = false;
            this.groupBoxMap.Text = "Map Pathes";
            // 
            // textBoxCurrentPath
            // 
            this.textBoxCurrentPath.Enabled = false;
            this.textBoxCurrentPath.Location = new System.Drawing.Point(604, 74);
            this.textBoxCurrentPath.Name = "textBoxCurrentPath";
            this.textBoxCurrentPath.Size = new System.Drawing.Size(91, 20);
            this.textBoxCurrentPath.TabIndex = 11;
            // 
            // labelCurrentPath
            // 
            this.labelCurrentPath.AutoSize = true;
            this.labelCurrentPath.Location = new System.Drawing.Point(605, 58);
            this.labelCurrentPath.Name = "labelCurrentPath";
            this.labelCurrentPath.Size = new System.Drawing.Size(84, 13);
            this.labelCurrentPath.TabIndex = 12;
            this.labelCurrentPath.Text = "Current Position";
            // 
            // textBoxSelectedPath
            // 
            this.textBoxSelectedPath.Enabled = false;
            this.textBoxSelectedPath.Location = new System.Drawing.Point(604, 127);
            this.textBoxSelectedPath.Name = "textBoxSelectedPath";
            this.textBoxSelectedPath.Size = new System.Drawing.Size(91, 20);
            this.textBoxSelectedPath.TabIndex = 13;
            // 
            // labelSelectedPath
            // 
            this.labelSelectedPath.AutoSize = true;
            this.labelSelectedPath.Location = new System.Drawing.Point(605, 111);
            this.labelSelectedPath.Name = "labelSelectedPath";
            this.labelSelectedPath.Size = new System.Drawing.Size(88, 13);
            this.labelSelectedPath.TabIndex = 14;
            this.labelSelectedPath.Text = "Selected Position";
            // 
            // timerOriginalStraight
            // 
            this.timerOriginalStraight.Interval = 5;
            this.timerOriginalStraight.Tick += new System.EventHandler(this.timerOriginalStraight_Tick);
            // 
            // timerOriginalReverse
            // 
            this.timerOriginalReverse.Interval = 5;
            this.timerOriginalReverse.Tick += new System.EventHandler(this.timerOriginalReverse_Tick);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelStatus.ImageKey = "(none)";
            this.labelStatus.ImageList = this.imageList1;
            this.labelStatus.Location = new System.Drawing.Point(484, 18);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(85, 13);
            this.labelStatus.TabIndex = 15;
            this.labelStatus.Text = "Status: Stopped";
            // 
            // labelLight
            // 
            this.labelLight.AutoSize = true;
            this.labelLight.ImageKey = "grayLight";
            this.labelLight.ImageList = this.imageList1;
            this.labelLight.Location = new System.Drawing.Point(573, 12);
            this.labelLight.Name = "labelLight";
            this.labelLight.Size = new System.Drawing.Size(19, 26);
            this.labelLight.TabIndex = 16;
            this.labelLight.Text = "    \r\n\r\n";
            // 
            // timerBlinkLight
            // 
            this.timerBlinkLight.Interval = 500;
            this.timerBlinkLight.Tick += new System.EventHandler(this.timerBlinkLight_Tick);
            // 
            // FormRobot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 389);
            this.Controls.Add(this.labelLight);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelSelectedPath);
            this.Controls.Add(this.textBoxSelectedPath);
            this.Controls.Add(this.labelCurrentPath);
            this.Controls.Add(this.textBoxCurrentPath);
            this.Controls.Add(this.groupBoxMap);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.buttonAddPath);
            this.Controls.Add(this.buttonReverse);
            this.Controls.Add(this.groupBoxHistory);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBoxBoard);
            this.Controls.Add(this.buttonRecord);
            this.MaximizeBox = false;
            this.Name = "FormRobot";
            this.Text = "Robot Form";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormRobot_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormRobot_KeyUp);
            this.groupBoxBoard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRobot)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBoxHistory.ResumeLayout(false);
            this.groupBoxMap.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerSteps;
        private System.Windows.Forms.Button buttonRecord;
        private System.Windows.Forms.GroupBox groupBoxBoard;
        private System.Windows.Forms.PictureBox pictureBoxRobot;
        private System.Windows.Forms.ListBox listBoxSteps;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel labelTime;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Timer timerPlay;
        private System.Windows.Forms.GroupBox groupBoxHistory;
        private System.Windows.Forms.Button buttonReverse;
        private System.Windows.Forms.Timer timerReverse;
        private System.Windows.Forms.Button buttonAddPath;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.TreeView treeViewPath;
        private System.Windows.Forms.GroupBox groupBoxMap;
        private System.Windows.Forms.TextBox textBoxCurrentPath;
        private System.Windows.Forms.Label labelCurrentPath;
        private System.Windows.Forms.TextBox textBoxSelectedPath;
        private System.Windows.Forms.Label labelSelectedPath;
        private System.Windows.Forms.Timer timerOriginalStraight;
        private System.Windows.Forms.Timer timerOriginalReverse;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelLight;
        private System.Windows.Forms.Timer timerBlinkLight;
    }
}

