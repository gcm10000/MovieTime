using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure.MovieDB.Options;
using TchotchomereCore.Infrastructure.MovieDB.Services;

namespace TchotchomereCore.Infrastructure.MovieDB.IoC;

public static class IoCExtensions
{
    public static IServiceCollection ConfigureMovieDB(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var movieDBOptions = configuration.GetRequiredSection(nameof(MovieDBOptions));

        services.AddScoped<IMovieTracker, MovieDBTrackerService>();
        services.Configure<MovieDBOptions>(options =>
        {
            //options.BaseUrl = movieDBOptions[nameof(MovieDBOptions.BaseUrl)]!;
            options.ApiKey = movieDBOptions[nameof(MovieDBOptions.ApiKey)]!;
            options.BaseUriQuery = new Uri(movieDBOptions["BaseUrlForSearch"]!);
            options.ExternalIdUriForFilm = new Uri(movieDBOptions["ExternalIdUrlForFilm"]!);
            options.ExternalIdUriForSerie = new Uri(movieDBOptions["ExternalIdUrlForSerie"]!);
        });

        return services;
    }

}
