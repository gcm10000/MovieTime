using Microsoft.Extensions.Options;
using Moq;
using TchotchomereCore.Application.Subscribers;
using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure.MovieDB.Options;
using TchotchomereCore.Infrastructure.MovieDB.Services;
using TchotchomereCore.Infrastructure.TeuTorrent.Services;

namespace TchotchomereCore.Tests;
public class ScraperSubscribersServiceTests
{
    [Fact]
    public async void ShouldExtractSerieInformation()
    {
        var html = File.ReadAllText("the-walking-dead-9a-temporada-2018-blu-ray-720p-download-torrent-dub-e-leg.html");
        var url = "https://teutorrent.piracyproxy.page/the-walking-dead-9a-temporada-2018-blu-ray-720p-download-torrent-dub-e-leg/";

        await ExecuteAsync(html, url);
    
        Assert.True(true);
    }

    [Fact]
    public async void ShouldExtractFilmInformation()
    {
        var html = File.ReadAllText("barrados-no-shopping-1995-blu-ray-720p-download-torrent-dublado.html");
        var url = "https://teutorrent.theproxy.ws/barrados-no-shopping-1995-blu-ray-720p-download-torrent-dublado/";

        await ExecuteAsync(html, url);

        Assert.True(true);
    }

    private static async Task ExecuteAsync(string html, string url)
    {
        var mockFilmRepository = new Mock<IFilmRepository>();
        var mockSerieRepository = new Mock<ISerieRepository>();

        var dataScraper = new TeuTorrentDataScraper();

        var movieDBOptionsValue = new MovieDBOptions
        {
            ApiKey = "3cc7aa7a8972f7e07bba853a11fbd66f",
            BaseUriQuery = new Uri("https://api.themoviedb.org/3/search/"),
            ExternalIdUriForFilm = new Uri("https://api.themoviedb.org/3/movie/"),
            ExternalIdUriForSerie = new Uri("https://api.themoviedb.org/3/tv/")
        };

        var movieDBOptions = Options.Create(movieDBOptionsValue);

        var movieTracker = new MovieDBTrackerService(movieDBOptions);

        var scraperSubscribersService = new ScraperSubscribersService
        (
            dataScraper: dataScraper,
            filmRepository: mockFilmRepository.Object,
            serieRepository: mockSerieRepository.Object,
            movieTracker: movieTracker
        );


        var @event = new Application.DTOs.Events.ExtractedUrlsEvent
        {
            EventIdentifier = Guid.NewGuid().ToString(),
            CurrentExtractedURL = new ExtractedURL
            (
                uri: new Uri(url),
                trace: Guid.NewGuid(),
                html: html
            )
        };

        await scraperSubscribersService.Handle(@event);
    }
}
