using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure.Sql.DataContexts;

namespace TchotchomereCore.Infrastructure.Sql.Repositories;
internal class SerieRepository : ISerieRepository
{
    private readonly DataContext _dataContext;

    public SerieRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddAsync(Serie serie, CancellationToken cancellationToken = default)
    {
        await _dataContext.AddAsync(serie, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}
