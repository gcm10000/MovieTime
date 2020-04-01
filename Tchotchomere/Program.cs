using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using LibraryShared;
using Newtonsoft.Json.Linq;
using System.Net.Http;


//https://openlink.click/feroz/?b=d3d3LnRlY2hub2xvZ3ktdW5pdmVyc2UuY29t&url=aHR0cHM6Ly9ueWFhLnNpL2Rvd25sb2FkLzExNTU4OTYudG9ycmVudA==&user=136870344-8&type=2&vez=2&anali=
//base64 on var url
namespace Tchotchomere
{
    class Program
    {

        //API KEY THE MOVIE DB (v3 auth): 3cc7aa7a8972f7e07bba853a11fbd66f
        const string url = "https://teutorrent.com/";
        //const string ConnectionString = @"Data Source=GABRIEL-PC\SQLEXPRESS;Initial Catalog=movietime_database;Integrated Security=True";
        //Server=sql.freeasphost.net\MSSQL2016;Database=movietime_database;uid=movietime;pwd=D8XCkpZhSWUGeCM;
        const string ConnectionString = @"Data Source=SQL5042.site4now.net;Initial Catalog=DB_A4EA9F_movietime;User Id=DB_A4EA9F_movietime_admin;Password=D8XCkpZhSWUGeCM;";
        const string apiKey = "3cc7aa7a8972f7e07bba853a11fbd66f";
        static string path = Path.Combine(Environment.CurrentDirectory, "urls");
        static string PathNewUrls = Path.Combine(path, "newurls.json");
        static string PathOldUrls = Path.Combine(path, "oldurls.json");
        static string PathErrorUrls = Path.Combine(path, "errorurls.json");
        static string PathMagnets = Path.Combine(path, "magnets.magnets");

        static List<string> NewUrls = new List<string>();
        static List<string> OldUrls = new List<string>();
        static int AddDb = 0;

        static BotClient botClient;
        static void Main(string[] args)
        {
            CreatePaths();
            NewUrls.Add(url);
            OldUrls.Add(url);
            //do while...
            //do first time, so save links on List and cycle repeat accessing all urls of website

            botClient = new BotClient("https://www.bludv.tv/", true);
            botClient.ResultEvent += BotClient_ResultEvent;
            botClient.Start();

            //do
            //{
            //    string title = string.Format("Total requested: {0}, Total Found: {1}, Added: {2}, Url requested: {3}", OldUrls.Count, NewUrls.Count, AddDb, NewUrls[0]);
            //    Console.Clear();
            //    Console.WriteLine(title);
            //    Console.Title = title;

            //    AccessingUrl(NewUrls[0]);
            //    Console.WriteLine();
            //    OldUrls.Add(NewUrls[0]);
            //    NewUrls.Remove(NewUrls[0]);

            //    ReleaseData(PathNewUrls, NewUrls);
            //    ReleaseData(PathOldUrls, OldUrls);

            //} while (NewUrls.Count > 0);
            Console.ReadKey();
        }
        private static void BotClient_ResultEvent(ResultEventArgs Result)
        {
            if (Result.Address.ToLower().Contains("temporada"))
            {

            }
            if (Result.Exception != null)
            {
                Console.WriteLine(Result.Exception.Message);
                return;
            }
            Console.Title = $"Total queued: {Result.OldUrls.Count} --- To load: {Result.NewUrls.Count} --- Current address: {Result.Address}";
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
        static void AccessingUrl(string link)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Encoding = System.Text.Encoding.UTF8;
                    string content = webClient.DownloadString(link);
                    List<string> urls = LinkExtractor.ExtractUrlSameHost(content, link);
                    foreach (var url in urls.ToList())
                    {
                        if (!NewUrls.Contains(url) && (!OldUrls.Contains(url)))
                            NewUrls.Add(url);
                        //Console.WriteLine(url);
                    }
                    GetInfoOnPage(content, link);
                }
            }
            catch (System.Net.WebException ex)
            {
                Console.WriteLine("Error: " + ex.Response);
                Console.WriteLine("Try again...");
                AccessingUrl(link);
            }
        }
        static void Base64Decode(string base64Encoded)
        {
            string base64Decoded;
            byte[] data = System.Convert.FromBase64String(base64Encoded);
            base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
            Console.Write(base64Decoded);
        }
        static void GetInfoOnPage(string html, string url)
        {
            //url = "https://teutorrent.com/a-vida-moderna-de-rocko-volta-ao-lar-2019-blu-ray-1080p-download-torrent-dub-e-leg/";
            //using (WebClient webClient = new WebClient())
            //{
            //    webClient.Encoding = System.Text.Encoding.UTF8;
            //    html = webClient.DownloadString(url);
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
                    if (info.Contains("Baixar:") || info.Contains("Baixar Filme:") || info.Contains("Baixar Série:") || info.Contains("Título Traduzido:") || info.Contains("Titulo Traduzido:"))
                    {
                        watch.Title = info.Replace("Baixar:", "").Replace("Baixar Filme:", "").Replace("Baixar Série:", "").Replace("Titulo Traduzido:", "").Replace("Título Traduzido:", "").Trim().CareTitle();
                    }
                    else if (info.Contains("Titulo Original:") || info.Contains("Título Original:"))
                    {
                        watch.TitleOriginal = info.Replace("Titulo Original:", "").Replace("Título Original:", "").Trim().CareTitle();
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
                var h4Inside = infoHTML.SelectNodes(".//h4");

                if (url.ToLower().Contains("temporada"))
                {
                    //It's a series!
                    watch.Type = TypeWatch.Series;
                    downloadData.SeasonTV = url.GetSeason();
                }
                else
                {
                    //It's a movie!
                    watch.Type = TypeWatch.Film;
                }
                if (linksInside == null)
                {
                    foreach (var link in links)
                    {
                        if (link.Attributes["href"] != null)
                        {
                            var magnet = System.Web.HttpUtility.HtmlDecode(link.Attributes["href"].Value);
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
                        watch.Type = TypeWatch.Series;
                        if (h4Inside != null)
                        {
                            foreach (var h4 in h4Inside)
                            {
                                var a = h4.SelectSingleNode(".//a");
                                var text = System.Web.HttpUtility.HtmlDecode(h4.InnerText).ToLower().RemoveDiacritics().Replace("–", "").Replace("download", "").Replace("episodio", "Episódio").Trim(); // – Download
                                var magnet = System.Web.HttpUtility.HtmlDecode(a.Attributes["href"].Value);
                                if (magnet.Contains("magnet:?"))
                                {
                                    DownloadData data = new DownloadData();
                                    data.Quality = downloadData.Quality;
                                    data.Audio = downloadData.Audio;
                                    data.Format = downloadData.Format;
                                    data.Size = downloadData.Size;
                                    data.DownloadText = magnet;
                                    data.EpisodeTV = text;
                                    data.SeasonTV = url.GetSeason();
                                    watch.Downloads.Add(data);
                                }
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

                if (!CheckDB(watch))
                {
                    InsertDB(watch);
                    AddDb++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                using (StreamWriter writer = new StreamWriter(PathErrorUrls, true))
                {
                    writer.WriteLine(ex.Message + ": " + url);
                }
            }

        }
        static bool CheckDB(Watch watch)
        {
            Commands commands = new Commands(ConnectionString);
            return commands.CountWatch(watch.Title, watch.TitleOriginal) > 0;
        }
        static void InsertDB(Watch watch)
        {
            //Insert DB here
            Commands cmd = new Commands(ConnectionString);
            int count = cmd.CountWatch(watch.Title, watch.TitleOriginal);
            int idWatch;
            if (count == 0)
            {
                Commands command = new Commands(ConnectionString);
                idWatch = command.InsertWatch(watch);
            }
            else
            {
                Commands command = new Commands(ConnectionString);
                idWatch = command.IDWatch(watch.Title, watch.TitleOriginal);
            }


            if (watch.Genres.Length > 0)
            {
                foreach (var genre in watch.Genres)
                {
                    Commands command = new Commands(ConnectionString);
                    command.InsertGenre(genre, idWatch);
                }
            }
            foreach (var download in watch.Downloads)
            {
                Commands command = new Commands(ConnectionString);
                int idDownload = command.InsertDownload(download);
                command.InsertWatchDownload(idWatch, idDownload);
            }
            if (watch.Subtitles.Count > 0)
            {
                foreach (var subtitle in watch.Subtitles)
                {
                    Commands command = new Commands(ConnectionString);
                    int idSubtitle = command.InsertSubtitle(subtitle);
                    command.InsertWatchSubtitle(idWatch, idSubtitle);
                }
            }
        }
        static Watch GetInfoOnTheMovieDB(Watch watch)
        {
            Uri baseUriQuery = new Uri("https://api.themoviedb.org/3/search/");
            //https://api.themoviedb.org/3/search/movie
            //api_key=<api_key>
            //&page=1&include_adult=true&query=homem%20aranha%20de%20volta%20ao%20lar&language=pt-BR
            Uri uriQuery;
            string ResultQuery;

            if (watch.Type == TypeWatch.Film)
            {
                uriQuery = new Uri(baseUriQuery, "movie")
                    .AddQuery("api_key", apiKey)
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
                            .AddQuery("api_key", apiKey);
                        string ResultId = GetResult(uriExternalId);

                        //ExternalID.Movie infoMovie = JsonConvert.DeserializeObject<ExternalID.Movie>(ResultId);
                        var data = (JObject)JsonConvert.DeserializeObject(ResultId);
                        if (data["id"] != null)
                            watch.IDTheMovieDB = (int)data["id"];
                        if (data["imdb_id"] != null)
                            watch.IDIMDb = (string)data["imdb_id"];

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
                        watch.Genres = genres.ToArray();
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
                    .AddQuery("api_key", apiKey)
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
            .AddQuery("api_key", apiKey);
                        string ResultId = GetResult(uriExternalId);

                        ExternalID.Series infoSeries = JsonConvert.DeserializeObject<ExternalID.Series>(ResultId);
                        watch.IDTheMovieDB = infoSeries.id;
                        watch.IDIMDb = infoSeries.imdb_id;
                        var genres = new List<string>();
                        //if (watch.Title.Contains("fear"))
                        //{

                        //}
                        foreach (var genre in Info.genre_ids)
                        {
                            genres.Add(Genre.TVWatch[genre]);
                        }
                        //Do not make this because take along more cicles
                        //watch.Title = (watch.Title == string.Empty) ? info.name : watch.Title;
                        watch.Title = Info.name;
                        //watch.TitleOriginal = (watch.TitleOriginal == string.Empty) ? info.original_name : watch.TitleOriginal;
                        watch.TitleOriginal = Info.original_name;
                        watch.Genres = genres.ToArray();
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
            if (!File.Exists(PathMagnets))
            {
                File.Create(PathMagnets);
            }
            if (!File.Exists(PathOldUrls))
            {
                File.Create(PathOldUrls);
            }
            if (!File.Exists(PathErrorUrls))
            {
                File.Create(PathErrorUrls);
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

            Regex regex = new Regex("<a.?(?:href)=[\"|']?(.*?)[\"|'|\\s|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);
            if (regex.IsMatch(html))
            {
                var matches = regex.Matches(html);
                foreach (Match match in matches)
                {
                    if (IsValidURI(match.Groups[1].Value))
                    {
                        Uri uri = new Uri(match.Groups[1].Value);
                        if (uri.Scheme == "magnet")
                        {
                            list.Add(match.Groups[1].Value);
                        }
                    }
                }
            }

            return list;
        }

        public static List<string> ExtractUrl(string html)
        {
            List<string> list = new List<string>();

            Regex regex = new Regex("<a.?(?:href)=[\"|']?(.*?)[\"|'|\\s|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);
            if (regex.IsMatch(html))
            {
                foreach (Match match in regex.Matches(html))
                {
                    if (IsValidURI(match.Groups[1].Value))
                    {                    
                        Uri uri = new Uri(match.Groups[1].Value);
                        if ((!uri.LocalPath.EndsWith(".css")) && (!uri.LocalPath.EndsWith(".png")) && (!uri.Query.Contains("?amp")) && (uri.Fragment.Equals(string.Empty)))
                        {
                            list.Add(match.Groups[1].Value);
                        }
                    }
                }
            }
            return list;
        }
        public static List<string> ExtractUrlSameHost(string html, string host)
        {
            List<string> list = new List<string>();
            Uri uriOri = new Uri(host);

            Regex regex = new Regex("<a.?(?:href)=[\"|']?(.*?)[\"|'|\\s|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);
            if (regex.IsMatch(html))
            {
                foreach (Match match in regex.Matches(html))
                {
                    if (IsValidURI(match.Groups[1].Value))
                    {                    
                        Uri uri = new Uri(match.Groups[1].Value);
                        if ((uri.Host == uriOri.Host) && (!uri.LocalPath.EndsWith(".css")) && (!uri.LocalPath.EndsWith(".png")) && (!uri.Query.Contains("?amp")) && (uri.Fragment.Equals(string.Empty)))
                        {
                            list.Add(match.Groups[1].Value);
                        }
                    }
                }
            }
            return list;
        }
        public static bool IsValidURI(string uri)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                return false;
            Uri tmp;
            if (!Uri.TryCreate(uri, UriKind.Absolute, out tmp))
                return false;
            return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps || tmp.Scheme.ToLower() == "magnet";
        }
    }
}