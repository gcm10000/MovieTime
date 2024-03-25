using TchotchomereCore.Domain.Entities;

namespace TchotchomereCore.Domain.Interfaces;
public interface IExtractedURLRepository
{
    Task AddIfUrlIsNotRegistedAsync(ExtractedURL extractedURL, CancellationToken cancellationToken = default);
    Task<int> AddRangeIfUrlsAreNotRegistedAsync(List<ExtractedURL> extractedUrls, CancellationToken cancellationToken = default);
    Task<bool> AnyExtractedURLNotProcessedAsync();
    Task<IEnumerable<ExtractedURL>> GetExtractedURLNotProcessedAsync(int quantity);
    Task UpdateProcessedDateTimeAsCurrentDateTimeAsync(Guid id, CancellationToken cancellationToken = default);
}
