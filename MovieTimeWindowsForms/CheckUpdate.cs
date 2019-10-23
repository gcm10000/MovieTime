using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieTimeWindowsForms
{
    public partial class CheckUpdate : Form
    {
        static string temp = Path.Combine(Path.GetTempPath(), "movietime");
        string[] urls = new string[2] { "http://movietime-001-site1.itempurl.com/Download/libvlc.zip", "http://movietime-001-site1.itempurl.com/Download/peerflix.zip" };
        string[] paths = new string[2] { Path.Combine(temp, "libvlc.zip"), Path.Combine(temp, "peerflix.zip") };
        public static string file = Path.Combine(Environment.CurrentDirectory, "x");

        int currentUrl = 0;

        public CheckUpdate()
        {
            InitializeComponent();
        }
        WebClient wc;
        private void DownloadFiles()
        {
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }
            using (wc = new WebClient())
            {
                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                wc.DownloadFileAsync(new System.Uri(urls[currentUrl]), paths[currentUrl]);
            }

        }
        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            currentUrl++;
            if (currentUrl < urls.Length)
            {
                using (wc = new WebClient())
                {
                    wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                    wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                    wc.DownloadFileAsync(new System.Uri(urls[currentUrl]), paths[currentUrl]);
                    toolStripStatusLabel1.Text = $"Baixando {(currentUrl + 1).ToString()} de {urls.Length.ToString()}.";
                }
            }
            else if (currentUrl == urls.Length)
            {
                Done();
            }
        }

        private void Done()
        {
            //All done. Every zip files was downloaded.
            //Extract Files to execute
            this.Invoke(new MethodInvoker(() =>
            {
                toolStripStatusLabel1.Text = "Extraindo arquivos...";
            }));
            new Thread(() =>
            {
                ExtractFiles();
                //Create file to indicate that isn't first time open the software
                File.Create(file);
                //Open Form Default
                this.Invoke(new MethodInvoker(() =>
                {
                    this.Hide();
                    OpenDefault();
                }));
            }).Start();
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }
        private void ExtractFiles()
        {
            foreach (var path in paths)
            {
                ZipFile.ExtractToDirectory(path, Environment.CurrentDirectory);
            }
        }

        private void CheckUpdate_Load(object sender, EventArgs e)
        {
            if (!File.Exists(file))
            {
                //Make download
                DownloadFiles();
                toolStripStatusLabel1.Text = $"Baixando {(currentUrl + 1).ToString()} de {urls.Length.ToString()}.";
            }
            else
            {
                //Already update
                //Open form Default
                OpenDefault();

            }
            //if (CheckVLC())
            //{

            //}
        }

        private void OpenDefault()
        {
            this.Hide();
            var Default = new Default();
            Default.FormClosed += (s, args) => this.Close();
            Default.Show();
        }
    }
}
