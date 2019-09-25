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
        #region Data from site
        public string Title { get; set; } = string.Empty;
        public string TitleOriginal { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;
        public TypeWatch Type { get; set; }
        public string Subtitle { get; set; } = string.Empty;
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
        public string SubtitleDownload { get; set; } = string.Empty; //Download
    }
}
