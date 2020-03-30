using System.Collections.Generic;

namespace MovieTimeLibraryCore
{
    public enum TypeWatch
    {
        Series = 0,
        Movie = 1
    }
    public class Watch
    {
        #region Data API MovieTime
        public string Title { get; set; }
        public string TitleOriginal { get; set; }
        public string Duration { get; set; }
        public string Synopsis { get; set; }
        public TypeWatch Type { get; set; }
        public List<Subtitle> Subtitles { get; set; }
        public List<DownloadData> Downloads { get; set; }
        #endregion
        #region Data from API
        public int IDTheMovieDB { get; set; }
        public string IDIMDb { get; set; }
        public List<string> Genres { get; set; }
        public string PosterPicture { get; set; }
        public string BackdropPicture { get; set; }
        public string Date { get; set; }
        #endregion
    }
}
