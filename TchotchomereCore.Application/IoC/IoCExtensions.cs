using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TchotchomereCore.Application.Services;
using TchotchomereCore.Application.Subscribers;
using TchotchomereCore.Domain.Interfaces;

namespace TchotchomereCore.Application;

public static class IocExtensions
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });

        services.AddScoped<ICrawlerLinkExtractor, GenericCrawlerLinkExtractorService>();
        services.AddTransient<ScraperSubscribersService>();

        return services;
    }
}
