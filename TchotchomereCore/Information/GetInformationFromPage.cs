using HtmlAgilityPack;
using MovieTimeLibraryCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TchotchomereCore.Information
{
    public class GetInformationFromPage
    {
        public void GetInfoOnPage(string html, string url)
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
                //watch = GetInfoOnTheMovieDB(watch);
                GetInformationFromAPI info = new GetInformationFromAPI();
                info.InformationFromTheMovieDB(watch);

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
    }
}
