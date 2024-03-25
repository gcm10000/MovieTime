using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TchotchomereCore.Domain.Interfaces;

namespace TchotchomereCore;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var serviceProvider = _serviceProvider.CreateScope().ServiceProvider;
        
        var crawler = serviceProvider.GetRequiredService<ICrawlerLinkExtractor>();
        
        var logger = serviceProvider.GetRequiredService<ILogger<Worker>>();

        logger.LogInformation("System starting now...");
        await crawler.StartAsync(stoppingToken);
        logger.LogInformation("System finished successfully");
        
    }
}
