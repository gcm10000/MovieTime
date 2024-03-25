namespace TchotchomereCore.Domain.Interfaces;
public interface ICrawlerLinkExtractor
{
    Task StartAsync(CancellationToken stoppingToken = default);
}
