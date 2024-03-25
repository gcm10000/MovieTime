using TchotchomereCore.Domain.Entities;

namespace TchotchomereCore.Domain.Interfaces;
public interface ISerieRepository
{
    Task AddAsync(Serie serie, CancellationToken cancellationToken = default);
}
