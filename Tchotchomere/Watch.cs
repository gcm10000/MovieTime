using System.Collections.Generic;

namespace Tchotchomere
{
    class Watch
    {
        public enum TypeWatch
        {
            Series = 0,
            Movie = 1
        }
        #region Data from site
        public string Title { get; set; } = string.Empty;
        public string TitleOriginal { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;
        public TypeWatch Type { get; set; }
        public List<Subtitle> Subtitles { get; set; } = new List<Subtitle>();
        public List<DownloadData> Downloads { get; set; } = new List<DownloadData>();
        #endregion
        #region Data from API
        public int IDTheMovieDB { get; set; }
        public string IDIMDb { get; set; } = string.Empty;
        public string[] Genre { get; set; }
        public string PosterPicture { get; set; } = string.Empty;
        public string BackdropPicture { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        #endregion
    }
}
