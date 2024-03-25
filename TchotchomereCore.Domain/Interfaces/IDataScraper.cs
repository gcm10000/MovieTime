using TchotchomereCore.Application.Builders;

namespace TchotchomereCore.Domain.Interfaces;
public interface IDataScraper
{
    MovieBuilder? GetMovieBuilderOrDefault(string html, string url);
}
