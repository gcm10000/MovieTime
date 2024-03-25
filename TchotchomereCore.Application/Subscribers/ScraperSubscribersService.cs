using DotNetCore.CAP;
using TchotchomereCore.Application.Builders;
using TchotchomereCore.Application.DTOs.Events;
using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.Enums;
using TchotchomereCore.Domain.Interfaces;

namespace TchotchomereCore.Application.Subscribers;
public class ScraperSubscribersService : ICapSubscribe
{
    private readonly IDataScraper _dataScraper;
    private readonly IFilmRepository _filmRepository;
    private readonly ISerieRepository _serieRepository;
    private readonly IMovieTracker _movieTracker;

    public ScraperSubscribersService(
        IDataScraper dataScraper,
        IFilmRepository filmRepository,
        ISerieRepository serieRepository,
        IMovieTracker movieTracker)
    {
        _dataScraper = dataScraper;
        _filmRepository = filmRepository;
        _serieRepository = serieRepository;
        _movieTracker = movieTracker;
    }

    [CapSubscribe(nameof(ExtractedUrlsEvent))]
    public async Task Handle(ExtractedUrlsEvent @event)
    {
        if (@event.CurrentExtractedURL.Html is null)
            return;

        var movieBuilder = _dataScraper
            .GetMovieBuilderOrDefault(@event.CurrentExtractedURL.Html, @event.CurrentExtractedURL.Url);

        if (movieBuilder is null)
            return;

        if (movieBuilder.TypeWatch == ETypeWatch.Film)
        {
            await HandleFilmAsync(movieBuilder, @event.CurrentExtractedURL.Id);
            return;
        }

        await HandleSerieAsync(movieBuilder, @event.CurrentExtractedURL.Id);
    }

    private async Task HandleSerieAsync(MovieBuilder movieBuilder, Guid extractedUrlId)
    {
        var populatedSerieBuilder = default(MovieBuilder);
        
        try
        {
            populatedSerieBuilder = await _movieTracker.GetSerieInformationAsync(movieBuilder);
        }
        catch (Exception)
        {
            populatedSerieBuilder.SetMovieStatus(EMovieStatus.Error);
        }
        finally
        {
            var serie = (Serie)populatedSerieBuilder.Build(extractedUrlId);
            await _serieRepository.AddAsync(serie);
        }
    }

    private async Task HandleFilmAsync(MovieBuilder movieBuilder, Guid extractedUrlId)
    {
        var populatedSerieBuilder = default(MovieBuilder);
        try
        {
            populatedSerieBuilder = await _movieTracker.GetFilmInformationAsync(movieBuilder);
        }
        catch (Exception)
        {
            populatedSerieBuilder.SetMovieStatus(EMovieStatus.Error);
        }
        finally
        {
            var film = (Film)populatedSerieBuilder.Build(extractedUrlId);
            await _filmRepository.AddAsync(film);
        }
    }
}
