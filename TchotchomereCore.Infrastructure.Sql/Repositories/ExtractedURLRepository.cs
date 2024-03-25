using Microsoft.EntityFrameworkCore;
using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure.Sql.DataContexts;

namespace TchotchomereCore.Infrastructure.Sql.Repositories;

public class ExtractedURLRepository : IExtractedURLRepository
{
    private readonly DataContext _dataContext;
    public ExtractedURLRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> AnyAsync(string urlHash)
    {
        var result = await _dataContext.ExtractedURL.AnyAsync(x => x.URLHash == urlHash);
        return result;
    }

    public async Task<bool> AnyExtractedURLNotProcessedAsync()
    {
        var result = await _dataContext.ExtractedURL.AnyAsync(x => x.ProcessedDateTime == null);
        return result;
    }

    public async Task<IEnumerable<ExtractedURL>> GetExtractedURLNotProcessedAsync(int quantity)
    {
        var result = await _dataContext.ExtractedURL
            .Where(x => x.ProcessedDateTime == null)
            .Take(quantity)
            .OrderBy(x => x.BaseUrl)
            .ToListAsync();

        return result;
    }

    public async Task AddIfUrlIsNotRegistedAsync(
        ExtractedURL extractedURL, CancellationToken cancellationToken = default)
    {
        if (!await AnyAsync(extractedURL.URLHash))
        {
            await _dataContext.ExtractedURL.AddAsync(extractedURL, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<int> AddRangeIfUrlsAreNotRegistedAsync(
        List<ExtractedURL> extractedUrls, 
        CancellationToken cancellationToken = default)
    {
        var newHashes = extractedUrls
            .Select(url => url.URLHash)
            .ToList();

        // Extrai os hashes das URLs já registradas no banco de dados
        var registeredHashes = await _dataContext.ExtractedURL
            .Select(url => url.URLHash) // Suponha que URLHash seja a coluna onde os hashes são armazenados
            .Where(url => newHashes.Contains(url))
            .ToListAsync(cancellationToken);

        // Calcula o hash para cada URL extraída e verifica se o hash já está registrado
        var urlsToAdd = extractedUrls
            .Where(url => !registeredHashes.Contains(url.URLHash))
            .Distinct()
            .ToList();

        // Adiciona as URLs filtradas ao contexto do banco de dados
        await _dataContext.ExtractedURL.AddRangeAsync(urlsToAdd, cancellationToken);

        // Salva as mudanças no banco de dados
        await _dataContext.SaveChangesAsync(cancellationToken);

        return urlsToAdd.Count;
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
