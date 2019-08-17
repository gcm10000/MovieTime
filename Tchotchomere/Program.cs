using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

//https://openlink.click/feroz/?b=d3d3LnRlY2hub2xvZ3ktdW5pdmVyc2UuY29t&url=aHR0cHM6Ly9ueWFhLnNpL2Rvd25sb2FkLzExNTU4OTYudG9ycmVudA==&user=136870344-8&type=2&vez=2&anali=
//base64 on var url
namespace Tchotchomere
{
    public static class Helper
    {
        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
    }
    class Program
    {
        const string url = "https://teutorrent.com/";
        
        static List<string> NewUrls = new List<string>();
        static List<string> OldUrls = new List<string>();

        static void Main(string[] args)
        {
            GetInfo("");
            NewUrls.Add(url);
            OldUrls.Add(url);
            //do while...
            //do first time, so save links on List and cycle repeat accessing all urls of website
            do
            {
                string title = string.Format("Total requested: {0}, Total Found: {1}, Url requested: {2}", OldUrls.Count, NewUrls.Count, NewUrls[0]);
                Console.WriteLine(title);
                Console.Title = title;
                AccessingUrl(NewUrls[0]);
                Console.WriteLine();
                OldUrls.Add(NewUrls[0]);
                NewUrls.Remove(NewUrls[0]);
            } while (NewUrls.Count > 1);
            Console.ReadKey();
        }

        static void AccessingUrl(string link)
        {
            using (WebClient webClient = new WebClient())
            {
                string content = webClient.DownloadString(link);
                List<string> urls = LinkExtractor.Extract(content, link);
                foreach (var url in urls.ToList())
                {
                    if (!NewUrls.Contains(url) && (!OldUrls.Contains(url)))
                        NewUrls.Add(url);
                    Console.WriteLine(url);
                }
            }
            File.WriteAllText(@"C:\Users\Gabriel\Documents\GitHub\test123.txt", string.Join(Environment.NewLine, NewUrls.ToArray()));
        }

        static void GetInfo(string html)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = System.Text.Encoding.UTF8;
                string content = webClient.DownloadString("https://teutorrent.com/vingadores-4-ultimato-2019-torrent-hd-720p-dublado-legendado-download/");

                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                var infoHTML = doc.DocumentNode.SelectNodes("//*[@class=\"content content-single\"]");
                Console.WriteLine(infoHTML[0].InnerHtml);
                string inner = infoHTML[0].InnerHtml;
                Console.WriteLine(inner.Replace("<br>", "\n").StripHTML());

            }
        }
    }
    public class LinkExtractor
    {
        /// <summary>
        /// Extracts all src and href links from a HTML string.
        /// </summary>
        /// <param name="html">The html source</param>
        /// <returns>A list of links - these will be all links including javascript ones.</returns>
        public static List<string> Extract(string html, string url)
        {
            List<string> list = new List<string>();
            Uri uriOri = new Uri(url);

            Regex regex = new Regex("<a.?(?:href)=[\"|']?(.*?)[\"|'|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);
            if (regex.IsMatch(html))
            {
                foreach (Match match in regex.Matches(html))
                {
                    Uri uri = new Uri(match.Groups[1].Value);
                    if ((uri.Host == uriOri.Host) && (!uri.LocalPath.EndsWith(".css")) && (!uri.LocalPath.EndsWith(".png")) && (!uri.Query.Contains("?amp")) && (uri.Fragment.Equals(string.Empty)))
                    {
                        list.Add(match.Groups[1].Value);
                    }
                }
            }

            return list;
        }
    }

}
