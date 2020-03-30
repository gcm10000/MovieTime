using System;
using System.IO;
using System.Linq;
using MovieTimeLibraryCore;
using MovieTimeLibraryCore.Context;
using TchotchomereCore.Information;

namespace TchotchomereCore
{
    class Program
    {
        const string apiKey = "3cc7aa7a8972f7e07bba853a11fbd66f";
        static string path = Path.Combine(Environment.CurrentDirectory, "urls");
        static string PathNewUrls = Path.Combine(path, "newurls.json");
        static string PathOldUrls = Path.Combine(path, "oldurls.json");
        static string PathErrorUrls = Path.Combine(path, "errorurls.json");
        static string PathMagnets = Path.Combine(path, "magnets.txt");


        static void Main(string[] args)
        {
            var botClient = new BotClient("https://www.bludv.tv/", true);
            botClient.ResultEvent += BotClient_ResultEvent; ;
            botClient.Start();
        }

        private static void BotClient_ResultEvent(ResultEventArgs Result)
        {

            if (Result.Exception != null)
            {
                Console.WriteLine(Result.Exception.Message);
                return;
            }

            Console.Title = $"Total queued: {Result.OldUrls.Count} --- To load: {Result.NewUrls.Count} --- Current address: {Result.Address}";
            GetFromPage(html: Result.ResultHtml, address: Result.Address, AllLinks: Result.TotalLinks.ToArray());
        }
        private static void GetFromPage(string html, string address, string[] AllLinks)
        {
            GetInformationFromPage info = new GetInformationFromPage();
            Watch watchBludv = info.GetInfoOnPage(html, address);
            GetInformationFromAPI infoAPI = new GetInformationFromAPI(apiKey);
            Watch watchMovieDB = infoAPI.InformationFromTheMovieDB(watchBludv);
            using (var db = new ApplicationContext())
            {
                db.Watches.Add(watchMovieDB);
            }

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
    }
}
