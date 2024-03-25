using System.Text.RegularExpressions;

namespace TchotchomereCore.Infrastructure
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

        //public static List<string> ExtractUrl(string html)
        //{
        //    List<string> list = new List<string>();

        //    Regex regex = new Regex("<a.?(?:href)=[\"|']?(.*?)[\"|'|\\s|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);
        //    if (regex.IsMatch(html))
        //    {
        //        var matches = regex.Matches(html);
        //        foreach (Match match in matches)
        //        {
        //            if (IsValidURI(match.Groups[1].Value))
        //            {
        //                Uri uri = new Uri(match.Groups[1].Value);
        //                if ((!uri.LocalPath.EndsWith(".css")) && (!uri.LocalPath.EndsWith(".png")) && (!uri.Query.Contains("?amp")) && (uri.Fragment.Equals(string.Empty)))
        //                {
        //                    list.Add(match.Groups[1].Value);
        //                }
        //            }
        //        }
        //    }
        //    return list;
        //}

        public static List<string> ExtractUrl(string html)
        {
            List<string> urls = new List<string>();

            // Padrão para capturar links em tags <a> com o atributo href
            string pattern = @"<a[^>]*\bhref\s*=\s*[""']?(?<url>[^""'#\s]+)[""']?[^>]*>";

            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(html);

            foreach (Match match in matches)
            {
                // Obter o valor do atributo href
                string url = match.Groups["url"].Value;

                // Verificar se o URL é válido antes de adicionar à lista
                if (IsValidURI(url))
                {
                    urls.Add(url);
                }
            }

            return urls;
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

        //public static bool IsValidURI(string uri)
        //{
        //    if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
        //        return false;
        //    Uri tmp;
        //    if (!Uri.TryCreate(uri, UriKind.Absolute, out tmp))
        //        return false;
        //    return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps || tmp.Scheme.ToLower() == "magnet";
        //}

        static bool IsValidURI(string uri)
        {
            return Uri.TryCreate(uri, UriKind.Absolute, out _);
        }
    }

}
