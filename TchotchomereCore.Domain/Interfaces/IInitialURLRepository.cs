using TchotchomereCore.Domain.Entities;

namespace TchotchomereCore.Domain.Interfaces;

public interface IInitialURLRepository
{
    Task AddAsync(InitialURL entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<InitialURL>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateProcessedDateTimeAsCurrentDateTimeAsync(Guid id, CancellationToken cancellationToken = default);
}
