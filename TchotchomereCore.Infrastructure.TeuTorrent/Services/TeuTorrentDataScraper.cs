using HtmlAgilityPack;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using TchotchomereCore.Application.Builders;
using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.Enums;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure.Helpers;

namespace TchotchomereCore.Infrastructure.TeuTorrent.Services;
public class TeuTorrentDataScraper : IDataScraper
{
    public MovieBuilder? GetMovieBuilderOrDefault(
        string html, 
        string url)
    {
        if (string.IsNullOrWhiteSpace(html))
            return null;

        string classContent = "content";
        var movieBuilder = new MovieBuilder();

        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var informationArea = doc.DocumentNode.SelectSingleNode($"//div[contains(@class, '{classContent}')]");

        if (informationArea == null)
            return null;

        var infoText = informationArea.InnerHtml
            .Replace("<br>", "\n")
            .StripHTML();

        var infoTextSplited = infoText
            .Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

        var titleByLinq = infoTextSplited
            .Select(x => x.RemoveDiacritics())
            .Select(x => x.Trim())
            .Select(x => x.ToLower())
            .Where(x => x.Contains(":"))
            .Where(x => x.Contains("titulo"))
            .Select(x => x.Substring(x.IndexOf(':') + 1))
            .Select(x => x.Trim())
            .FirstOrDefault();

        movieBuilder.SetTitle(titleByLinq);

        foreach (var strInfo in infoTextSplited)
        {
            if (strInfo.Contains("Baixar:") || strInfo.Contains("Baixar Filme:") || strInfo.Contains("Baixar Série:") || strInfo.Contains("Título Traduzido:") || strInfo.Contains("Titulo Traduzido:"))
            {
                var title = strInfo.Replace("Baixar:", "").Replace("Baixar Filme:", "").Replace("Baixar Série:", "").Replace("Titulo Traduzido:", "").Replace("Título Traduzido:", "").Trim().CareTitle();
                movieBuilder.SetTitle(title);
            }
            else if (strInfo.Contains("Titulo Original:") || strInfo.Contains("Título Original:"))
            {
                var originalTitle = strInfo.Replace("Titulo Original:", "").Replace("Título Original:", "").Trim().CareTitle();
                movieBuilder.SetOriginalTitle(originalTitle);
            }
            else if (strInfo.ToLower().Contains("duração:"))
            {
                var duration = strInfo.ToLower().Replace("duração:", "").Trim();
                movieBuilder.SetDuration(duration);
            }
        }

        var links = doc.DocumentNode.SelectNodes("//a");
        var linksInside = informationArea.SelectNodes(".//a");
        var h4Inside = informationArea.SelectNodes(".//h4");

        var typeMovie = GetTypeMovieByUrl(doc);

        movieBuilder.SetTypeWatch(typeMovie);

        var dataDownloads = GetDataDownloads(doc, informationArea);

        movieBuilder.AddDataDownload(dataDownloads);

        return movieBuilder;
    }

    private static List<DataDownload> GetDataDownloads(
        HtmlDocument doc,
        HtmlNode informationArea)
    {
        var linksInside = informationArea?.SelectNodes(".//a");
        var h4Inside = informationArea?.SelectNodes(".//h4");

        if (linksInside?.Count == 1)
        {
            var singleDataDownloads = GetSingleDataDownloads(doc);
            return singleDataDownloads;
        }
        else if (linksInside?.Count > 1 && h4Inside != null)
        {
            var seriesDataDownloads = GetSeriesDataDownloads(h4Inside, doc);
            return seriesDataDownloads;
        }

        return GetStandardDataDownloads(doc);
    }

    private static List<DataDownload> GetSingleDataDownloads(HtmlDocument doc)
    {
        var links = doc.DocumentNode.SelectNodes("//a");
        return GetDownloadsFromLinks(links);
    }

    public static List<DataDownload> GetSeriesDataDownloads(
        HtmlNodeCollection h4Inside, 
        HtmlDocument doc)
    {
        return h4Inside
            .Select(h4 => new
            {
                Text = System.Web.HttpUtility.HtmlDecode(h4.InnerText)
                    .ToLower()
                    .RemoveDiacritics()
                    .Replace("–", "")
                    .Replace("download", "")
                    .Replace("episodio", "Episódio")
                    .Trim(),
                TagAnchor = h4.SelectSingleNode(".//a")
            })
            .Select(x => new
            {
                Text = x.Text,
                Href = HttpUtility.HtmlDecode(x.TagAnchor?.Attributes["href"]?.Value)
            })
            .Where(obj => !string.IsNullOrEmpty(obj.Href))
            .Where(obj => obj.Href!.Contains("magnet"))
            .Select(obj => new
            {
                Magnet = GetMagnetOrDefault(obj.Href),
                obj
            })
            .Where(obj => !string.IsNullOrEmpty(obj.Magnet))
            .Select(x => new DataDownload
            {
                Magnet = x.Magnet!,
                SeasonTV = GetDataDownloadSeason(doc, x.obj.Text),
                EpisodeTV = x.obj.Text
            })
            .ToList();
    }

    private static int? GetDataDownloadSeason(HtmlDocument doc, string innerText)
    {
        var headline = GetHeadline(doc);
        var seasonsFromHeadline = GetSeasonsFromText(headline);

        if (seasonsFromHeadline.Count == 0)
        {
            var season = seasonsFromHeadline.FirstOrDefault();
            return season;
        }

        var seasonsFromInnerText = GetSeasonsFromText(innerText);
        var result = seasonsFromInnerText.FirstOrDefault();
        return result;
    }

    private static string GetHeadline(HtmlDocument doc)
    {
        string headlineText = doc.DocumentNode.SelectSingleNode("//h1[@class='big title-single']/strong").InnerText;
        return headlineText;
    }

    private static List<DataDownload> GetStandardDataDownloads(HtmlDocument doc)
    {
        var links = doc.DocumentNode.SelectNodes("//a");
        return GetDownloadsFromLinks(links);
    }

    private static List<DataDownload> GetDownloadsFromLinks(HtmlNodeCollection? links)
    {
        if (links == null)
            return [];

        return links.Where(link => link.Attributes["href"] != null)
            .Select(link => System.Web.HttpUtility.HtmlDecode(link.Attributes["href"].Value))
            .Where(link => link.Contains("magnet"))
            .Select(magnet => new DataDownload() { Magnet = magnet })
            .ToList();
    }

    static bool AnyMagnetAsParamUrl(string url)
    {
        var uri = new Uri(url);

        var queryParameters = HttpUtility.ParseQueryString(uri.Query);

        if (!queryParameters.HasKeys())
            return false;

        var anyMagnetAsParamUrl = queryParameters.AllKeys
            .Select(key => new { Value = queryParameters[key] })
            .Where(parameter => !string.IsNullOrEmpty(parameter.Value))
            .Any(parameter => parameter.Value!.Contains("magnet"));

        return anyMagnetAsParamUrl;
    }

    private static string? GetMagnetOrDefault(string? url)
    {
        if (url is null) 
            return null;

        if (!AnyMagnetAsParamUrl(url))
            return url;

        var extractedMagnetLink = ExtractMagnetLinkOrDefault(url);
        return extractedMagnetLink;
    }

    private static string? ExtractMagnetLinkOrDefault(string url)
    {
        var uri = new Uri(url);

        var queryParameters = HttpUtility.ParseQueryString(uri.Query);

        if (!queryParameters.HasKeys())
            return null;

        var extractedMagnetLink = queryParameters.AllKeys
            .Select(key => new { Value = queryParameters[key] })
            .Where(parameter => !string.IsNullOrEmpty(parameter.Value))
            .Where(parameter => parameter.Value!.Contains("magnet"))
            .Select(parameter => HttpUtility.UrlDecode(HttpUtility.HtmlDecode(parameter.Value)))
            .FirstOrDefault();

        return extractedMagnetLink;
    }

    public static int GetSeason(string url)
    {
        int index = url.IndexOf("-temporada", 0);
        if (index > -1)
        {
            var str = url.Remove(index);
            var split = str.Split('-');
            var numberOnly = Regex.Replace(split[split.Length - 1], "[^0-9.]", "");
            var x = Convert.ToInt32(numberOnly);
            return x;
        }
        return 0;
    }

    public static List<int> GetSeasonsFromText(string headlineText)
    {
        if (!headlineText.Contains("temporada", StringComparison.CurrentCultureIgnoreCase))
            return [];

        // Padrão de expressão regular para extrair números de temporadas de uma string de texto
        string pattern = @"\b(?:Todas\s+Temporada)?\s*(\d+)\s*(?:ª|a)?\s*(?:à\s+(\d+))?";

        // Lista para armazenar os números de temporada encontrados
        List<int> seasons = new List<int>();

        // Procura por todas as correspondências com o padrão na string do texto da manchete
        MatchCollection matches = Regex.Matches(headlineText, pattern, RegexOptions.IgnoreCase);

        // Itera sobre as correspondências encontradas e extrai os números de temporada
        foreach (Match match in matches)
        {
            // Extrai o número de temporada à esquerda
            if (int.TryParse(match.Groups[1].Value, out int seasonNumber))
            {
                seasons.Add(seasonNumber);
            }

            // Extrai o número de temporada à direita, se presente
            if (match.Groups.Count > 2 && int.TryParse(match.Groups[2].Value, out int seasonNumberRight))
            {
                for (int i = seasonNumber + 1; i <= seasonNumberRight; i++)
                {
                    seasons.Add(i);
                }
            }
        }

        // Remove duplicatas e retorna os números de temporada
        var result = seasons
            .Distinct()
            .Where(season => season >= 1 && season <= 99)
            .ToList();

        return result;
    }

    private static ETypeWatch GetTypeMovieByUrl(HtmlDocument doc)
    {
        var headline = GetHeadline(doc);

        var season = GetSeasonsFromText(headline);

        if (season.Count > 0)
            return ETypeWatch.Series;

        return ETypeWatch.Film;
    }
}
