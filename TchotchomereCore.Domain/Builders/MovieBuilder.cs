
using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.Enums;
using TchotchomereCore.Domain.ValueObjects;

namespace TchotchomereCore.Application.Builders;
public class MovieBuilder
{
    private readonly List<DataDownload> _dataDownloadList = [];
    public string? Title { get; private set; }
    public string? OriginalTitle { get; private set; }
    public string? Duration { get; private set; }
    public string? Synopsis { get; private set; }
    public ETypeWatch TypeWatch { get; private set; }
    public ImdbData ImdbData { get; private set; } = new ImdbData();
    public string BackdropPath { get; private set; }
    public string PosterPath { get; private set; }
    public string ReleaseDate { get; private set; }
    public List<Genre> Genres { get; private set; } = [];
    public EMovieStatus MovieStatus { get; private set; }

    public MovieBuilder SetTitle(string? title)
    {
        Title = title;
        return this;
    }

    public MovieBuilder SetOriginalTitle(string originalTitle)
    {
        OriginalTitle = originalTitle;
        return this;
    }

    public MovieBuilder SetDuration(string duration)
    {
        Duration = duration;
        return this;
    }

    public MovieBuilder SetSynopsis(string synopsis)
    {
        Synopsis = synopsis;
        return this;
    }

    public MovieBuilder AddDataDownload(params DataDownload[] dataDownload)
    {
        _dataDownloadList.AddRange(dataDownload);
        return this;
    }

    public MovieBuilder AddDataDownload(IEnumerable<DataDownload> dataDownload)
    {
        _dataDownloadList.AddRange(dataDownload);
        return this;
    }

    public MovieBuilder SetTypeWatch(ETypeWatch typeWatch)
    {
        TypeWatch = typeWatch;
        return this;
    }

    //public MovieBuilder SetImdbData(ImdbData imdbData)
    //{
    //    ImdbData = imdbData;
    //}

    public Movie Build(Guid extractedId)
    {
        //ArgumentNullException.ThrowIfNull(Title, nameof(MovieBuilder.Title));
        //ArgumentNullException.ThrowIfNull(OriginalTitle, nameof(MovieBuilder.OriginalTitle));
        
        //ArgumentNullException.ThrowIfNull(Duration, nameof(MovieBuilder.Duration));
        
        //ArgumentNullException.ThrowIfNull(Synopsis, nameof(MovieBuilder.Synopsis));

        if (TypeWatch == ETypeWatch.Film)
        {
            var film = new Film
            (
                Title,
                OriginalTitle,
                Duration,
                Synopsis,
                _dataDownloadList,
                MovieStatus,
                extractedId
            );
            return film;
        }

        var serie = new Serie
        (
            Title,
            OriginalTitle,
            Duration,
            Synopsis,
            _dataDownloadList,
            MovieStatus,
            extractedId
        );

        return serie;
    }

    public MovieBuilder AddGenres(IEnumerable<Genre> genres)
    {
        Genres.AddRange(genres);
        return this;
    }

    public MovieBuilder SetBackdropPicture(string backdropPath)
    {
        BackdropPath = backdropPath;
        return this;
    }

    public MovieBuilder SetPosterPicture(string posterPath)
    {
        PosterPath = posterPath;
        return this;
    }

    public MovieBuilder SetReleaseDate(string releaseDate)
    {
        ReleaseDate = releaseDate;
        return this;
    }

    public MovieBuilder SetIDTheMovieDB(int? id)
    {
        ImdbData.IDTheMovieDB = id;
        return this;
    }

    public MovieBuilder SetIDIMDb(string? imdb_id)
    {
        ImdbData.IDIMDb = imdb_id;
        return this;
    }

    public MovieBuilder SetMovieStatus(EMovieStatus movieStatus)
    {
        MovieStatus = movieStatus;
        return this;
    }
}