using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tchotchomere
{
    class DownloadData
    {
        public string Quality { get; set; } = string.Empty;
        public string Audio { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int SeasonTV { get; set; } = 0;
        public string EpisodeTV { get; set; } = string.Empty;
        public string DownloadText { get; set; }
    }
}
