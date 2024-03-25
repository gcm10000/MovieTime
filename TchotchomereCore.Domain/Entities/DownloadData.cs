namespace TchotchomereCore.Domain.Entities;

public class DataDownload : Entity
{
    public string Format { get; set; }
    public string Size { get; set; }
    public int? SeasonTV { get; set; }
    public string EpisodeTV { get; set; }
    public required string Magnet { get; set; }
}
