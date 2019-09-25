using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

//https://openlink.click/feroz/?b=d3d3LnRlY2hub2xvZ3ktdW5pdmVyc2UuY29t&url=aHR0cHM6Ly9ueWFhLnNpL2Rvd25sb2FkLzExNTU4OTYudG9ycmVudA==&user=136870344-8&type=2&vez=2&anali=
//base64 on var url
namespace Tchotchomere
{
    class Program
    {
        //API KEY THE MOVIE DB (v3 auth): 3cc7aa7a8972f7e07bba853a11fbd66f
        const string url = "https://teutorrent.com/";
        static string path = Path.Combine(Environment.CurrentDirectory, "urls");
        static string PathNewUrls = Path.Combine(path, "newurls.json");
        static string PathOldUrls = Path.Combine(path, "oldurls.json");

        static List<string> NewUrls = new List<string>();
        static List<string> OldUrls = new List<string>();


        static void Main(string[] args)
        {
            //using (WebClient webClient = new WebClient())
            //{
            //    webClient.Encoding = System.Text.Encoding.UTF8;
            //    //var url = "https://teutorrent.com/chernobyl-1o-temporada-2019-blu-ray-720p-download-torrent-dub-e-leg/";
            //    //var url = "https://teutorrent.com/ballers-4a-temporada-2018-blu-ray-720p-download-torrent-dub-e-leg/";
            //    //var url = "https://teutorrent.com/vingadores-4-ultimato-2019-torrent-hd-720p-dublado-legendado-download/";
            //    var url = "https://teutorrent.com/";
            //    string content = webClient.DownloadString(url);
            //    GetInfo(content, url);
            //}

            GetInfoOnTheMovieDB("Breaking Bad", Watch.TypeWatch.Series) ;

            CreatePaths();
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

                ReleaseData(PathNewUrls, NewUrls);
                ReleaseData(PathOldUrls, OldUrls);

            } while (NewUrls.Count > 0);
            Console.ReadKey();
        }

        static void AccessingUrl(string link)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = System.Text.Encoding.UTF8;
                string content = webClient.DownloadString(link);
                List<string> urls = LinkExtractor.ExtractUrl(content, link);
                foreach (var url in urls.ToList())
                {
                    if (!NewUrls.Contains(url) && (!OldUrls.Contains(url)))
                        NewUrls.Add(url);
                    Console.WriteLine(url);
                }
                GetInfoOnPage(content, link);
            }
        }

        static void GetInfoOnPage(string html, string url)
        {
            //using (WebClient webClient = new WebClient())
            //{
            //    webClient.Encoding = System.Text.Encoding.UTF8;
            //    //string content = webClient.DownloadString("https://teutorrent.com/vingadores-4-ultimato-2019-torrent-hd-720p-dublado-legendado-download/");
            //}
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var infoHTML = doc.DocumentNode.SelectSingleNode("//*[@class=\"content content-single\"]");
            if (infoHTML == null) return;
            //var aHTML = doc.DocumentNode.SelectSingleNode("//*[@class=\"tooltip2\"]");
            string infoText = infoHTML.InnerHtml.Replace("<br>", "\n").StripHTML();
            string[] infoTextSplited = infoText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            Watch watch = new Watch();
            DownloadData downloadData = new DownloadData();

            foreach (var info in infoTextSplited)
            {
                if (info.Contains("Baixar Filme:") || info.Contains("Baixar Série:"))
                {
                    watch.Title = info.Replace("Baixar Filme:", "").Replace("Baixar Série:", "").Trim();
                }
                else if (info.Contains("Titulo Original:"))
                {
                    watch.TitleOriginal = info.Replace("Titulo Original:", "").Trim();
                }
                else if (info.Contains("IMDb:"))
                {
                    watch.IMDb = info.Replace("IMDb:", "").Trim();
                }
                else if (info.Contains("Gênero:"))
                {
                    watch.Gender = info.Replace("Gênero:", "").Trim();

                }
                else if ((info.Contains("Ano de Lançamento:")) || (info.Contains("Lançamento:")))
                {
                    watch.Year = info.Replace("Ano de Lançamento:", "").Replace("Lançamento:", "").Trim();
                }
                else if (info.Contains("Qualidade:"))
                {
                    downloadData.Quality = info.Replace("Qualidade:", "").Trim();
                }
                else if (info.Contains("Áudio:") && (downloadData.Audio == string.Empty))
                {
                    downloadData.Audio = info.Replace("Áudio:", "").Trim();
                }
                else if (info.Contains("Legenda:"))
                {
                    watch.Subtitle = info.Replace("Legenda:", "").Trim();
                }
                else if (info.ToLower().Contains("formato:"))
                {
                    downloadData.Format = info.ToLower().Replace("formato:", "").ToUpper().Trim();
                }
                else if (info.ToLower().Contains("tamanho:"))
                {
                    downloadData.Size = info.ToLower().Replace("tamanho:", "").ToUpper().Trim();
                }
                else if (info.ToLower().Contains("duração:"))
                {
                    watch.Duration = info.ToLower().Replace("duração:", "").Trim();
                }
                else if (info.ToLower().Contains("sinopse"))
                {
                    watch.Synopsis = info.Replace("sinopse:", "").Replace("Sinopse:", "").Replace("SINOPSE:", "").Replace("SINOPSE DA SÉRIE:", "").Trim();
                }
            }

            var links = doc.DocumentNode.SelectNodes("//a");
            var linksInside = infoHTML.SelectNodes(".//a");
            if (linksInside == null)
            {
                //Series only magnet
                watch.Type = Watch.TypeWatch.Series;

                foreach (var link in links)
                {
                    if (link.Attributes["href"] != null)
                    {
                        var magnet = link.Attributes["href"].Value;
                        if (magnet.Contains("magnet:?"))
                        {
                            downloadData.DownloadText = magnet;
                            watch.Downloads.Add(downloadData);
                        }
                    }
                }
            }
            else
            {
                if (linksInside.Count == 1)
                {
                    if (url.ToLower().Contains("temporada"))
                    {
                        //It's a series!
                        watch.Type = Watch.TypeWatch.Series;
                    }
                    else
                    {
                        //It's a movie!
                        watch.Type = Watch.TypeWatch.Movie;
                    }

                    foreach (var link in links)
                    {
                        if (link.Attributes["href"] != null)
                        {
                            var magnet = link.Attributes["href"].Value;
                            if (magnet.Contains("magnet:?"))
                            {
                                downloadData.DownloadText = magnet;
                                watch.Downloads.Add(downloadData);
                            }
                        }
                    }


                }
                else if (linksInside.Count > 1)
                {
                    //It's a series!
                    watch.Type = Watch.TypeWatch.Series;
                    List<string> arraydownload = new List<string>();
                    foreach (var link in linksInside)
                    {
                        var magnet = link.Attributes["href"].Value;
                        if (magnet.Contains("magnet:?"))
                        {
                            arraydownload.Add(magnet);
                            DownloadData data = new DownloadData();
                            data = downloadData;
                            data.DownloadText = magnet;
                            watch.Downloads.Add(data);

                        }
                    }

                }
            }


            //---OUTPUT---
            Console.WriteLine("Title: " + watch.Title);
            Console.WriteLine("TitleOriginal: " + watch.TitleOriginal);
            Console.WriteLine("IMDb: " + watch.IMDb);
            Console.WriteLine("Gender: " + watch.Gender);
            Console.WriteLine("Year: " + watch.Year);
            Console.WriteLine("Subtitle: " + watch.Subtitle);
            Console.WriteLine("Duration: " + watch.Duration);
            Console.WriteLine("Synopsis: " + watch.Synopsis);
            Console.WriteLine("Information of download:");
            Console.WriteLine("Total Count: " + watch.Downloads.Count.ToString());
            int i = 1;
            foreach (var info in watch.Downloads)
            {
                Console.WriteLine(" {0}) ", i++);
                Console.WriteLine(" Quality: " + info.Quality);
                Console.WriteLine(" Audio: " + info.Audio);
                Console.WriteLine(" Format: " + info.Format);
                Console.WriteLine(" Size: " + info.Size);
                Console.WriteLine(" Magnet: " + info.DownloadText);
            }



        }

        static void GetInfoOnTheMovieDB(string Title, Watch.TypeWatch type)
        {
            Uri baseUriQuery = new Uri("https://api.themoviedb.org/3/search/");
            //https://api.themoviedb.org/3/search/movie
            //api_key=3cc7aa7a8972f7e07bba853a11fbd66f
            //&page=1&include_adult=true&query=homem%20aranha%20de%20volta%20ao%20lar&language=pt-BR
            Uri uriQuery;
            string ResultQuery;
            if (type == Watch.TypeWatch.Movie)
            {
                uriQuery = new Uri(baseUriQuery, "movie")
                    .AddQuery("api_key", "3cc7aa7a8972f7e07bba853a11fbd66f")
                    .AddQuery("page", "1")
                    .AddQuery("include_adult", "true")
                    .AddQuery("language", "pt-BR")
                    .AddQuery("query", Title);
                ResultQuery = GetResult(uriQuery);
                MovieInformationQuery info = JsonConvert.DeserializeObject<MovieInformationQuery>(ResultQuery);
                if (info.results != null)
                {
                    if (info.results.Length > 0)
                    {
                        //https://api.themoviedb.org/3/movie/{movie_id}/external_ids?api_key=<<api_key>>
                        Uri baseQueryExternalId = new Uri("https://api.themoviedb.org/3/movie/");
                        Uri uriExternalId = new Uri(baseQueryExternalId, $"{info.results[0].id}/external_ids")
                            .AddQuery("api_key", "3cc7aa7a8972f7e07bba853a11fbd66f");
                        string ResultId = GetResult(uriExternalId);
                        ExternalID.Movie infoMovie = JsonConvert.DeserializeObject<ExternalID.Movie>(ResultId);
                        //infoMovie.imdb_id
                    }
                }
            }
            else
            {
                uriQuery = new Uri(baseUriQuery, "tv")
                    .AddQuery("api_key", "3cc7aa7a8972f7e07bba853a11fbd66f")
                    .AddQuery("page", "1")
                    .AddQuery("include_adult", "true")
                    .AddQuery("language", "pt-BR")
                    .AddQuery("query", Title);
                ResultQuery = GetResult(uriQuery);
                TVInformationQuery info = JsonConvert.DeserializeObject<TVInformationQuery>(ResultQuery);
                if (info.results != null)
                {
                    if (info.results.Length > 0)
                    {
                        Uri baseQueryExternalId = new Uri("https://api.themoviedb.org/3/tv/");
                        Uri uriExternalId = new Uri(baseQueryExternalId, $"{info.results[0].id}/external_ids")
                            .AddQuery("api_key", "3cc7aa7a8972f7e07bba853a11fbd66f");
                        string ResultId = GetResult(uriExternalId);
                        ExternalID.Series infoSeries = JsonConvert.DeserializeObject<ExternalID.Series>(ResultId);
                        //infoSeries.imdb_id
                    }
                }
            }
        }

        public static string GetResult(Uri url)
        {
            string result = string.Empty;

            using (var clientAPI = new HttpClient())
            {
                ServicePointManager.Expect100Continue = true;
                //SecurityProtocolType.Tls12 missing on Framework 4.0 only
                //SecurityProtocolType.Tls12  == (SecurityProtocolType)(0xc00)
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc00);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url.ToString());
                request.KeepAlive = false;
                request.UserAgent = @"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    result = reader.ReadToEnd();

                    reader.Close();
                    dataStream.Close();
                }
            }

            return result;
        }
        static void CreatePaths()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(PathNewUrls))
            {
                File.Create(PathNewUrls);
            }
            if (!File.Exists(PathOldUrls))
            {
                File.Create(PathOldUrls);
            }
        }
        static void ReleaseData(string Path, List<string> list)
        {
            if (File.Exists(Path))
            {
                string json = JsonConvert.SerializeObject(list);
                File.WriteAllText(Path, json);
            }
            else
                CreatePaths();
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
