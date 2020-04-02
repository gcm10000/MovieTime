using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TchotchomereCore.Information
{
    public static class InfoExtractor
    {
        public static bool ExistsDisplayName(string magnet)
        {
            Regex MagnetRegex = new Regex("dn=(.*?)[&|$]", RegexOptions.Singleline | RegexOptions.CultureInvariant);
            return MagnetRegex.IsMatch(magnet);
        }
        public static string EpisodeExtractor(string magnet)
        {
            var dn = Regex.Match(magnet, "dn=(.*?)[&|$]", RegexOptions.IgnoreCase).Value;
            Regex MagnetRegex = new Regex("dn=(.*?)[&|$]", RegexOptions.Singleline | RegexOptions.CultureInvariant);
            Regex EpisodeRegex = new Regex("E(.*?)[%|\\s|.|+|-|_|[a-zA-Z]|$]", RegexOptions.Singleline | RegexOptions.CultureInvariant);
            if (EpisodeRegex.IsMatch(dn))
            {
                var resultRegex = EpisodeRegex.Match(dn);
                var mRegex = resultRegex.Groups[1];
                return (mRegex.Value); 
            }
            return string.Empty;
        }
    }
}
