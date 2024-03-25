using TchotchomereCore.Application.Builders;

namespace TchotchomereCore.Domain.Interfaces;
public interface IMovieTracker
{
    Task<MovieBuilder> GetFilmInformationAsync(MovieBuilder movieBuilder);
    Task<MovieBuilder> GetSerieInformationAsync(MovieBuilder movieBuilder);
}
