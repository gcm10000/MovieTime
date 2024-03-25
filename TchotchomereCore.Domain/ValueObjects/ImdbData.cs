namespace TchotchomereCore.Domain.ValueObjects;
public record ImdbData
{
    public int? IDTheMovieDB { get; set; }
    public string? IDIMDb { get; set; }
    //public List<string> Genres { get; set; }
    public string PosterPicture { get; set; }
    public string BackdropPicture { get; set; }
    public string Date { get; set; }
}
