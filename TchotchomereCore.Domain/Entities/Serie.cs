using System.ComponentModel.DataAnnotations.Schema;
using TchotchomereCore.Domain.Enums;

namespace TchotchomereCore.Domain.Entities;

[Table(nameof(Serie))]
public class Serie : Movie
{
    private Serie() { }

    public Serie(
        string title,
        string originalTitle,
        string duration,
        string synopsis,
        List<DataDownload> downloads, 
        EMovieStatus status,
        Guid extractedUrlId) 
        : base(title, originalTitle, duration, synopsis, downloads, status, extractedUrlId)
    {
    }
}
