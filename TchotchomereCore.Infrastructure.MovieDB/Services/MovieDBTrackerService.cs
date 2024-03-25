using Microsoft.Extensions.Options;
using MovieTimeLibraryCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TchotchomereCore.Application.Builders;
using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.Enums;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure.Helpers;
using TchotchomereCore.Infrastructure.MovieDB.Options;

namespace TchotchomereCore.Infrastructure.MovieDB.Services;
public class MovieDBTrackerService : IMovieTracker
{
    private readonly MovieDBOptions _movieDBOptions;
    //private readonly string _apiKey;
    //private readonly string _baseUrl;

    public MovieDBTrackerService(IOptions<MovieDBOptions> movieDBOptions)
    {
        _movieDBOptions = movieDBOptions.Value;
        //_apiKey = movieDB.ApiKey;
        //_baseUrl = movieDBOptions.Value.BaseUrl;
    }

    public async Task<MovieBuilder> GetFilmInformationAsync(MovieBuilder movieBuilder)
    {
        //Uri baseUriQuery = new Uri(_baseUrl);

        //https://api.themoviedb.org/3/search/movie
        //api_key=<api_key>
        //&page=1&include_adult=true&query=homem%20aranha%20de%20volta%20ao%20lar&language=pt-BR

        var uriSubQuery = new Uri(_movieDBOptions.BaseUriQuery, "movie")
            .AddQuery("api_key", _movieDBOptions.ApiKey)
            .AddQuery("page", "1")
            .AddQuery("include_adult", "true")
            .AddQuery("language", "pt-BR");
            //.AddQuery("query", Uri.EscapeUriString(movieBuilder.Title));

        var resultsFromQuery = await GetInformationOrDefaultAsync<MovieInformationQuery>(movieBuilder, uriSubQuery);

        if (resultsFromQuery is null || resultsFromQuery.results.Length == 0)
        {
            movieBuilder.SetMovieStatus(EMovieStatus.Incomplete);
            return movieBuilder;
        }

        var data = await GetFilmExternalIdsOrDefaultAsync(resultsFromQuery);

        movieBuilder.SetIDTheMovieDB((int?)data?["id"]);
        movieBuilder.SetIDIMDb((string?)data?["imdb_id"]);

        var matchedNumber = resultsFromQuery.results
            .Select((info, index) => new 
            {
                Index = index,
                Distance = !string.IsNullOrEmpty(movieBuilder.Title)
                ? Helper.LevenshteinDistance.Compute(movieBuilder.Title, info.title)
                : Helper.LevenshteinDistance.Compute(movieBuilder.OriginalTitle ?? string.Empty, info.original_title)
            })
            .OrderBy(x => x.Distance)
            .First();

        var movieInformationQueryResult = resultsFromQuery.results[matchedNumber.Index];
        var genres = GetFilmGenres(movieInformationQueryResult);

        movieBuilder.SetTitle(movieInformationQueryResult.title)
            .SetOriginalTitle(movieInformationQueryResult.original_title)
            .SetSynopsis(movieInformationQueryResult.overview)
            .AddGenres(genres)
            .SetBackdropPicture(movieInformationQueryResult.backdrop_path)
            .SetPosterPicture(movieInformationQueryResult.poster_path)
            .SetReleaseDate(movieInformationQueryResult.release_date);

        return movieBuilder;
    }

    public async Task<MovieBuilder> GetSerieInformationAsync(MovieBuilder movieBuilder)
    {
        var uriSubQuery = new Uri(_movieDBOptions.BaseUriQuery, "tv")
            .AddQuery("api_key", _movieDBOptions.ApiKey)
            .AddQuery("page", "1")
            .AddQuery("include_adult", "true")
            .AddQuery("language", "pt-BR");

        var resultsFromQuery = await GetInformationOrDefaultAsync<TVInformationQuery>(movieBuilder, uriSubQuery);

        if (resultsFromQuery is null)
        {
            movieBuilder.SetMovieStatus(EMovieStatus.Incomplete);
            return movieBuilder;
        }

        var matchedNumber = resultsFromQuery.results
            .Select((info, index) => new
            {
                Index = index,
                Distance = !string.IsNullOrEmpty(movieBuilder.Title)
                ? Helper.LevenshteinDistance.Compute(movieBuilder.Title ?? string.Empty, info.name)
                : Helper.LevenshteinDistance.Compute(movieBuilder.OriginalTitle ?? string.Empty, info.original_name)
            })
            .OrderBy(x => x.Distance)
            .First();

        var serieIds = await GetExternalIdsOrDefault(resultsFromQuery);

        var movieInformationQueryResult = resultsFromQuery.results[matchedNumber.Index];

        var genres = GetSerieGenres(movieInformationQueryResult);

        movieBuilder.SetTitle(movieInformationQueryResult.name)
            .SetIDTheMovieDB(serieIds?.id)
            .SetIDIMDb(serieIds?.imdb_id)
            .AddGenres(genres)
            .SetOriginalTitle(movieInformationQueryResult.original_name)
            .SetSynopsis(movieInformationQueryResult.overview)
            .SetBackdropPicture(movieInformationQueryResult.backdrop_path)
            .SetPosterPicture(movieInformationQueryResult.poster_path)
            .SetReleaseDate(movieInformationQueryResult.first_air_date);

        return movieBuilder;
    }

    private async Task<ExternalID.Series?> GetExternalIdsOrDefault(TVInformationQuery? resultsFromQuery)
    {
        if (resultsFromQuery is null) 
            return null;
        Uri uriExternalId = new Uri
        (
            _movieDBOptions.ExternalIdUriForSerie, 
            $"{resultsFromQuery.results[0].id}/external_ids"
        )
        .AddQuery("api_key", _movieDBOptions.ApiKey);

        var resultId = await GetResultAsStringOrDefaultAsync(uriExternalId);

        if (resultId is null)
            return null;

        var serieIds = JsonConvert.DeserializeObject<ExternalID.Series>(resultId);
        return serieIds;
    }

    private static List<Genre> GetSerieGenres(TVResultQuery movieInformationQueryResult)
    {
        var genres = Genre.MovieGenres
            .Where(x => x.TheMovieDBCode.HasValue)
            .Where(x => movieInformationQueryResult.genre_ids.Contains(x.TheMovieDBCode!.Value))
            .ToList();

        //var genres = movieInformationQueryResult.genre_ids
        //    .Where(number => Genre.TVWatch.ContainsKey)
        //    .Select(genre => Genre.MovieGenres.TVWatch[genre])
        //    .ToList();

        return genres;
    }

    private static List<Genre> GetFilmGenres(MovieResultQuery info)
    {
        var genres = Genre.MovieGenres
            .Where(x => x.TheMovieDBCode.HasValue)
            .Where(x => info.genre_ids.Contains(x.TheMovieDBCode!.Value))
            .ToList();

        return genres;
        //var releasedGenres = info.genre_ids
        //    .Where(Genre.FilmGenres.ContainsKey)
        //    .Select(rawGenre => Genre.FilmGenres[rawGenre])
        //    .ToList();

        //return releasedGenres;
    }

    private async Task<JObject?> GetFilmExternalIdsOrDefaultAsync(
        MovieInformationQuery? resultsFromQuery)
    {
        if (resultsFromQuery is null)
            return null;

        Uri uriExternalId = new Uri
        (
            _movieDBOptions.ExternalIdUriForFilm, 
            $"{resultsFromQuery.results[0].id}/external_ids"
        )
        .AddQuery("api_key", _movieDBOptions.ApiKey);
        
        var resultAsString = await GetResultAsStringOrDefaultAsync(uriExternalId);

        if (resultAsString is null)
            return default;

        var data = JsonConvert.DeserializeObject(resultAsString) as JObject;
        return data;
    }

    public static async Task<T?> GetInformationOrDefaultAsync<T>(MovieBuilder movieBuilder, Uri uri)
        where T : IQuery
    {
        var resultsFromFirstQuery = await GetMovieInformationResultByTitleAsync<T>(movieBuilder, uri);

        if (resultsFromFirstQuery is null || resultsFromFirstQuery.IsResultsNull())
        {
            var movieInformationQuery =
                await GetMovieInformationResultByOriginalTitleOrDefault<T>(movieBuilder, uri);
            return movieInformationQuery;
        }
        //return default;

        if (!resultsFromFirstQuery.IsResultsEmpty())
            return resultsFromFirstQuery;

        return default;
    }

    private static async Task<T?> GetMovieInformationResultByTitleAsync<T>(
        MovieBuilder movieBuilder, Uri uriSubQuery)
    {
        if (string.IsNullOrWhiteSpace(movieBuilder.Title))
            return default;

        var queryDubbedTitle = uriSubQuery
            .AddQuery("query", Uri.EscapeDataString(movieBuilder.Title));

        var resultAsString = await GetResultAsStringOrDefaultAsync(queryDubbedTitle);

        if (resultAsString is null)
            return default;

        var result = JsonConvert.DeserializeObject<T>(resultAsString);
        return result;
    }

    private static async Task<T?> GetMovieInformationResultByOriginalTitleOrDefault<T>(
        MovieBuilder movieBuilder, Uri uriSubQuery)
    {
        if (string.IsNullOrWhiteSpace(movieBuilder.OriginalTitle))
            return default;

        var queryOriginalTitle = uriSubQuery
            .AddQuery("query", Uri.EscapeDataString(movieBuilder.OriginalTitle));

        var result = await GetResultAsStringOrDefaultAsync(queryOriginalTitle);

        if (result is null)
            return default;

        var movieInformationQuery = JsonConvert.DeserializeObject<T>(result);
        return movieInformationQuery;
    }

    private static async Task<string?> GetResultAsStringOrDefaultAsync(Uri uri)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd(@"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");

        var response = await client.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();
        return content;
    }
}
