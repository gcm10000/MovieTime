using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

            //int x = Helper.LevenshteinDistance.Compute("Annabelle 3: De Volta", "Annabelle 3: De Volta Para Casa");

            Watch watch = new Watch();
            watch.Title = "Breaking Bad";
            watch.Type = Watch.TypeWatch.Series;

            GetInfoOnTheMovieDB(watch);

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
            try
            {


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
                    else if (info.Contains("Qualidade:"))
                    {
                        downloadData.Quality = info.Replace("Qualidade:", "").Trim();
                    }
                    else if (info.Contains("Áudio:") && (downloadData.Audio == string.Empty))
                    {
                        downloadData.Audio = info.Replace("Áudio:", "").Trim();
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
                }

                var links = doc.DocumentNode.SelectNodes("//a");
                var linksInside = infoHTML.SelectNodes(".//a");

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
                if (linksInside == null)
                {
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

                //Get Info On TheMovieDB
                watch = GetInfoOnTheMovieDB(watch);

                //---OUTPUT---
                Console.WriteLine("Information from TeuTorrent:");
                Console.WriteLine("Title: " + watch.Title);
                Console.WriteLine("TitleOriginal: " + watch.TitleOriginal);
                Console.WriteLine("Duration: " + watch.Duration);
                int e = 1;
                Console.WriteLine("Information of Subtitle:");
                Console.WriteLine("Total Count: " + watch.Subtitles.Count.ToString());
                foreach (var subtitle in watch.Subtitles)
                {
                    Console.WriteLine(" {0}) ", e++);
                    Console.WriteLine("Language: " + subtitle.Lang);
                    Console.WriteLine("Download: " + subtitle.DownloadText);
                }
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

                //Insert DB here
                Commands cmd = new Commands();
                int idWatch = cmd.InsertWatch(watch);

                List<int> idDownloads = new List<int>();
                foreach (var download in watch.Downloads)
                {
                    Commands command = new Commands();
                    int idDownload = command.InsertDownload(download);
                    command.InsertWatchDownload(idWatch, idDownload);
                }
                List<int> idSubtitles = new List<int>();
                if (watch.Subtitles.Count > 0)
                {
                    foreach (var subtitle in watch.Subtitles)
                    {
                        Commands command = new Commands();
                        int idSubtitle = command.InsertSubtitle(subtitle);
                        command.InsertWatchSubtitle(idWatch, idSubtitle);
                    }
                }

            }
            catch (Exception)
            {

            }

        }

        static Watch GetInfoOnTheMovieDB(Watch watch)
        {
            Uri baseUriQuery = new Uri("https://api.themoviedb.org/3/search/");
            //https://api.themoviedb.org/3/search/movie
            //api_key=3cc7aa7a8972f7e07bba853a11fbd66f
            //&page=1&include_adult=true&query=homem%20aranha%20de%20volta%20ao%20lar&language=pt-BR
            Uri uriQuery;
            string ResultQuery;
            if (watch.Type == Watch.TypeWatch.Movie)
            {
                uriQuery = new Uri(baseUriQuery, "movie")
                    .AddQuery("api_key", "3cc7aa7a8972f7e07bba853a11fbd66f")
                    .AddQuery("page", "1")
                    .AddQuery("include_adult", "true")
                    .AddQuery("language", "pt-BR");
                var strQuery = (watch.Title != string.Empty) ? uriQuery.ToString() + $"&query={Uri.EscapeUriString(watch.Title)}" : uriQuery.ToString() + $"&query={Uri.EscapeUriString(watch.TitleOriginal)}";
                ResultQuery = GetResult(new Uri(strQuery));
                MovieInformationQuery ResultsFromQuery = JsonConvert.DeserializeObject<MovieInformationQuery>(ResultQuery);
                if (ResultsFromQuery.results != null)
                {
                    if (ResultsFromQuery.results.Length > 0)
                    {
                        List<int> numbersComputed = new List<int>();

                        foreach (var info in ResultsFromQuery.results)
                        {
                            if (watch.Title != string.Empty)
                            {
                                numbersComputed.Add(Helper.LevenshteinDistance.Compute(watch.Title, info.title));
                            }
                            else if (watch.TitleOriginal != string.Empty)
                            {
                                numbersComputed.Add(Helper.LevenshteinDistance.Compute(watch.TitleOriginal, info.original_title));
                            }
                        }

                        //return int[0] = index
                        //return int[1] = value
                        int x = GetMinNumber(numbersComputed.ToArray())[0];
                        var Info = ResultsFromQuery.results[x];
                        Uri baseQueryExternalId = new Uri("https://api.themoviedb.org/3/movie/");
                        Uri uriExternalId = new Uri(baseQueryExternalId, $"{ResultsFromQuery.results[0].id}/external_ids")
                            .AddQuery("api_key", "3cc7aa7a8972f7e07bba853a11fbd66f");
                        string ResultId = GetResult(uriExternalId);

                        ExternalID.Movie infoMovie = JsonConvert.DeserializeObject<ExternalID.Movie>(ResultId);
                        watch.IDTheMovieDB = infoMovie.id;
                        watch.IDIMDb = infoMovie.imdb_id;

                        var genres = new List<string>();
                        foreach (var genre in Info.genre_ids)
                        {
                            genres.Add(Genre.MovieWatch[genre]);
                        }
                        //watch.Title = (watch.Title == string.Empty) ? info.title : watch.Title; -> more cycle 
                        watch.Title = Info.title;
                        //watch.TitleOriginal = (watch.TitleOriginal == string.Empty) ? info.original_title : watch.TitleOriginal;
                        watch.TitleOriginal = Info.original_title;
                        watch.Synopsis = Info.overview;
                        watch.Genre = genres.ToArray();
                        watch.BackdropPicture = Info.backdrop_path;
                        watch.PosterPicture = Info.poster_path;
                        watch.Date = Info.release_date;
                        return watch;
                    }
                }
            }
            else
            {
                uriQuery = new Uri(baseUriQuery, "tv")
                    .AddQuery("api_key", "3cc7aa7a8972f7e07bba853a11fbd66f")
                    .AddQuery("page", "1")
                    .AddQuery("include_adult", "true")
                    .AddQuery("language", "pt-BR");
                var strQuery = (watch.Title != string.Empty) ? uriQuery.ToString() + $"&query={Uri.EscapeUriString(watch.Title)}" : uriQuery.ToString() + $"&query={Uri.EscapeUriString(watch.TitleOriginal)}";
                ResultQuery = GetResult(new Uri(strQuery));
                TVInformationQuery ResultsFromQuery = JsonConvert.DeserializeObject<TVInformationQuery>(ResultQuery);
                if (ResultsFromQuery.results != null)
                {
                    if (ResultsFromQuery.results.Length > 0)
                    {
                        List<int> numbersComputed = new List<int>();

                        foreach (var info in ResultsFromQuery.results)
                        {
                            if (watch.Title != string.Empty)
                            {
                                numbersComputed.Add(Helper.LevenshteinDistance.Compute(watch.Title, info.name));
                            }
                            else if (watch.TitleOriginal != string.Empty)
                            {
                                numbersComputed.Add(Helper.LevenshteinDistance.Compute(watch.TitleOriginal, info.original_name));
                            }
                        }
                        //return int[0] = index
                        //return int[1] = value
                        int x = GetMinNumber(numbersComputed.ToArray())[0];
                        var Info = ResultsFromQuery.results[x];

                        Uri baseQueryExternalId = new Uri("https://api.themoviedb.org/3/tv/");
                        Uri uriExternalId = new Uri(baseQueryExternalId, $"{ResultsFromQuery.results[0].id}/external_ids")
            .AddQuery("api_key", "3cc7aa7a8972f7e07bba853a11fbd66f");
                        string ResultId = GetResult(uriExternalId);

                        ExternalID.Movie infoMovie = JsonConvert.DeserializeObject<ExternalID.Movie>(ResultId);
                        watch.IDTheMovieDB = infoMovie.id;
                        watch.IDIMDb = infoMovie.imdb_id;
                        var genres = new List<string>();
                        foreach (var genre in Info.genre_ids)
                        {
                            genres.Add(Genre.MovieWatch[genre]);
                        }
                        //watch.Title = (watch.Title == string.Empty) ? info.name : watch.Title;
                        watch.Title = Info.name;
                        //watch.TitleOriginal = (watch.TitleOriginal == string.Empty) ? info.original_name : watch.TitleOriginal;
                        watch.TitleOriginal = Info.original_name;
                        watch.Genre = genres.ToArray();
                        watch.Synopsis = Info.overview;
                        watch.BackdropPicture = Info.backdrop_path;
                        watch.PosterPicture = Info.poster_path;
                        watch.Date = Info.first_air_date;
                        return watch;
                    }
                }
            }
            throw new NullReferenceException("Nada foi encontrado.");
        }

        static int[] GetMinNumber(int[] numbers)
        {
            
            int posicao_menor = 0;
            int menor = numbers[0];
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < menor)
                {
                    menor = numbers[i];
                    posicao_menor = i;
                }
            }

            return new int[] { posicao_menor, menor };
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
