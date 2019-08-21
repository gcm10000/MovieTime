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
                List<string> urls = LinkExtractor.ExtractUrl(content, link);
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
                var aHTML = doc.DocumentNode.SelectSingleNode("//*[@class=\"tooltip2\"]");
                string infoText = infoHTML[0].InnerHtml.Replace("<br>", "\n").StripHTML();
                string[] infoTextSplited = infoText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                Movie movie = new Movie();
                foreach (var info in infoTextSplited)
                {
                    if (info.Contains("Baixar Filme:"))
                    {
                        movie.Title = info.Replace("Baixar Filme:", "").Trim();
                    }
                    else if (info.Contains("Titulo Original:"))
                    {
                        movie.TitleOriginal = info.Replace("Titulo Original:", "").Trim();
                    }
                    else if (info.Contains("IMDb:"))
                    {
                        movie.IMDb = info.Replace("IMDb:", "").Trim();
                    }
                    else if (info.Contains("Gênero:"))
                    {
                        movie.Gender = info.Replace("Gênero:", "").Trim();

                    }
                    else if (info.Contains("Ano de Lançamento:"))
                    {
                        movie.Year = info.Replace("Ano de Lançamento:", "").Trim();
                    }
                    else if (info.Contains("Qualidade:"))
                    {
                        movie.Quality = info.Replace("Qualidade:", "").Trim();
                    }
                    else if (info.Contains("Áudio:") && (movie.Audio == null))
                    {
                        movie.Audio = info.Replace("Áudio:", "").Trim();
                    }
                    else if (info.Contains("Legenda:"))
                    {
                        movie.Subtitle = info.Replace("Legenda:", "").Trim();
                    }
                    else if (info.Contains("Formato:"))
                    {
                        movie.Format = info.Replace("Formato:", "").Trim();
                    }
                    else if (info.Contains("Tamanho:"))
                    {
                        movie.Size = info.Replace("Tamanho:", "").Trim();
                    }
                    else if (info.Contains("Duração:"))
                    {
                        movie.Duration = info.Replace("Duração:", "").Trim();
                    }
                    else if (info.Contains("Sinopse:"))
                    {
                        movie.Synopsis = info.Replace("Sinopse:", "").Trim();
                    }
                }

                var magnetTorrent = LinkExtractor.ExtractMagnet(content);
                if (magnetTorrent.Count > 0)
                {
                    movie.Download = magnetTorrent[0];
                }

                Console.WriteLine("Title: " + movie.Title);
                Console.WriteLine("TitleOriginal: " + movie.TitleOriginal);
                Console.WriteLine("IMDb: " + movie.IMDb);
                Console.WriteLine("Gender: " + movie.Gender);
                Console.WriteLine("Year: " + movie.Year);
                Console.WriteLine("Quality: " + movie.Quality);
                Console.WriteLine("Audio: " + movie.Audio);
                Console.WriteLine("Subtitle: " + movie.Subtitle);
                Console.WriteLine("Format: " + movie.Format);
                Console.WriteLine("Size: " + movie.Size);
                Console.WriteLine("Duration: " + movie.Duration);
                Console.WriteLine("Synopsis: " + movie.Synopsis);
                Console.WriteLine("Download: " + movie.Download);
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
        public static List<string> ExtractMagnet(string html)
        {
            List<string> list = new List<string>();

            Regex regex = new Regex("<a.?(?:href)=[\"|']?(.*?)[\"|'|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);
            if (regex.IsMatch(html))
            {
                foreach (Match match in regex.Matches(html))
                {
                    Uri uri = new Uri(match.Groups[1].Value);
                    if (match.Groups[1].Value.Contains("magnet:?"))
                    {
                        list.Add(match.Groups[1].Value);
                    }
                }
            }

            return list;
        }

        public static List<string> ExtractUrl(string html, string url)
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
