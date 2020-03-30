using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TchotchomereCore
{
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
