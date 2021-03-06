﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using MovieTimeLibraryCore;
using MovieTimeLibraryCore.Context;
using Newtonsoft.Json;
using TchotchomereCore.Information;

namespace TchotchomereCore
{
    class Program
    {
        const string apiKey = "3cc7aa7a8972f7e07bba853a11fbd66f";
        static byte[] cryptographyKey = Encoding.UTF8.GetBytes("qwertyas23dfzxc5vqMerasd25f4b1");
        static string path = Path.Combine(Environment.CurrentDirectory, "urls");
        static string PathNewUrls = Path.Combine(path, "newurls.json");
        static string PathOldUrls = Path.Combine(path, "oldurls.json");
        static string PathErrorUrls = Path.Combine(path, "errorurls.json");
        static string PathMagnets = Path.Combine(path, "magnets.txt");
        static string url = "https://www.bludv.tv/";
        static string ltT = Path.Combine(Environment.CurrentDirectory, "ltT.ejson");

        static void Main(string[] args)
        {
            var botClient = new BotClient(url, true);
            botClient.ResultEvent += BotClient_ResultEvent;
            botClient.Start();

            Console.ReadLine();
        }
        private static void BotClient_ResultEvent(ResultEventArgs Result)
        {
            if (Result.Exception != null)
            {
                Console.WriteLine(Result.Exception.Message);
                return;
            }
            Console.Title = $"Total queued: {Result.OldUrls.Count} --- To load: {Result.NewUrls.Count} --- Current address: {Result.Address}";
            
            var HasTorrent = Result.TotalLinks.Any(x => new Uri(x).Scheme == "magnet");
            if (HasTorrent)
                GetFromPage(html: Result.ResultHtml, address: Result.Address, AllLinks: Result.TotalLinks.ToArray());
        }
        private static void GetFromPage(string html, string address, string[] AllLinks)
        {
            try
            {

            
            GetInformationFromPage info = new GetInformationFromPage();
            Watch watchBludv = info.GetInfoFromBludv(html, address, "content");
            GetInformationFromAPI infoAPI = new GetInformationFromAPI(apiKey);
            Watch watch = infoAPI.InformationFromTheMovieDB(watchBludv);
            //using (var db = new ApplicationContext())
            //{
            //    db.Watches.Add(watchMovieDB);
            //}

            //---OUTPUT---
            WatchToFile(watch);
            Console.WriteLine($"Informações de {address}:");
            Console.WriteLine("Título: " + watch.Title);
            Console.WriteLine();
            }
            catch (Exception ex)
            {
                var now = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                using (StreamWriter stream = new StreamWriter(Path.Combine(Environment.CurrentDirectory, $"log{now}.txt")))
                {
                    stream.Write($"{now}: {address}: {ex.Message}\n");
                    stream.Flush();
                }
            }
            //Console.WriteLine("Título Original: " + watch.TitleOriginal);
            //Console.WriteLine("Duração: " + watch.Duration);
            //int e = 1;
            //Console.WriteLine("Informações da legenda:");
            //Console.WriteLine("Total: " + watch.Subtitles.Count.ToString());
            //foreach (var subtitle in watch.Subtitles)
            //{
            //    Console.WriteLine(" {0}) ", e++);
            //    Console.WriteLine("Idioma: " + subtitle.Lang);
            //    Console.WriteLine("Download: " + subtitle.DownloadText);
            //}
            //Console.WriteLine("Informações do download:");
            //Console.WriteLine("Total: " + watch.Downloads.Count.ToString());
            //int i = 1;
            //foreach (var aInfo in watch.Downloads)
            //{
            //    Console.WriteLine(" {0}) ", i++);
            //    Console.WriteLine(" Qualidade: " + aInfo.Quality);
            //    Console.WriteLine(" Áudio: " + aInfo.Audio);
            //    Console.WriteLine(" Formato: " + aInfo.Format);
            //    Console.WriteLine(" Tamanho: " + aInfo.Size);
            //    Console.WriteLine(" Magnet: " + aInfo.Magnet);
            //}
            //var listMagnet = AllLinks.Where(x => new Uri(x).Scheme == "magnet");
            //Console.WriteLine(address);
            //foreach (var item in listMagnet)
            //{
            //    Console.WriteLine(item);
            //    using (StreamWriter writer = new StreamWriter(PathMagnets, true))
            //    {
            //        writer.WriteLine(item + Environment.NewLine);
            //    }
            //}
        }
        private static void WatchToFile(Watch watch)
        {
            if (!File.Exists(ltT))
            {
                var list = new List<Watch>();
                list.Add(watch);
                OutputWatches(list);
            }
            else
            {
                var list = InputWatches();
                list.Add(watch);
                OutputWatches(list);
            }
        }

        private static List<Watch> InputWatches()
        {
            using (StreamReader streamReader = new StreamReader(ltT, System.Text.Encoding.UTF8))
            {
                var encodedBase64ListJson = Encoding.UTF8.GetBytes(streamReader.ReadToEnd());
                byte[] decodeBase64 = System.Convert.FromBase64String(Encoding.UTF8.GetString(encodedBase64ListJson));
                return JsonConvert.DeserializeObject<List<Watch>>(Encoding.UTF8.GetString(decodeBase64));
            }
        }
        private static void OutputWatches(List<Watch> watches)
        {
            using (StreamWriter streamWriter = new StreamWriter(ltT, false, System.Text.Encoding.UTF8))
            {
                var listWatch = JsonConvert.SerializeObject(watches);
                var encodeBase64 = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(listWatch));
                streamWriter.Write(encodeBase64, Encoding.UTF8);
            }
        }
    }
}
