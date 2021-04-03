using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xabe.FFmpeg.Downloader;

namespace Nvidia_Grouping_Tool {
    public class VideoGrouper {

        // Nvidia video name format.
        private static Regex nvidiaVideoFormat = new Regex(@".*\d{4}.\d{2}.\d{2} - \d{2}.\d{2}.\d{2}.\d{2}.DVR.mp4");
        // FFmpeg size format when concatenating (retrieves the size in kB).
        private static Regex ffmpegSizeFormat = new Regex(@"(?!size=\s*)\d+(?=(?i)kb)");
        private static string ffmpegLocation = $@"{Directory.GetCurrentDirectory()}\ressources\ffmpeg.exe";
        private static string concatFileLocation = $@"{Directory.GetCurrentDirectory()}\temp\concat.txt";

        static VideoGrouper() {
            GetPrerequisite();
        }

        public static void GetPrerequisite() {
            // Downloads FFMPEG if it's not already there.
            if (!File.Exists(ffmpegLocation)) {
                FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, $@"{Directory.GetCurrentDirectory()}\ressources\");
            }
            // Creates temp folder just in case.
            Directory.CreateDirectory(Path.GetDirectoryName(concatFileLocation));
        }

        public static void MergeVideosInBackground(object sender, DoWorkEventArgs e) {
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
            List<List<string>> sessionsList = RetrieveSessions();
            if (sessionsList.Count == 0) {
                return;
            }

            // Initialize report, do not wait for work to start (which takes about half a second or so).
            backgroundWorker.ReportProgress(0, (1, sessionsList.Count));

            foreach (List<string> session in sessionsList) {
                // Video name example: Recordings/Minecraft/Minecraft - 01-01-2020 12.30.00.mp4
                string videoFileName = $"{Path.GetDirectoryName(session[0])}\\{Path.GetFileName(Path.GetDirectoryName(session[0]))} - " +
                                       $"{File.GetCreationTime(session[0]).ToString("G").Replace('/', '-').Replace(':', '.')}.mp4";

                long totalSessionSizeInKb = 0;
                StreamWriter concatFile = new StreamWriter(concatFileLocation);

                foreach (string video in session) {
                    totalSessionSizeInKb += new FileInfo(video).Length / 1024;
                    concatFile.WriteLine($"file '{video}'");
                }

                concatFile.Close();

                // FFmpeg Process to merge videos.
                Process concatProcess = new Process {
                    StartInfo = {
                        WorkingDirectory = Directory.GetCurrentDirectory(),
                        FileName = "ressources\\ffmpeg.exe",
                        // FFmpeg command to concatenate videos from a text file.
                        Arguments = $"-f concat -safe 0 -i temp\\concat.txt -c copy \"{videoFileName}\"",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true
                    }
                };

                long sessionMergingProgressKb = 0;

                // Called everytime a new line is outputted by FFmpeg process.
                void ConcatDataReceived(object processSending, DataReceivedEventArgs errorOutput) {
                    if (backgroundWorker.CancellationPending) {
                        // If it has not already exited, sometime it's too fast so it wants to kill something that is already killed.
                        if (!concatProcess.HasExited) {
                            concatProcess.Kill();
                        } else {
                            // Deletes the video FFmpeg was currently working with & deletes the concat.txt file, then stops the worker.
                            File.Delete(videoFileName);
                            File.Delete(concatFileLocation);
                            e.Cancel = true;
                            backgroundWorker.Dispose();
                            return;
                        }
                    }

                    string line = errorOutput.Data;
                    if (line == null) { return; }
                    Match retrieveSize = ffmpegSizeFormat.Match(line);

                    if (retrieveSize.Success) {
                        sessionMergingProgressKb = long.Parse(retrieveSize.Value);
                        int sessionMergingProgress = (int)(sessionMergingProgressKb * 100 / totalSessionSizeInKb);

                        // Reports the progress & current session / total sessions for the label.
                        int globalProgressPercentage = sessionsList.IndexOf(session) * 100 / sessionsList.Count + sessionMergingProgress / sessionsList.Count;
                        backgroundWorker.ReportProgress(globalProgressPercentage, (sessionsList.IndexOf(session) + 1, sessionsList.Count));

                        // Removes the event handler if it's 100% finished, because it loses a bit of size when it's finished to the progress goes back to 99%,
                        // this prevents the progress bar from going back.
                        if (sessionMergingProgress == 100) {
                            concatProcess.ErrorDataReceived -= ConcatDataReceived;
                        }
                    }
                }

                concatProcess.ErrorDataReceived += ConcatDataReceived;

                concatProcess.Start();
                concatProcess.BeginErrorReadLine();
                concatProcess.WaitForExit();

            }

            File.Delete(concatFileLocation);
            return;
        }

        public static void GroupVideosInBackground(object sender, DoWorkEventArgs e) {
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
            List<List<string>> sessionsList = RetrieveSessions();
            if (sessionsList.Count == 0) {
                return;
            }
            backgroundWorker.ReportProgress(0, (1, sessionsList.Count));

            foreach (List<string> session in sessionsList) {

                // Stops the work if a user asked cancelation.
                if (backgroundWorker.CancellationPending) {
                    e.Cancel = true;
                    return;
                }

                // Folder name example: Recordings/Minecraft/Minecraft - 01-01-2020 12.30.00/[Videos here]
                string folderName = $"{Path.GetDirectoryName(session[0])}\\{Path.GetFileName(Path.GetDirectoryName(session[0]))} - " +
                                    $"{File.GetCreationTime(session[0]).ToString("G").Replace('/', '-').Replace(':', '.')}\\";

                // Creates the folder and moves each video of the session inside the folder.
                Directory.CreateDirectory(folderName);
                foreach (string video in session) {
                    File.Move(video, $"{folderName}\\{Path.GetFileName(video)}");
                }

                // Reports the progress & current session / total sessions for the label.
                int globalProgressPercentage = sessionsList.IndexOf(session) / sessionsList.Count * 100;
                backgroundWorker.ReportProgress(globalProgressPercentage, (sessionsList.IndexOf(session) + 1, sessionsList.Count));
            }
        }

        private static List<List<string>> RetrieveSessions() {

            List<List<string>> sessionsList = new List<List<string>>();
            string[] gamesSubfolders = Directory.GetDirectories(NvidiaRetriever.RecordsLocation);

            foreach (string gameFolder in gamesSubfolders) {
                // Only files that matches the nvidia format (not the ones that are already merged ones).
                List<string> videoList = new List<string>(Directory.GetFiles(gameFolder, "*.mp4")
                    .Where(path => nvidiaVideoFormat.IsMatch(path)));

                // If there is only 1 video it's impossible to merge.
                if (videoList.Count >= 2) {
                    // Sort the videos alphabetically, which sorts them by date due to Nvidia naming.
                    videoList.Sort();
                    List<string> videoSession = new List<string>();

                    for (int i = 1; i < videoList.Count; i++) {
                        string video1 = videoList[i - 1];
                        string video2 = videoList[i];

                        // If both videos are part of the same session only.
                        if (VideosAreSameSession(video1, video2)) {
                            videoSession.Add(video1);
                        } else if (videoSession.Count > 0) {
                            // Saves session if there are videos in the session if the session has not already been merged.
                            if (!File.Exists($"{Path.GetDirectoryName(videoSession[0])}\\{Path.GetFileName(Path.GetDirectoryName(videoSession[0]))} - " +
                               $"{File.GetCreationTime(videoSession[0]).ToString("G").Replace('/', '-').Replace(':', '.')}.mp4")) {

                                // video1 = past video2, which was the last part of the session but was not added. So add it now.
                                videoSession.Add(video1);
                                sessionsList.Add(new List<String>(videoSession));
                            }

                            videoSession.Clear();
                        }

                    }
                    // If a session was on the last iteration it has not been saved, this solves the issue, it will also be the newest session of the game folder.
                    if (videoSession.Count > 0) {
                        // Adds the missing video.
                        videoSession.Add(videoList[videoList.Count - 1]);

                        // Since it's the newest / latest session, we can only do the check here and not for every session of the folder.
                        // If the last video is more recent than (Now - video duration) or if the session has already been merged.
                        if (DateTime.Compare(File.GetCreationTime(videoSession[videoSession.Count - 1])
                            , DateTime.Now.AddSeconds(-(NvidiaRetriever.RecordsDuration + 10))) > 0
                            || File.Exists($"{Path.GetDirectoryName(videoSession[0])}\\{Path.GetFileName(Path.GetDirectoryName(videoSession[0]))} - " +
                            $"{File.GetCreationTime(videoSession[0]).ToString("G").Replace('/', '-').Replace(':', '.')}.mp4")) {

                            // Remove the session: the user might still be playing and recording, so wait in case more videos will be added to the session.
                            videoSession.Clear();
                        } else {
                            sessionsList.Add(new List<String>(videoSession));
                            videoSession.Clear();
                        }
                    }
                }
            }
            return sessionsList;
        }

        // Check if two videos are part of the same sesion.
        private static bool VideosAreSameSession(string video1, string video2) {
            DateTime video1CreationDate = File.GetCreationTime(video1).AddSeconds(NvidiaRetriever.RecordsDuration);
            DateTime video2CreationDate = File.GetCreationTime(video2);

            // If the first video + the video duration is still later than video 2 it means they're part of the same session.
            if (DateTime.Compare(video1CreationDate, video2CreationDate) > 0) {
                return true;
            } else {
                return false;
            }
        }


        public static void GroupOldVideos() {
            string[] gamesSubFolders = Directory.GetDirectories(NvidiaRetriever.RecordsLocation);

            foreach (string gameFolder in gamesSubFolders) {

                // Only videos that matches the nvidia format (not the ones that are already merged ones).
                List<string> videoList = new List<string>(Directory.GetFiles(gameFolder, "*.mp4")
                    .Where(path => nvidiaVideoFormat.IsMatch(path)));

                if (videoList.Count > 0) {
                    Directory.CreateDirectory($"{gameFolder}\\Old");
                    foreach (string video in videoList) {
                        File.Move(video, $"{gameFolder}\\Old\\{Path.GetFileName(video)}");
                    }
                }
            }
        }
    }
}
