using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Xabe.FFmpeg;


namespace Nvidia_Grouping_Tool {
    public static class VideoMerging {

        /*
            FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);
         
            long totalSizeInKb = 0;
            foreach (string video in videos) {
                long sizeInKb = new FileInfo(video).Length / 1024;
                Console.WriteLine(video + " - " + sizeInKb);
                totalSizeInKb += sizeInKb;
            }

            StreamWriter file = new StreamWriter(@"E:\Dev\C#\Nvidia Grouping Tool\Test concat\test.txt");
            foreach (var video in videos) {
                Console.WriteLine("test");
                file.WriteLine("file '" + video + "'");
            }
            file.Close();

       
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C ffmpeg -f concat -safe 0 -i \"E:\\Dev\\C#\\Nvidia Grouping Tool\\Test concat\\test.txt\" -c copy \"E:\\Dev\\C#\\Nvidia Grouping Tool\\Test concat\\output.mp4\"";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
                
            */
        static MainWindow mainForm = Application.OpenForms.OfType<MainWindow>().FirstOrDefault();

        private static void mergeVideos(object sender, DoWorkEventArgs e) {

            List<List<String>> sessionList = (List<List<string>>) e.Argument;


            mainForm.groupNowButton.Enabled = false;
            foreach (List<String> session in sessionList) {
                Xabe.FFmpeg.Downloader.FFmpegDownloader.GetLatestVersion(Xabe.FFmpeg.Downloader.FFmpegVersion.Official);

                mainForm.mergeProgressLabel.Text = $"Grouping ... ({sessionList.IndexOf(session) + 1}/{sessionList.Count})";
                mainForm.mergeProgressBar.Maximum = sessionList.Count * 100;
                mainForm.mergeProgressBar.Minimum = 0;
                mainForm.mergeProgressBar.Value = sessionList.IndexOf(session) * 100;


                long totalSizeInKb = 0;
                StreamWriter file = new StreamWriter($@"{Path.GetDirectoryName(session[0])}\concat.txt");

                foreach (string video in session) {
                    long sizeInKb = new FileInfo(video).Length / 1024;
                    Console.WriteLine(video + " - " + sizeInKb);
                    totalSizeInKb += sizeInKb;
                    file.WriteLine($"file '{video}'");
                }
                file.Close();

                string videoName = Path.GetDirectoryName(session[0]) + @"\" + Path.GetFileName(Path.GetDirectoryName(session[0])) + " - " + File.GetCreationTime(session[0]).ToString("g").Replace('/', '-').Replace(':', '.') + ".mp4";

                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/C ressources\\ffmpeg -f concat -safe 0 -i \"{ Path.GetDirectoryName(session[0])}\\concat.txt\" -c copy \"{videoName}\"";
                process.StartInfo.CreateNoWindow = false;//METTRE TRUE APRES, RETIRE LA CONSOLE QUAND ON CLIQUE
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                //process.StartInfo.RedirectStandardOutput = true;

                process.Start();

                string processOutput = null;
                string sizePattern = @"(?!size=\s*)\d+(?=(?i)kb)";
                long currentProgressKb = 0;
                Regex sizeRg = new Regex(sizePattern);
                while ((processOutput = process.StandardError.ReadLine()) != null) {
                    Match match = sizeRg.Match(processOutput);
                    if (match.Success) {
                        currentProgressKb = long.Parse(match.Value);
                        int result = (int) (currentProgressKb * 100 / totalSizeInKb);
                        if (result == 100) {
                            mainForm.mergeProgressBar.Value = ((sessionList.IndexOf(session)) * 100 + result);
                            break; 
                        }
                        Console.WriteLine(currentProgressKb + " x100  / " + totalSizeInKb + " = " + result);
                        mainForm.mergeProgressBar.Value = ((sessionList.IndexOf(session))*100 + result);
                    } else {
                        Console.WriteLine("No match: " + processOutput);
                    }

                }

                process.WaitForExit();
                process.Close();
                Console.WriteLine("Process exited");
                if(File.Exists($@"{Path.GetDirectoryName(session[0])}\concat.txt")) {
                    File.Delete($@"{Path.GetDirectoryName(session[0])}\concat.txt");
                }

            }

            
        }


        public static void MergeVideosSessions(List<List<String>> sessionList) {

            List<List<String>> sessionListClone = new List<List<String>>(sessionList);
            foreach (List<String> session in sessionListClone) {
                string videoName = Path.GetDirectoryName(session[0]) + @"\" + Path.GetFileName(Path.GetDirectoryName(session[0])) + " - " + File.GetCreationTime(session[0]).ToString("g").Replace('/', '-').Replace(':', '.') + ".mp4";
                if(File.Exists(videoName)) {
                    Console.WriteLine("contains " + videoName);
                    sessionList.Remove(session);
                }
            }

            if (sessionList.Count == 0) {
                mainForm.mergeProgressBar.Value = 0;
                mainForm.mergeProgressLabel.Text = "No sessions found";
                return;
            }


            if (Properties.Settings.Default.folder_instead_of_merging == true) {
                foreach (List<String> session in sessionList) {
                    mainForm.mergeProgressLabel.Text = $"Grouping ... ({sessionList.IndexOf(session) + 1}/{sessionList.Count})";
                    mainForm.mergeProgressBar.Maximum = sessionList.Count;
                    mainForm.mergeProgressBar.Minimum = 0;
                    mainForm.mergeProgressBar.Value = sessionList.IndexOf(session) + 1;
                    string folderName = Path.GetDirectoryName(session[0]) + @"\" + Path.GetFileName(Path.GetDirectoryName(session[0])) + " - " + File.GetCreationTime(session[0]).ToString("g").Replace('/', '-').Replace(':', '.') + @"\";
                    Console.WriteLine(folderName);
                    Directory.CreateDirectory(Path.GetDirectoryName(folderName));
                    foreach (string video in session) {
                        System.IO.File.Move(video, (folderName + @"\" + Path.GetFileName(video)));
                    }
                }
            } else {
                BackgroundWorker bgWorker = new BackgroundWorker();
                //bgWorker.WorkerReportsProgress = true;
                //bgWorker.WorkerSupportsCancellation = true;

                bgWorker.DoWork += mergeVideos;
                bgWorker.RunWorkerAsync(argument:sessionList);
                bgWorker.RunWorkerCompleted += changeProgressLabel;
                
                mainForm.groupNowButton.Enabled = true;
                foreach (var session in sessionList) {
                    Console.WriteLine("======================");
                    foreach (var video in session) {
                        Console.WriteLine(video);

                    }

                }

            }

        }

        private static void changeProgressLabel(object sender, RunWorkerCompletedEventArgs e) {
            if(e.Error != null) {
                mainForm.mergeProgressLabel.Text = "An error occured";
                MessageBox.Show("Error while merging (please report): \n" + e.Error.Message);
            } else if (e.Cancelled) {
                mainForm.mergeProgressLabel.Text = "Grouping canceled";
            } else {
                mainForm.mergeProgressLabel.Text = "Grouping completed";
            }
            mainForm.groupNowButton.Enabled = true;
            
        }


        public static List<List<String>> GetSessions(string path, int recordDuration) {


            List<List<String>> sessionList = new List<List<String>>();
            
            Regex nvidiaFormatRegex = new Regex(@".*\d{4}.\d{2}.\d{2} - \d{2}.\d{2}.\d{2}.\d{2}.DVR.mp4");
            string[] gamesFolder = GetSubDirectories(path);
            List<String> singleSession = new List<String>();

            foreach (string folder in gamesFolder) {
                List<String> videoList =  new List<String>(Directory.GetFiles(folder, "*.mp4"));
                singleSession.Clear();

                if (videoList.Count >= 2) { 

                    videoList.Sort();
                    string[] tempCompare = new string[2];
                    
                    foreach(string video in new List<String>(videoList)) {
                        if(!nvidiaFormatRegex.IsMatch(video)) {
                            Console.WriteLine(video + " is not from nvidia");
                            videoList.Remove(video);
                        }
                        if(DateTime.Compare(File.GetCreationTime(video), DateTime.Now.AddSeconds(-(recordDuration + 10))) >= 0) {
                            Console.WriteLine(video + " is too recent");
                            videoList.Remove(video);
                        }
                    }

                    if (videoList.Count < 2) { continue; }
                    for(int i = 0; i < videoList.Count; i++) {
                        string video = videoList[i];
                        
                        if(i == 0) {
                            tempCompare[0] = tempCompare[1] = video;
                            continue;
                        }

                        tempCompare[0] = tempCompare[1];
                        tempCompare[1] = video;

                        if(isSameSession(tempCompare[0], tempCompare[1], recordDuration)) {
                            if (!singleSession.Contains(tempCompare[0])) {
                                singleSession.Add(tempCompare[0]);
                            }
                            if (!singleSession.Contains(tempCompare[1])) {
                                singleSession.Add(tempCompare[1]);
                            }
                        } else {
                            if(singleSession.Count > 0) {
                                sessionList.Add(new List<String>(singleSession));
                                singleSession.Clear();
                            }
                        }

                    }
                }



            }

            return sessionList;
        }

        private static bool isSameSession(string video1, string video2, int recordDuration) {

            DateTime lastWritten1 = File.GetCreationTime(video1);   
            DateTime lastWritten2 = File.GetCreationTime(video2);  

            lastWritten1 = lastWritten1.AddSeconds(recordDuration); 

            if (lastWritten1.CompareTo(lastWritten2) > 0) {
                return true;
            } else {
                return false;
            }
        }

        private static string[] GetSubDirectories(string path) {
            string[] directories;
            try {
                directories = (Directory.GetDirectories(path));
            } catch (Exception excep) {
                directories = new string [] { "Null" };
                MessageBox.Show("Please report the issue on github: \n" + excep.ToString());
            }
            return directories;
        }

    }

}
