using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure.Sql.DataContexts;

namespace TchotchomereCore.Infrastructure.Sql.Repositories;
public class FilmRepository : IFilmRepository
{
    private readonly DataContext _dataContext;
    public FilmRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddAsync(
        Film film, CancellationToken cancellationToken = default)
    {
        await _dataContext.AddAsync(film, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}
