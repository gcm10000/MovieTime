using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LibraryShared;

namespace MovieTimeWindowsForms
{
    public partial class Player : Form
    {
        Watch watch;
        bool playFinished;
        string peerflix = Path.Combine(Environment.CurrentDirectory, "peerflix.exe");
        string url;
        bool isSetMedia;
        public Player(Watch watch)
        {
            this.watch = watch;
            this.Text = watch.Title + " - MovieTime";

            InitializeComponent();

            if (watch.Type == Watch.TypeWatch.Movie)
            {
                StartClientTorrent(watch.Downloads[0].DownloadText);
            }
            else
            {
                //Foreach...
            }
        }
        private void StartClientTorrent(string arguments)
        {
            Process process = new Process();
            var startInfo = new ProcessStartInfo(fileName: peerflix, arguments: "\"" + arguments + "\"");
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            process.StartInfo = startInfo;
            process.OutputDataReceived += Process_DataReceived;
            process.ErrorDataReceived += Process_DataReceived;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

        private void Process_DataReceived(object sender, DataReceivedEventArgs e)
        {
            String result = Regex.Replace(e.Data, @"\u001b\[[^m]*m", String.Empty);
            Console.WriteLine(result);

            if ((result.Contains("open vlc")) && (!isSetMedia))
            {
                //open vlc and enter http://192.168.1.4:8888/ as the network address
                String url = result.Replace("open vlc and enter ", "").Replace(" as the network address", "").Trim();
                this.url = url;
                vlcControl.SetMedia(new Uri(url), new string[] { });
                isSetMedia = true;
                vlcControl.Play();
            }

        }
        private void StartPlayerForm(string url)
        {
            var mediaOptions = new string[] {};
            this.vlcControl.SetMedia(new Uri(url), mediaOptions);
            playFinished = false;
            vlcControl.PositionChanged += (sender, e) =>
            {
                Console.Write("\r" + Math.Floor(e.NewPosition * 100) + "%");
            };

            vlcControl.EncounteredError += (sender, e) =>
            {
                Console.Error.Write("An error occurred");
                playFinished = true;
            };

            vlcControl.EndReached += (sender, e) => {
                playFinished = true;
            };

            vlcControl.Playing += (sender, e) =>
            {
                //var media = vlcControl1.GetCurrentMedia();
                //media.Parse();

                Console.WriteLine(this.vlcControl.Audio.Tracks.Current.ID);
            };
            this.vlcControl.Play();
            //Console.WriteLine("{0}) {1}.", this.vlcControl1.Audio.Tracks.Current.ID, this.vlcControl1.Audio.Tracks.Current.Name);

        }
    }
}
