using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Nvidia_Grouping_Tool.ChoiceMessageBox;

namespace Nvidia_Grouping_Tool {
    public partial class MainWindow : Form {

        private BackgroundWorker backgroundGrouper;
        private BackgroundWorker BackgroundGrouper { get => backgroundGrouper; set => backgroundGrouper = value; }

        private bool closeAfterWork = false;
        private bool CloseAppAfterBackgroundWork { get => closeAfterWork; set => closeAfterWork = value; }


        private void ShowDialogDurationEverChanged() {
            ChoiceMessageBox recordsDurationEverChanged = new ChoiceMessageBox("Have you changed the shadowplay record duration before opening this app ?", "Yes", "No") {
                Height = 150
            };

            if (recordsDurationEverChanged.ShowDialog() == DialogResult.OK) {
                switch (recordsDurationEverChanged.ButtonClicked) {

                    case ButtonClick.Left:  // Potential issues, check if it's ok or not.
                        ShowDialogDurationChangedBeforeOrAfter();   // Compact name :)
                        break;

                    case ButtonClick.Right: // User never changed duration, no potential issue here.
                        MessageBox.Show("Everything is fine, enjoy :)");
                        Properties.Settings.Default.first_time_opening = false;
                        Properties.Settings.Default.Save();
                        break;
                }
            } else {
                MessageBox.Show("An error occured, please try again. If this is recurrent, you can report the issue on github (" + recordsDurationEverChanged.DialogResult + ")");
            }
        }

        private void ShowDialogDurationChangedBeforeOrAfter() {
            ChoiceMessageBox recordsDurationChangedBefore = new ChoiceMessageBox("Was it before you started recording, or after (which means you have different duration recordings in your folder, " +
                                                                                                     "if this is in another folder you can say before)", "Before", "After") {
                Height = 170
            };

            if (recordsDurationChangedBefore.ShowDialog() == DialogResult.OK) {
                switch (recordsDurationChangedBefore.ButtonClicked) {

                    case ButtonClick.Left:  // User changed before, no problem.
                        MessageBox.Show("Everything is fine, enjoy :)");
                        Properties.Settings.Default.first_time_opening = false;
                        Properties.Settings.Default.Save();
                        break;

                    case ButtonClick.Right: // User changed after, ask to move manually or automatically to avoid any problem.
                        ShowDialogMoveOldFiles();
                        break;
                }
            } else {
                MessageBox.Show("An error occured, please try again. If this is recurrent, you can report the issue on github (" + recordsDurationChangedBefore.DialogResult + ")");
            }
        }

        private void ShowDialogMoveOldFiles() {
            ChoiceMessageBox automaticallyMoveFiles = new ChoiceMessageBox("You need to move your videos from the root of the game folder, do you want to to it automatically (it will put the videos in a folder named \"old\"" +
                                                                                       " in the game folder (Example: Recordings/Minecraft/Old/[Your old videos here]) or manually ? If nothing is done it could result in a wrong merging" +
                                                                                       " of your videos", "Automatically", "Manually") {
                Height = 210
            };
            if (automaticallyMoveFiles.ShowDialog() == DialogResult.OK) {
                switch (automaticallyMoveFiles.ButtonClicked) {
                    case ButtonClick.Left:  // Automatically moves videos in a folder.
                        VideoGrouper.GroupOldVideos();
                        MessageBox.Show("Thanks for waiting, everything is all right now, enjoy :)");
                        Properties.Settings.Default.first_time_opening = false;
                        Properties.Settings.Default.Save();
                        break;
                    case ButtonClick.Right: // Wait for user to manually moves the videos.
                        MessageBox.Show("Click OK When you're done please (you just need to move the videos away from the root game folder (default location where videos are saved, you can pur them in a subfolder)",
                                        "Information", MessageBoxButtons.OK);
                        MessageBox.Show("Everything is fine, enjoy :)");
                        Properties.Settings.Default.first_time_opening = false;
                        Properties.Settings.Default.Save();
                        break;
                }
            } else {
                MessageBox.Show("An error occured, please try again. If this is recurrent, you can report the issue on github (" + automaticallyMoveFiles.DialogResult + ")");
            }
        }



        public MainWindow() {
            InitializeComponent();
            RetrieveSettings();

            
            if (this.startMinimizedToolStripMenuItem.Checked == true) {
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
            } else {
                Properties.Settings.Default.Upgrade();
                // First time opening, avoid wrong merging if the user had shorter / longer records in the past.
                if (Properties.Settings.Default.first_time_opening == true) {
                    ShowDialogDurationEverChanged();
                }
            }
        }

        private void OnMainWindowLoad(object sender, EventArgs e) {
            VideoGrouper.GetPrerequisite();
            InitializeTimer();
            InitializeBackgroundWorker();
            InitializeLabels();
            InitializeProgressBar();
            InitializeTimePicker();
            InitializeSize();
        }


        //
        // BackgroundWorker related methods.
        //
        private void InitializeBackgroundWorker() {
            this.BackgroundGrouper = new BackgroundWorker {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            this.BackgroundGrouper.ProgressChanged += OnBackgroundGrouperProgressChanged;
            this.BackgroundGrouper.RunWorkerCompleted += OnBackgroundGrouperCompleted;
        }
        private void OnBackgroundGrouperProgressChanged(object sender, ProgressChangedEventArgs e) {
            ValueTuple<int, int> args = (ValueTuple<int, int>)e.UserState;
            int currentSession = args.Item1;
            int totalSessions = args.Item2;
            this.mergeProgressBar.Value = e.ProgressPercentage;
            this.mergeProgressLabel.Text = $"Grouping ... ({currentSession}/{totalSessions})";
            Console.WriteLine($"Progress: {e.ProgressPercentage}% ({currentSession}/{totalSessions})");
        }

        private void OnBackgroundGrouperCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error != null) {
                MessageBox.Show("An error occured while grouping:\n" + e.Error.ToString());
                Console.WriteLine(e.Error.StackTrace);
            } else if (e.Cancelled) {
                // If app should close after the cancellation, do it.
                ShowTemporaryText(this.mergeProgressLabel, "Grouping canceled", "Waiting ...", 7500);
                if (this.CloseAppAfterBackgroundWork) {
                    this.CloseAppAfterBackgroundWork = false;
                    OnExitAppClick(null, null);
                }
            } else {
                ShowTemporaryText(this.mergeProgressLabel, "Grouping completed", "Waiting ...", 7500);
            }
            this.groupNowButton.Enabled = true;
            this.cancelGroupButton.Enabled = false;
        }

        private async void ShowTemporaryText(ToolStripStatusLabel label, string text, string textAfter, int durationMs) {
            label.Text = text;
            await Task.Delay(durationMs);

            // Just check if the user has not already restarted a group for instance.
            if (label.Text == text) {
                this.mergeProgressBar.Value = 0;
                label.Text = textAfter;
            }

        }

        private void ResetBackgroundGrouperWorkHandlers() {
            this.BackgroundGrouper.DoWork -= VideoGrouper.GroupVideosInBackground;
            this.BackgroundGrouper.DoWork -= VideoGrouper.MergeVideosInBackground;
        }


        //
        // Initialization methods.
        //
        private void InitializeTimer() {
            this.mergeTimer.Enabled = true;
            this.mergeTimer.Interval = Properties.Settings.Default.merge_interval * 1000;
            this.mergeTimer.Stop();
            this.mergeTimer.Start();
        }

        private void RetrieveSettings() {
            this.minimizeToTrayOnCloseToolStripMenuItem.Checked = Properties.Settings.Default.minimize_tray;
            this.startMinimizedToolStripMenuItem.Checked = Properties.Settings.Default.start_minimized;
            this.startOnStartupToolStripMenuItem.Checked = Properties.Settings.Default.start_with_windows;
            this.folderCheckBox.Checked = Properties.Settings.Default.folder_instead_of_merging;
            this.mergeIntervalPicker.Value = new DateTime(2004, 11, 07, 0, 0, 0).AddSeconds(Properties.Settings.Default.merge_interval);
        }

        private void InitializeLabels() {
            // Sets the right value on the labels.
            this.recordsPath.Text = new TimeSpan(0, 0, 0, NvidiaRetriever.RecordsDuration).ToString(@"mm\:ss");
            this.recordsDuration.Text = NvidiaRetriever.RecordsLocation;
        }

        private void InitializeProgressBar() {
            // Call the resize method so that it actually fits right.
            OnStatusStripResize(null, null);
        }

        private void InitializeTimePicker() {
            this.mergeIntervalPicker.MinDate = DateTime.Today.AddSeconds(NvidiaRetriever.RecordsDuration);
        }

        private void InitializeSize() {
            if (this.recordsPath.Bounds.Right > 300) {
                this.MinimumSize = new Size(this.recordsPath.Bounds.Right + 50, 280);
                this.MaximumSize = new Size(this.recordsPath.Bounds.Right + 350, 350);
            }
        }

        //
        // Foreground event handlers caused by direct user input.
        //

        // Grouping / Merging related methods.
        private void OnGroupButtonClick(object sender, EventArgs e) {
            this.groupNowButton.Enabled = false;
            this.cancelGroupButton.Enabled = true;
            // Resets the timer.
            this.mergeTimer.Stop();
            this.mergeTimer.Start();
            // If a merging / grouping job is already running, let it finish.
            if (backgroundGrouper.IsBusy) { return; }
            ResetBackgroundGrouperWorkHandlers();
            if (this.folderCheckBox.Checked) {
                backgroundGrouper.DoWork += VideoGrouper.GroupVideosInBackground;
            } else {
                backgroundGrouper.DoWork += VideoGrouper.MergeVideosInBackground;
            }
            backgroundGrouper.RunWorkerAsync();
        }

        private void OnCancelButtonClick(object sender, EventArgs e) {
            backgroundGrouper.CancelAsync();
        }


        // Others buttons related methods.

        private void OnReportBugButtonClick(object sender, EventArgs e) {
            Process.Start("https://github.com/Zekoyuu/Nvidia-Grouping-Tool/issues/new");
        }

        private void OnIsSomethingWrongLinkClick(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://github.com/Zekoyuu/Nvidia-Grouping-Tool/blob/master/README.md");
        }

        private void OnSourceCodeButtonClick(object sender, EventArgs e) {
            Process.Start("https://github.com/Zekoyuu/Nvidia-Grouping-Tool/");
        }

        private void OnTrayIconDoubleClick(object sender, MouseEventArgs e) {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void OnShowButtonTrayClick(object sender, EventArgs e) {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void OnAppClose(object sender, FormClosingEventArgs e) {
            // Minimize if the user chose to, else exit.
            if (this.minimizeToTrayOnCloseToolStripMenuItem.Checked == true) {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                trayIcon.Visible = true;
                e.Cancel = true;
            } else {
                if (this.backgroundGrouper.IsBusy) {
                    this.BackgroundGrouper.CancelAsync();
                    this.CloseAppAfterBackgroundWork = true;
                    e.Cancel = true;
                }

            }
        }

        private void OnExitAppClick(object sender, EventArgs e) {
            // If grouping / merging is in progress, wait for it to finish.
            if (this.backgroundGrouper.IsBusy) {
                this.BackgroundGrouper.CancelAsync();
                this.CloseAppAfterBackgroundWork = true;
            }

            if (!this.CloseAppAfterBackgroundWork) {
                // Exit even if the user chose to minimize.
                this.FormClosing -= OnAppClose;
                this.Close();
            }

        }

        private void OnStartOnStartupToolStripClick(object sender, EventArgs e) {
            try {
                // Simply add the executable path in the "Run" registry.
                RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (startOnStartupToolStripMenuItem.Checked == true) {
                    reg.SetValue("NvidiaGroupingTool", $"\"{ Application.ExecutablePath}\"");
                    Properties.Settings.Default.start_with_windows = true;
                    Properties.Settings.Default.Save();
                } else {
                    reg.DeleteValue("NvidiaGroupingTool", false);
                    Properties.Settings.Default.start_with_windows = false;
                    Properties.Settings.Default.Save();
                }
            } catch (Exception exception) {
                MessageBox.Show("An error occured (please report it):\n" + exception.ToString());
                startOnStartupToolStripMenuItem.Checked = false;
                Properties.Settings.Default.start_with_windows = false;
                Properties.Settings.Default.Save();
            }
        }

        private void OnUpdateValuesToolStripClick(object sender, EventArgs e) {
            InitializeLabels();
            InitializeTimePicker();
        }

        private void OnMinimizeToTrayToolStripClick(object sender, EventArgs e) {
            if (minimizeToTrayOnCloseToolStripMenuItem.Checked == true) {
                Properties.Settings.Default.minimize_tray = true;
            } else {
                Properties.Settings.Default.minimize_tray = false;
            }
            Properties.Settings.Default.Save();
        }

        private void OnStartMinimizedToolStripClick(object sender, EventArgs e) {
            if (startMinimizedToolStripMenuItem.Checked == true) {
                Properties.Settings.Default.start_minimized = true;
            } else {
                Properties.Settings.Default.start_minimized = false;
            }
            Properties.Settings.Default.Save();
        }

        private void OnFolderizeCheckBoxCheckChanged(object sender, EventArgs e) {
            if (folderCheckBox.Checked == true) {
                Properties.Settings.Default.folder_instead_of_merging = true;
            } else {
                Properties.Settings.Default.folder_instead_of_merging = false;
            }
            Properties.Settings.Default.Save();
        }

        //
        // Background event handlers caused by indirect user input.
        //

        private void OnMergeIntervalPickerValueChange(object sender, EventArgs e) {
            this.mergeIntervalPicker.MinDate = DateTime.Today.AddSeconds(NvidiaRetriever.RecordsDuration);
            // DateTimePicker to seconds.
            DateTime mergeIntervalDate = this.mergeIntervalPicker.Value;
            int mergeInterval = mergeIntervalDate.Second + mergeIntervalDate.Minute * 60 + mergeIntervalDate.Hour * 3600;
            Properties.Settings.Default.merge_interval = mergeInterval;
            Properties.Settings.Default.Save();

            InitializeTimer();
        }

        private void OnStatusStripResize(object sender, EventArgs e) {
            this.mergeProgressBar.Width = this.groupStatusStrip.Width - this.mergeProgressLabel.Width - 20;
        }

        private void OnProgressLabelChanged(object sender, EventArgs e) {
            this.mergeProgressBar.Width = groupStatusStrip.Width - mergeProgressLabel.Width - 20;
        }

        private void OnMergeTimerTick(object sender, EventArgs e) {
            InitializeLabels();
            InitializeTimePicker();
            groupNowButton.PerformClick();
        }

        
    }
}
