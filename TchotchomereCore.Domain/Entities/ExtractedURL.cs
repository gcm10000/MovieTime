using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using TchotchomereCore.Application.DTOs.Events;
using TchotchomereCore.Infrastructure;

namespace TchotchomereCore.Domain.Entities;

public sealed class ExtractedURL : Entity
{
    public string Url { get; private set; }
    public string? Html { get; private set; }
    [NotMapped]
    public Uri Uri { get => new(Url); }
    public string URLHash { get; private set; }
    public string BaseUrl { get; private set; }
    /// <summary>
    /// Quando foi acessado a URL.
    /// </summary>
    public DateTime? ProcessedDateTime { get; private set; }

    public Guid Trace { get; private set; }

#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
    private ExtractedURL() { }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.

    public ExtractedURL(Uri uri, Guid trace, string? html = null)
    {
        var baseUrl = uri.Scheme + "://" + uri.Host;
        BaseUrl = baseUrl;
        Url = uri.ToString();
        Trace = trace;
        URLHash = ComputeHash(Url);

        if (html != null)
        {
            var urls = LinkExtractor.ExtractUrl(html);

            var hasTorrent = urls.Any(x => new Uri(x).Scheme == "magnet" || IsValidMagnetUrl(x));
            if (hasTorrent)
            {
                Html = html;

                var @event = new ExtractedUrlsEvent()
                {
                    EventIdentifier = Guid.NewGuid().ToString(),
                    CurrentExtractedURL = this
                };

                Raise(@event);
            }
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        ExtractedURL other = (ExtractedURL)obj;
        return URLHash == other.URLHash;
    }

    public override int GetHashCode()
    {
        return URLHash.GetHashCode();
    }

    private static string ComputeHash(string input)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        var builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }

    static bool IsValidMagnetUrl(string url)
    {
        // Expressão regular para verificar se o parâmetro "url" inicia com "magnet"
        string pattern = @"[?&]url=magnet[^&]+";
        Regex regex = new Regex(pattern);

        return regex.IsMatch(url);
    }
}
