using Microsoft.Extensions.DependencyInjection;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure.TeuTorrent.Services;

namespace TchotchomereCore.Infrastructure.TeuTorrent.IoC;

public static class IoCExtensions
{
    public static IServiceCollection ConfigureTeuTorrent(this IServiceCollection services)
    {
        services.AddScoped<IDataScraper, TeuTorrentDataScraper>();
        return services;
    }
}
