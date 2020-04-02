using HtmlAgilityPack;
using MovieTimeLibraryCore;
using System;
using System.Net;

namespace TchotchomereCore.Information
{
    public class GetInformationFromPage
    {
        public Watch GetInfoFromTeuTorrent(string html, string url, string classContent)
        {
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var infoHTML = doc.DocumentNode.SelectSingleNode($"//*[@class=\"{classContent}\"]");
                if (infoHTML == null) return null;
                string infoText = infoHTML.InnerHtml.Replace("<br>", "\n").StripHTML();
                string[] infoTextSplited = infoText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                Watch watch = new Watch();
                DownloadData downloadData = new DownloadData();

                foreach (var strInfo in infoTextSplited)
                {
                    if (strInfo.Contains("Baixar:") || strInfo.Contains("Baixar Filme:") || strInfo.Contains("Baixar Série:") || strInfo.Contains("Título Traduzido:") || strInfo.Contains("Titulo Traduzido:"))
                    {
                        watch.Title = strInfo.Replace("Baixar:", "").Replace("Baixar Filme:", "").Replace("Baixar Série:", "").Replace("Titulo Traduzido:", "").Replace("Título Traduzido:", "").Trim().CareTitle();
                    }
                    else if (strInfo.Contains("Titulo Original:") || strInfo.Contains("Título Original:"))
                    {
                        watch.TitleOriginal = strInfo.Replace("Titulo Original:", "").Replace("Título Original:", "").Trim().CareTitle();
                    }
                    else if (strInfo.Contains("Qualidade:"))
                    {
                        downloadData.Quality = strInfo.Replace("Qualidade:", "").Trim();
                    }
                    else if (strInfo.Contains("Áudio:") && (downloadData.Audio == string.Empty))
                    {
                        downloadData.Audio = strInfo.Replace("Áudio:", "").Trim();
                    }
                    else if (strInfo.ToLower().Contains("formato:"))
                    {
                        downloadData.Format = strInfo.ToLower().Replace("formato:", "").ToUpper().Trim();
                    }
                    else if (strInfo.ToLower().Contains("tamanho:"))
                    {
                        downloadData.Size = strInfo.ToLower().Replace("tamanho:", "").ToUpper().Trim();
                    }
                    else if (strInfo.ToLower().Contains("duração:"))
                    {
                        watch.Duration = strInfo.ToLower().Replace("duração:", "").Trim();
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

                return watch;
                //GetInformationFromAPI info = new GetInformationFromAPI();
                //info.InformationFromTheMovieDB(watch);

                //---OUTPUT---
                //Console.WriteLine("Information from TeuTorrent:");
                //Console.WriteLine("Title: " + watch.Title);
                //Console.WriteLine("TitleOriginal: " + watch.TitleOriginal);
                //Console.WriteLine("Duration: " + watch.Duration);
                //int e = 1;
                //Console.WriteLine("Information of Subtitle:");
                //Console.WriteLine("Total Count: " + watch.Subtitles.Count.ToString());
                //foreach (var subtitle in watch.Subtitles)
                //{
                //    Console.WriteLine(" {0}) ", e++);
                //    Console.WriteLine("Language: " + subtitle.Lang);
                //    Console.WriteLine("Download: " + subtitle.DownloadText);
                //}
                //Console.WriteLine("Information of download:");
                //Console.WriteLine("Total Count: " + watch.Downloads.Count.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }
        public Watch GetInfoFromBludv(string html, string url, string classContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var infoHTML = doc.DocumentNode.SelectSingleNode($"//*[@class=\"{classContent}\"]");
            if (infoHTML == null) return null;
            string infoText = infoHTML.InnerHtml.Replace("<br>", "\n").StripHTML();
            string[] infoTextSplited = infoText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            Watch watch = new Watch();
            watch.Downloads = new System.Collections.Generic.List<DownloadData>();
            DownloadData downloadData = new DownloadData();

            foreach (var strInfo in infoTextSplited)
            {
                if (strInfo.Contains("Baixar:") || strInfo.Contains("Baixar Filme:") || strInfo.Contains("Baixar Série:") || strInfo.Contains("Título Traduzido:") || strInfo.Contains("Titulo Traduzido:"))
                {
                    watch.Title = strInfo.Replace("Baixar:", "").Replace("Baixar Filme:", "").Replace("Baixar Série:", "").Replace("Titulo Traduzido:", "").Replace("Título Traduzido:", "").Trim().CareTitle();
                }
                else if (strInfo.Contains("Titulo Original:") || strInfo.Contains("Título Original:"))
                {
                    watch.TitleOriginal = strInfo.Replace("Titulo Original:", "").Replace("Título Original:", "").Trim().CareTitle();
                }
                else if (strInfo.Contains("Qualidade:"))
                {
                    downloadData.Quality = strInfo.Replace("Qualidade:", "").Trim();
                }
                else if (strInfo.Contains("Áudio:") && (downloadData.Audio == string.Empty))
                {
                    downloadData.Audio = strInfo.Replace("Áudio:", "").Trim();
                }
                else if (strInfo.ToLower().Contains("formato:"))
                {
                    downloadData.Format = strInfo.ToLower().Replace("formato:", "").ToUpper().Trim();
                }
                else if (strInfo.ToLower().Contains("tamanho:"))
                {
                    downloadData.Size = strInfo.ToLower().Replace("tamanho:", "").ToUpper().Trim();
                }
                else if (strInfo.ToLower().Contains("duração:"))
                {
                    watch.Duration = strInfo.ToLower().Replace("duração:", "").Trim();
                }
            }

            var links = doc.DocumentNode.SelectNodes("//a");
            var linksInside = infoHTML.SelectNodes(".//a");

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
                    //watch.Type = TypeWatch.Series;
                    if (linksInside != null)
                    {
                        foreach (var a in linksInside)
                        {
                            var text = System.Web.HttpUtility.HtmlDecode(a.InnerText).ToLower().RemoveDiacritics().Replace("–", "").Replace("download", "").Replace("episodio", "Episódio").Trim(); // – Download
                            var link = System.Web.HttpUtility.HtmlDecode(a.Attributes["href"].Value);
                            if (link.Contains("magnet:?"))
                            {
                                DownloadData data = new DownloadData();
                                data.Quality = text.ToUpper();
                                data.Audio = downloadData.Audio;
                                data.Format = downloadData.Format;
                                data.Size = WebUtility.HtmlDecode(downloadData.Size);
                                data.Magnet = link;
                                if (link.Contains("Vingadores"))
                                {

                                }
                                if (watch.Type == TypeWatch.Series)
                                    if (InfoExtractor.ExistsDisplayName(link))
                                        data.EpisodeTV = InfoExtractor.EpisodeExtractor(link);
                                //else if ...
                                data.SeasonTV = url.GetSeason();
                                watch.Downloads.Add(data);
                            }
                        }
                    }
                }
            }

            return watch;
        }
    }
}
