using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovieTimeApp
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {

        // https://github.com/videojs/video.js/issues/5910
        //  A RESPEITO DA LEITURA DE MKV
        // you can sometimes get some MKVs to play in browsers that support webm, because webm is a subset of MKV

        //MOVIETIME - SOCKET CLIENT - WEBASSEMBLY (SOCKET SERVER) - WEBSOCKET - BROWSER EMBEDDED (INTERNET EXPLORER)
        private RealTimeClient realTimeClient;
        private const int socketPort = 5010;
        private const int webSocketPort = 5000;
        public MainWindow()
        {
            Helper.SetLastVersionIE();
            OpenBridge(socketPort, webSocketPort);

            InitializeComponent();
            var vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            realTimeClient = new RealTimeClient(ReceiveData, new IPEndPoint(IPAddress.Loopback, socketPort));


            //var options = new string[]
            //{
            //    // VLC options can be given here. Please refer to the VLC command line documentation.
            //};

            //this.myVideoControl.SourceProvider.CreatePlayer(vlcLibDirectory, options);

            //// Load libvlc libraries and initializes stuff. It is important that the options (if you want to pass any) and lib directory are given before calling this method.
            //this.myVideoControl.SourceProvider.MediaPlayer.Play("https://www.w3schools.com/html/mov_bbb.mp4");
            this.webBrowser.Navigate("https://www.google.com/");
            //this.webBrowser = this.webBrowser2 = this.webBrowser3;
        }
        public void ReceiveData(string MethodName, string Section, string Body, bool AllData)
        {
            MessageBox.Show($"section: {Section}\nmessage: {Body}\nalldata:{AllData}");
        }
        public void OpenBridge(int socketPort, int webSocketPort)
        {
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "bridge", "movietimebridge.exe");
            //StartProcess(path, $"{socketPort} {webSocketPort}");
            StartProcess(path, $"{socketPort} {webSocketPort}", (o, outLine) => 
            {
                if (!String.IsNullOrEmpty(outLine.Data))
                {
                    Console.WriteLine("Info Bridge: {0}", outLine.Data);
                    // Add the text to the collected output.
                    if (outLine.Data.ToLower().Contains("server ready"))
                    {
                        realTimeClient.Connect();
                        string body = "teste teste2 teste3";
                        realTimeClient.Send("SendMessage", "Action", body);
                    }
                }
            });
        }
        public void OpenPeerflix()
        {
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "peerflix", "peerflix.exe");
        }
        public void StartProcess(string FileName, string Arguments, Action<object, DataReceivedEventArgs> Data)
        {
            // Initialize the process and its StartInfo properties.
            // The sort command is a console application that
            // reads and sorts text input.

            Process sortProcess = new Process();
            sortProcess.StartInfo.FileName = FileName;
            sortProcess.StartInfo.Arguments = Arguments;

            // Set UseShellExecute to false for redirection.
            sortProcess.StartInfo.UseShellExecute = false;

            // Hide window
            sortProcess.StartInfo.CreateNoWindow = true;

            // Redirect the standard output of the sort command.
            // This stream is read asynchronously using an event handler.
            sortProcess.StartInfo.RedirectStandardOutput = true;

            // Set our event handler to asynchronously read the sort output.
            sortProcess.OutputDataReceived += (o, ea) => Data(o, ea);

            // Redirect standard input as well.  This stream
            // is used synchronously.
            sortProcess.StartInfo.RedirectStandardInput = true;

            // Start the process.
            sortProcess.Start();
            try
            {
                AppDomain.CurrentDomain.ProcessExit += (a, b) => sortProcess.Kill();
            }
            catch (System.InvalidOperationException) //Program was previously killed.
            {}
            // Start the asynchronous read of the sort output stream.
            sortProcess.BeginOutputReadLine();
        }
    }
}
