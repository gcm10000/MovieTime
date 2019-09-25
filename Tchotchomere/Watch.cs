using System.Collections.Generic;

namespace Tchotchomere
{
    class Watch
    {
        public enum TypeWatch
        {
            Movie,
            Series
        }
        public string Title { get; set; } = string.Empty;
        public string TitleOriginal { get; set; } = string.Empty;
        public string IMDb { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public TypeWatch Type { get; set; }
        
        public List<DownloadData> Downloads { get; set; } = new List<DownloadData>();
    }
}
