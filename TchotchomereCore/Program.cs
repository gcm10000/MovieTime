using System;
using System.IO;
using System.Linq;
using MovieTimeLibraryCore;
using MovieTimeLibraryCore.Context;

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

            if (Result.Address.ToLower().Contains("temporada"))
            {

            }

            var listMagnet = Result.TotalLinks.Where(x => new Uri(x).Scheme == "magnet");
            Console.WriteLine(Result.Address);
            foreach (var item in listMagnet)
            {
                Console.WriteLine(item);
                using (StreamWriter writer = new StreamWriter(PathMagnets, true))
                {
                    writer.WriteLine(item + Environment.NewLine);
                }
            }
        }
    }
}
