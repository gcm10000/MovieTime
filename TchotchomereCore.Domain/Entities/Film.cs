using System.ComponentModel.DataAnnotations.Schema;
using TchotchomereCore.Domain.Enums;

namespace TchotchomereCore.Domain.Entities;

[Table(nameof(Film))]
public class Film : Movie
{
    private Film() { }

    public Film(
        string title,
        string originalTitle,
        string? duration,
        string synopsis,
        List<DataDownload> downloads,
        EMovieStatus status,
        Guid extractedUrlId) 
        : base(title, originalTitle, duration, synopsis, downloads, status, extractedUrlId)
    {
    }
}
