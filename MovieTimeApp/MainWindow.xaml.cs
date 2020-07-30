using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
            var vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            Client client = new Client();
            //client.Receive("ReceiveMessage", ReceiveData);

            var options = new string[]
            {
                // VLC options can be given here. Please refer to the VLC command line documentation.
            };

            this.myVideoControl.SourceProvider.CreatePlayer(vlcLibDirectory, options);

            // Load libvlc libraries and initializes stuff. It is important that the options (if you want to pass any) and lib directory are given before calling this method.
            this.myVideoControl.SourceProvider.MediaPlayer.Play("https://www.w3schools.com/html/mov_bbb.mp4");
            //client.Send("SendMessage", "teste", "alo");

        }
        public void ReceiveData(string window, string message)
        {
            MessageBox.Show($"window: {window}\nmessage: {message}");
        }
    }
}
