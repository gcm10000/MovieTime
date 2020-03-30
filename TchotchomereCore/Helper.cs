using HtmlAgilityPack;
using MovieTimeLibraryCore;
using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Tchotchomere
{
    public static class Helper
    {
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
                                downloadData.Magnet = magnet;
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
                                    downloadData.Magnet = magnet;
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
                                    data.Magnet = magnet;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }
        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        public static string CareTitle(this string input)
        {
            string[] splitinput = input.Split(' ');
            string newstring = string.Empty;
            foreach (var word in splitinput)
            {
                if (!word.ToLower().Contains("temporada"))
                {
                    if (!((word.Contains("º")) || (word.Contains("ª"))))
                    {
                        newstring += word + " ";
                    }
                }
            }
            return newstring.TrimEnd();
        }
        public static int GetSeason(this string url)
        {
            int index = url.IndexOf("-temporada", 0);
            int x = 0;
            if (index > -1)
            {
                string str = url.Remove(index);
                var split = str.Split('-');
                string numberOnly = Regex.Replace(split[split.Length - 1], "[^0-9.]", "");
                x = Convert.ToInt32(numberOnly);
            }
            return x;
        }
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(System.Text.NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }
        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri);
            ub.Query = httpValueCollection.ToString();

            return ub.Uri;
        }
        public static class LevenshteinDistance
        {
            public static int Compute(string s, string t)
            {
                if (string.IsNullOrEmpty(s))
                {
                    if (string.IsNullOrEmpty(t))
                        return 0;
                    return t.Length;
                }

                if (string.IsNullOrEmpty(t))
                {
                    return s.Length;
                }

                int n = s.Length;
                int m = t.Length;
                int[,] d = new int[n + 1, m + 1];

                // initialize the top and right of the table to 0, 1, 2, ...
                for (int i = 0; i <= n; d[i, 0] = i++) ;
                for (int j = 1; j <= m; d[0, j] = j++) ;

                for (int i = 1; i <= n; i++)
                {
                    for (int j = 1; j <= m; j++)
                    {
                        int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                        int min1 = d[i - 1, j] + 1;
                        int min2 = d[i, j - 1] + 1;
                        int min3 = d[i - 1, j - 1] + cost;
                        d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                    }
                }
                return d[n, m];
            }
        }
    }
}
