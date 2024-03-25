using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure.Sql.DataContexts;
using TchotchomereCore.Infrastructure.Sql.Interceptors;
using TchotchomereCore.Infrastructure.Sql.Repositories;

namespace TchotchomereCore.Infrastructure.Sql.IoC;

public static class IoCExtensions
{
    public static IServiceCollection ConfigureSql(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IExtractedURLRepository, ExtractedURLRepository>();
        services.AddScoped<IInitialURLRepository, InitialURLRepository>();
        services.AddScoped<IFilmRepository, FilmRepository>();
        services.AddScoped<ISerieRepository, SerieRepository>();

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddDbContext<DataContext>((sp, options) =>
        {
            ConfigureDatabaseProvider(options, configuration);

            AddInterceptors(sp, options);
        });
        return services;
    }

    private static void AddInterceptors(IServiceProvider sp, DbContextOptionsBuilder options)
    {
        //options.AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
        //options.AddInterceptors(sp.GetRequiredService<CacheInvalidationInterceptor>());
        options.AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>());
    }

    private static void ConfigureDatabaseProvider(
        DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = configuration.GetConnectionString("Data");

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

            optionsBuilder.UseMySql(connectionString, serverVersion);

        }
    }
}
