using System;
using System.Collections.Generic;
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
        //MOVIETIME - SOCKET CLIENT - WEBASSEMBLY (SOCKET SERVER) - WEBSOCKET - BROWSER EMBEDDED (INTERNET EXPLORER)
        ClientSocket client;
        public MainWindow()
        {
            InitializeComponent();
            var vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            RealTimeClient realTimeClient = new RealTimeClient(ReceiveData, new IPEndPoint(IPAddress.Loopback, 5010));
            realTimeClient.Connect();
            string body = "teste teste2 teste3";
            // Send test data to the remote device.  
            client.Send("SET SendMessage" + Environment.NewLine);
            client.Send("Section: ActionMenu" + Environment.NewLine);
            client.Send("Content-Length: " + (body.Length * 2).ToString() + Environment.NewLine);
            client.Send(Environment.NewLine);
            client.Send(body);
            Thread.Sleep(500);
            client.Send(body);

            //var options = new string[]
            //{
            //    // VLC options can be given here. Please refer to the VLC command line documentation.
            //};

            //this.myVideoControl.SourceProvider.CreatePlayer(vlcLibDirectory, options);

            //// Load libvlc libraries and initializes stuff. It is important that the options (if you want to pass any) and lib directory are given before calling this method.
            //this.myVideoControl.SourceProvider.MediaPlayer.Play("https://www.w3schools.com/html/mov_bbb.mp4");
            this.webBrowser1.Navigate("https://www.google.com/");
            this.webBrowser1 = this.webBrowser2 = this.webBrowser3;
        }
        public void ReceiveData(string MethodName, string Section, string Body, bool AllData)
        {
            MessageBox.Show($"window: {Section}\nmessage: {Body}");
        }
    }
}
