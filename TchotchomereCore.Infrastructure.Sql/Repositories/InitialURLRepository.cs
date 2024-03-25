using Microsoft.EntityFrameworkCore;
using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure.Sql.DataContexts;

namespace TchotchomereCore.Infrastructure.Sql.Repositories;
public class InitialURLRepository : IInitialURLRepository
{
    private readonly DataContext _dataContext;

    public InitialURLRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddAsync(InitialURL entity, CancellationToken cancellationToken = default)
    {
        await _dataContext.AddAsync(entity, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<InitialURL>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dataContext.InitialURL.ToListAsync(cancellationToken);
        return result;
    }

    public async Task UpdateProcessedDateTimeAsCurrentDateTimeAsync(
        Guid id, CancellationToken cancellationToken = default)
    {
        await _dataContext.ExtractedURL
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(
                x => x.SetProperty(
                    a => a.ProcessedDateTime, b => DateTime.Now
                ), cancellationToken);
    }
}
