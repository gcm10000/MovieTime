using TchotchomereCore.Domain.Entities;

namespace TchotchomereCore.Domain.Interfaces;
public interface IFilmRepository
{
    Task AddAsync(Film film, CancellationToken cancellationToken = default);
}
