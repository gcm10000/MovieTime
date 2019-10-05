using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Core;
using LibraryShared;

namespace MovieTimeWindowsForms
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Watch watch = new Watch();
            //watch.Title = "Homem Aranha - Longe de Casa";
            //watch.Type = Watch.TypeWatch.Movie;
            //watch.Downloads.Add(new DownloadData() { DownloadText = @"magnet:?xt=urn:btih:F564D1711F5817589A5C7E8CED467F65B9A1D93F&dn=%5bWWW.BLUDV.TV%5d%20John%20Wick%203%20-%20Parabellum%20%202019%20%281080p%20-%20BluRay%29%20Acesse%20o%20ORIGINAL%20WWW.BLUDV.TV&tr=udp%3a%2f%2ftracker.openbittorrent.com%3a80%2fannounce&tr=udp%3a%2f%2ftracker.opentrackr.org%3a1337%2fannounce&tr=udp%3a%2f%2f9.rarbg.to%3a2780%2fannounce&tr=udp%3a%2f%2fexplodie.org%3a6969%2fannounce&tr=http%3a%2f%2fglotorrents.pw%3a80%2fannounce&tr=udp%3a%2f%2fp4p.arenabg.com%3a1337%2fannounce&tr=udp%3a%2f%2ftorrent.gresille.org%3a80%2fannounce&tr=udp%3a%2f%2ftracker.aletorrenty.pl%3a2710%2fannounce&tr=udp%3a%2f%2ftracker.coppersurfer.tk%3a6969%2fannounce&tr=udp%3a%2f%2ftracker.piratepublic.com%3a1337%2fannounce" });

            Application.Run(new Player(watch));
        }
    }
}
