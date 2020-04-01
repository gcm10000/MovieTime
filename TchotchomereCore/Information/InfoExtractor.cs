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
        public static int EpisodeExtractor(string magnet)
        {
            Regex EpisodeRegex = new Regex("E(.*?)[%|\\s|.|-|$]", RegexOptions.Singleline | RegexOptions.CultureInvariant);
            if (EpisodeRegex.IsMatch(magnet))
            {
                var resultRegex = EpisodeRegex.Match(magnet);
                var mRegex = resultRegex.Groups[1];
                return int.Parse(mRegex.Value); 
            }
            return -1;
        }
    }
}
