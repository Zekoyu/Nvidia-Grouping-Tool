
using System.Windows.Forms;

namespace Nvidia_Grouping_Tool {
    partial class MainWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startOnStartupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startMinimizedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeToTrayOnCloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportABugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourceCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupNowButton = new System.Windows.Forms.Button();
            this.detectedPathLabel = new System.Windows.Forms.Label();
            this.detectedDurationLabel = new System.Windows.Forms.Label();
            this.somethingWrongLink = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupIntervalLabel = new System.Windows.Forms.Label();
            this.folderCheckBox = new System.Windows.Forms.CheckBox();
            this.mergeIntervalPicker = new System.Windows.Forms.DateTimePicker();
            this.recordsDuration = new System.Windows.Forms.Label();
            this.recordsPath = new System.Windows.Forms.Label();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showApp = new System.Windows.Forms.ToolStripMenuItem();
            this.exitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.groupStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mergeProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mergeProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.mergeTimer = new System.Windows.Forms.Timer(this.components);
            this.cancelGroupButton = new System.Windows.Forms.Button();
            this.mainMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.trayMenuStrip.SuspendLayout();
            this.groupStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(334, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startOnStartupToolStripMenuItem,
            this.startMinimizedToolStripMenuItem,
            this.minimizeToTrayOnCloseToolStripMenuItem,
            this.updateValuesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // startOnStartupToolStripMenuItem
            // 
            this.startOnStartupToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.startOnStartupToolStripMenuItem.CheckOnClick = true;
            this.startOnStartupToolStripMenuItem.Name = "startOnStartupToolStripMenuItem";
            this.startOnStartupToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.startOnStartupToolStripMenuItem.Text = "Start on startup";
            this.startOnStartupToolStripMenuItem.Click += new System.EventHandler(this.OnStartOnStartupToolStripClick);
            // 
            // startMinimizedToolStripMenuItem
            // 
            this.startMinimizedToolStripMenuItem.CheckOnClick = true;
            this.startMinimizedToolStripMenuItem.Name = "startMinimizedToolStripMenuItem";
            this.startMinimizedToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.startMinimizedToolStripMenuItem.Text = "Start minimized in tray";
            this.startMinimizedToolStripMenuItem.Click += new System.EventHandler(this.OnStartMinimizedToolStripClick);
            // 
            // minimizeToTrayOnCloseToolStripMenuItem
            // 
            this.minimizeToTrayOnCloseToolStripMenuItem.CheckOnClick = true;
            this.minimizeToTrayOnCloseToolStripMenuItem.Name = "minimizeToTrayOnCloseToolStripMenuItem";
            this.minimizeToTrayOnCloseToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.minimizeToTrayOnCloseToolStripMenuItem.Text = "Minimize to tray";
            this.minimizeToTrayOnCloseToolStripMenuItem.Click += new System.EventHandler(this.OnMinimizeToTrayToolStripClick);
            // 
            // updateValuesToolStripMenuItem
            // 
            this.updateValuesToolStripMenuItem.Name = "updateValuesToolStripMenuItem";
            this.updateValuesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.updateValuesToolStripMenuItem.Text = "Update values";
            this.updateValuesToolStripMenuItem.Click += new System.EventHandler(this.OnUpdateValuesToolStripClick);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnExitAppClick);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportABugToolStripMenuItem,
            this.sourceCodeToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // reportABugToolStripMenuItem
            // 
            this.reportABugToolStripMenuItem.Name = "reportABugToolStripMenuItem";
            this.reportABugToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reportABugToolStripMenuItem.Text = "Report a bug";
            this.reportABugToolStripMenuItem.Click += new System.EventHandler(this.OnReportBugButtonClick);
            // 
            // sourceCodeToolStripMenuItem
            // 
            this.sourceCodeToolStripMenuItem.Name = "sourceCodeToolStripMenuItem";
            this.sourceCodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sourceCodeToolStripMenuItem.Text = "Source code";
            this.sourceCodeToolStripMenuItem.Click += new System.EventHandler(this.OnSourceCodeButtonClick);
            // 
            // groupNowButton
            // 
            this.groupNowButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupNowButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.groupNowButton.Location = new System.Drawing.Point(12, 140);
            this.groupNowButton.MinimumSize = new System.Drawing.Size(80, 25);
            this.groupNowButton.Name = "groupNowButton";
            this.groupNowButton.Size = new System.Drawing.Size(310, 45);
            this.groupNowButton.TabIndex = 1;
            this.groupNowButton.Text = "Group now";
            this.groupNowButton.UseVisualStyleBackColor = true;
            this.groupNowButton.Click += new System.EventHandler(this.OnGroupButtonClick);
            // 
            // detectedPathLabel
            // 
            this.detectedPathLabel.AutoSize = true;
            this.detectedPathLabel.Location = new System.Drawing.Point(10, 15);
            this.detectedPathLabel.Name = "detectedPathLabel";
            this.detectedPathLabel.Size = new System.Drawing.Size(130, 13);
            this.detectedPathLabel.TabIndex = 3;
            this.detectedPathLabel.Text = "Detected recordings path:";
            // 
            // detectedDurationLabel
            // 
            this.detectedDurationLabel.AutoSize = true;
            this.detectedDurationLabel.Location = new System.Drawing.Point(10, 30);
            this.detectedDurationLabel.Name = "detectedDurationLabel";
            this.detectedDurationLabel.Size = new System.Drawing.Size(147, 13);
            this.detectedDurationLabel.TabIndex = 4;
            this.detectedDurationLabel.Text = "Detected recordings duration:";
            // 
            // somethingWrongLink
            // 
            this.somethingWrongLink.AutoSize = true;
            this.somethingWrongLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(145)))), ((int)(((byte)(227)))));
            this.somethingWrongLink.Location = new System.Drawing.Point(10, 45);
            this.somethingWrongLink.Name = "somethingWrongLink";
            this.somethingWrongLink.Size = new System.Drawing.Size(107, 13);
            this.somethingWrongLink.TabIndex = 5;
            this.somethingWrongLink.TabStop = true;
            this.somethingWrongLink.Text = "Is something wrong ?";
            this.somethingWrongLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnIsSomethingWrongLinkClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupIntervalLabel);
            this.groupBox1.Controls.Add(this.folderCheckBox);
            this.groupBox1.Controls.Add(this.mergeIntervalPicker);
            this.groupBox1.Controls.Add(this.recordsDuration);
            this.groupBox1.Controls.Add(this.recordsPath);
            this.groupBox1.Controls.Add(this.detectedPathLabel);
            this.groupBox1.Controls.Add(this.somethingWrongLink);
            this.groupBox1.Controls.Add(this.detectedDurationLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 107);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // groupIntervalLabel
            // 
            this.groupIntervalLabel.AutoSize = true;
            this.groupIntervalLabel.Location = new System.Drawing.Point(10, 81);
            this.groupIntervalLabel.Name = "groupIntervalLabel";
            this.groupIntervalLabel.Size = new System.Drawing.Size(121, 13);
            this.groupIntervalLabel.TabIndex = 9;
            this.groupIntervalLabel.Text = "Group every: (hh:mm:ss)";
            // 
            // folderCheckBox
            // 
            this.folderCheckBox.AutoSize = true;
            this.folderCheckBox.Location = new System.Drawing.Point(13, 61);
            this.folderCheckBox.Name = "folderCheckBox";
            this.folderCheckBox.Size = new System.Drawing.Size(296, 17);
            this.folderCheckBox.TabIndex = 7;
            this.folderCheckBox.Text = "Put sessions files in a folder instead of merging the videos";
            this.folderCheckBox.UseVisualStyleBackColor = true;
            this.folderCheckBox.CheckedChanged += new System.EventHandler(this.OnFolderizeCheckBoxCheckChanged);
            // 
            // mergeIntervalPicker
            // 
            this.mergeIntervalPicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.mergeIntervalPicker.Location = new System.Drawing.Point(137, 78);
            this.mergeIntervalPicker.Name = "mergeIntervalPicker";
            this.mergeIntervalPicker.ShowUpDown = true;
            this.mergeIntervalPicker.Size = new System.Drawing.Size(70, 20);
            this.mergeIntervalPicker.TabIndex = 8;
            this.mergeIntervalPicker.Value = new System.DateTime(2021, 3, 18, 18, 9, 0, 0);
            this.mergeIntervalPicker.ValueChanged += new System.EventHandler(this.OnMergeIntervalPickerValueChange);
            // 
            // recordsDuration
            // 
            this.recordsDuration.AutoSize = true;
            this.recordsDuration.Location = new System.Drawing.Point(157, 30);
            this.recordsDuration.Name = "recordsDuration";
            this.recordsDuration.Size = new System.Drawing.Size(33, 13);
            this.recordsDuration.TabIndex = 7;
            this.recordsDuration.Text = "None";
            // 
            // recordsPath
            // 
            this.recordsPath.AutoSize = true;
            this.recordsPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.recordsPath.Location = new System.Drawing.Point(157, 15);
            this.recordsPath.Name = "recordsPath";
            this.recordsPath.Size = new System.Drawing.Size(33, 13);
            this.recordsPath.TabIndex = 6;
            this.recordsPath.Text = "None";
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenuStrip;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Nvidia Grouping Tool";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnTrayIconDoubleClick);
            // 
            // trayMenuStrip
            // 
            this.trayMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showApp,
            this.exitApp});
            this.trayMenuStrip.Name = "trayMenuStrip";
            this.trayMenuStrip.Size = new System.Drawing.Size(104, 48);
            // 
            // showApp
            // 
            this.showApp.Name = "showApp";
            this.showApp.Size = new System.Drawing.Size(103, 22);
            this.showApp.Text = "Show";
            this.showApp.Click += new System.EventHandler(this.OnShowButtonTrayClick);
            // 
            // exitApp
            // 
            this.exitApp.Name = "exitApp";
            this.exitApp.Size = new System.Drawing.Size(103, 22);
            this.exitApp.Text = "Exit";
            this.exitApp.Click += new System.EventHandler(this.OnExitAppClick);
            // 
            // groupStatusStrip
            // 
            this.groupStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mergeProgressLabel,
            this.mergeProgressBar});
            this.groupStatusStrip.Location = new System.Drawing.Point(0, 219);
            this.groupStatusStrip.Name = "groupStatusStrip";
            this.groupStatusStrip.Size = new System.Drawing.Size(334, 22);
            this.groupStatusStrip.TabIndex = 7;
            this.groupStatusStrip.Text = "statusStrip1";
            this.groupStatusStrip.Resize += new System.EventHandler(this.OnStatusStripResize);
            // 
            // mergeProgressLabel
            // 
            this.mergeProgressLabel.Name = "mergeProgressLabel";
            this.mergeProgressLabel.Size = new System.Drawing.Size(60, 17);
            this.mergeProgressLabel.Text = "Waiting ...";
            this.mergeProgressLabel.TextChanged += new System.EventHandler(this.OnProgressLabelChanged);
            // 
            // mergeProgressBar
            // 
            this.mergeProgressBar.AutoSize = false;
            this.mergeProgressBar.CausesValidation = false;
            this.mergeProgressBar.Name = "mergeProgressBar";
            this.mergeProgressBar.Size = new System.Drawing.Size(200, 16);
            this.mergeProgressBar.Step = 1;
            this.mergeProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // mergeTimer
            // 
            this.mergeTimer.Tick += new System.EventHandler(this.OnMergeTimerTick);
            // 
            // cancelGroupButton
            // 
            this.cancelGroupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelGroupButton.Enabled = false;
            this.cancelGroupButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.cancelGroupButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cancelGroupButton.Location = new System.Drawing.Point(12, 191);
            this.cancelGroupButton.MinimumSize = new System.Drawing.Size(80, 25);
            this.cancelGroupButton.Name = "cancelGroupButton";
            this.cancelGroupButton.Size = new System.Drawing.Size(310, 25);
            this.cancelGroupButton.TabIndex = 8;
            this.cancelGroupButton.Text = "Cancel";
            this.cancelGroupButton.UseVisualStyleBackColor = true;
            this.cancelGroupButton.Click += new System.EventHandler(this.OnCancelButtonClick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(334, 241);
            this.Controls.Add(this.cancelGroupButton);
            this.Controls.Add(this.groupStatusStrip);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupNowButton);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 350);
            this.MinimumSize = new System.Drawing.Size(350, 280);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nvidia Grouping Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnAppClose);
            this.Load += new System.EventHandler(this.OnMainWindowLoad);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.trayMenuStrip.ResumeLayout(false);
            this.groupStatusStrip.ResumeLayout(false);
            this.groupStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startOnStartupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startMinimizedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minimizeToTrayOnCloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Label detectedPathLabel;
        private System.Windows.Forms.Label detectedDurationLabel;
        private System.Windows.Forms.LinkLabel somethingWrongLink;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label recordsPath;
        private System.Windows.Forms.Label recordsDuration;
        private ToolStripMenuItem reportABugToolStripMenuItem;
        private ToolStripMenuItem sourceCodeToolStripMenuItem;
        private CheckBox folderCheckBox;
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenuStrip;
        private ToolStripMenuItem showApp;
        private ToolStripMenuItem exitApp;
        private StatusStrip groupStatusStrip;
        private DateTimePicker mergeIntervalPicker;
        private Timer mergeTimer;
        private Label groupIntervalLabel;
        public ToolStripStatusLabel mergeProgressLabel;
        public ToolStripProgressBar mergeProgressBar;
        public Button groupNowButton;
        public Button cancelGroupButton;
        private ToolStripMenuItem updateValuesToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}

