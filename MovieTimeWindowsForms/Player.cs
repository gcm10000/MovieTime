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
        bool playFinished;
        readonly string peerflix = Path.Combine(Environment.CurrentDirectory, "peerflix.exe");
        bool isSetMedia;
        public Player(Watch watch)
        {
            //this.vlcControl.VlcLibDirectory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64")); ;
            //this.vlcControl.VlcMediaplayerOptions = new[] { "-vv" };

            this.Text = watch.Title + " - MovieTime";

            InitializeComponent();
            //vlcControl.SetMedia(<MEDIA>), new string[] { });

            //Check movie or TV series
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
            //Configuration to process async
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
                vlcControl.SetMedia(new Uri(url), new string[] { });
                isSetMedia = true;
                Play();
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
                timer.Stop();
            };

            vlcControl.EndReached += (sender, e) => {
                playFinished = true;
                timer.Stop();
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
        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (!playFinished)
            {
                vlcControl.Stop();
                timer.Stop();
                timer.Enabled = false;
            }
        }
        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (vlcControl.IsPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }
        private void Play()
        {
            timer.Enabled = true;
            timer.Start();
            vlcControl.Play();
        }
        private void Pause()
        {
            vlcControl.Pause();
            timer.Stop();
            timer.Enabled = false;
        }
        private void SeekBar_Scroll(object sender, EventArgs e)
        {
            //seekBar.Value
            //if (seekBar.Value % seekBar.SmallChange != 0)
            vlcControl.Position = seekBar.Value / 100;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (vlcControl.Position == -1)
            {
                seekBar.Value = 0;
            }
            else
            {
                seekBar.Value = (int)Math.Ceiling(vlcControl.Position * 100);
            }
        }
    }
}
