using TchotchomereCore.Infrastructure.TeuTorrent.Services;

namespace TchotchomereCore.Tests;

public class TeuTorrentDataScraperTests
{
    [Fact]
    public void ShouldReturnMovieBuilderNotNull()
    {
        var url = "https://teutorrent.theproxy.ws/teen-wolf-3a-temporada-parte-2-2014-bdrip-bluray-720p-dual-audio-torrent/";

        var html = File.ReadAllText("barrados-no-shopping-1995-blu-ray-720p-download-torrent-dublado.html");
        var dataScraper = new TeuTorrentDataScraper();
        var movieBuilder = dataScraper.GetMovieBuilderOrDefault(html, url);

        Assert.NotNull(movieBuilder);
    }

    [Theory]
    [InlineData("The Walking Dead 9ª Temporada (2018) Blu-Ray 720p Download Torrent Dub e Leg")]
    [InlineData("Dr.house Todas Temporada 1ª à 8ª Completa Torrent (2004-2012) Bluray 720p Dublado Download")]
    public void ShouldReturnSeasonNumbersFromHeadline(string headline)
    {
        var seasons = TeuTorrentDataScraper.GetSeasonsFromText(headline);

        Assert.True(seasons.Count != 0);
    }
}