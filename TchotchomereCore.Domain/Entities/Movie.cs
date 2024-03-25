using TchotchomereCore.Domain.Enums;
using TchotchomereCore.Domain.ValueObjects;

namespace TchotchomereCore.Domain.Entities;
public abstract class Movie : Entity
{
    public string Title { get; set; }
    public string OriginalTitle { get; set; }
    public string? Duration { get; set; }
    public string Synopsis { get; set; }
    //[Required]
    //public TypeWatch Type { get; set; }
    public List<Subtitle> Subtitles { get; set; }
    public List<DataDownload> Downloads { get; set; }
    public ImdbData ImdbData { get; set; }
    public EMovieStatus Status { get; set; }
    public Guid? ExtractedUrlId {  get; set; }
    public ExtractedURL? ExtractedUrl { get; set; }

#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
    protected Movie() { }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.

    public Movie(
        string title,
        string originalTitle,
        string? duration,
        string synopsis,
        List<DataDownload> downloads,
        EMovieStatus status,
        Guid extractedUrlId)
    {
        Title = title;
        OriginalTitle = originalTitle;
        Duration = duration;
        Synopsis = synopsis;
        Downloads = downloads;
        Status = status;
        ExtractedUrlId = extractedUrlId;
    }
}
