using MovieTimeLibraryCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace TchotchomereCore.Information
{
    public class GetInformationFromAPI
    {
        private string apiKey;
        public GetInformationFromAPI(string apiKey)
        {
            this.apiKey = apiKey;
        }
        public Watch InformationFromTheMovieDB(Watch watch)
        {
            Uri baseUriQuery = new Uri("https://api.themoviedb.org/3/search/");
            //https://api.themoviedb.org/3/search/movie
            //api_key=<api_key>
            //&page=1&include_adult=true&query=homem%20aranha%20de%20volta%20ao%20lar&language=pt-BR
            Uri uriQuery;
            string ResultQuery;

            if (watch.Type == TypeWatch.Film)
            {
                uriQuery = new Uri(baseUriQuery, "movie")
                    .AddQuery("api_key", apiKey)
                    .AddQuery("page", "1")
                    .AddQuery("include_adult", "true")
                    .AddQuery("language", "pt-BR");
                var strQuery = (watch.Title != string.Empty) ? uriQuery.ToString() + $"&query={Uri.EscapeUriString(watch.Title)}" : uriQuery.ToString() + $"&query={Uri.EscapeUriString(watch.TitleOriginal)}";
                ResultQuery = GetResult(new Uri(strQuery));
                MovieInformationQuery ResultsFromQuery = JsonConvert.DeserializeObject<MovieInformationQuery>(ResultQuery);
                if (ResultsFromQuery.results != null)
                {
                    if (ResultsFromQuery.results.Length > 0)
                    {
                        List<int> numbersComputed = new List<int>();

                        foreach (var info in ResultsFromQuery.results)
                        {
                            if (watch.Title != string.Empty)
                            {
                                numbersComputed.Add(Helper.LevenshteinDistance.Compute(watch.Title, info.title));
                            }
                            else if (watch.TitleOriginal != string.Empty)
                            {
                                numbersComputed.Add(Helper.LevenshteinDistance.Compute(watch.TitleOriginal, info.original_title));
                            }
                        }

                        //return int[0] = index
                        //return int[1] = value
                        int x = GetMinNumber(numbersComputed.ToArray())[0];
                        var Info = ResultsFromQuery.results[x];
                        Uri baseQueryExternalId = new Uri("https://api.themoviedb.org/3/movie/");
                        Uri uriExternalId = new Uri(baseQueryExternalId, $"{ResultsFromQuery.results[0].id}/external_ids")
                            .AddQuery("api_key", apiKey);
                        string ResultId = GetResult(uriExternalId);

                        //ExternalID.Movie infoMovie = JsonConvert.DeserializeObject<ExternalID.Movie>(ResultId);
                        var data = (JObject)JsonConvert.DeserializeObject(ResultId);
                        if (data["id"] != null)
                            watch.IDTheMovieDB = (int)data["id"];
                        if (data["imdb_id"] != null)
                            watch.IDIMDb = (string)data["imdb_id"];

                        var genres = new List<string>();
                        foreach (var genre in Info.genre_ids)
                        {
                            genres.Add(Genre.MovieWatch[genre]);
                        }
                        //watch.Title = (watch.Title == string.Empty) ? info.title : watch.Title; -> more cycle 
                        watch.Title = Info.title;
                        //watch.TitleOriginal = (watch.TitleOriginal == string.Empty) ? info.original_title : watch.TitleOriginal;
                        watch.TitleOriginal = Info.original_title;
                        watch.Synopsis = Info.overview;
                        watch.Genres = new List<string>(genres);
                        watch.BackdropPicture = Info.backdrop_path;
                        watch.PosterPicture = Info.poster_path;
                        watch.Date = Info.release_date;
                        return watch;
                    }
                }
            }
            else
            {
                uriQuery = new Uri(baseUriQuery, "tv")
                    .AddQuery("api_key", apiKey)
                    .AddQuery("page", "1")
                    .AddQuery("include_adult", "true")
                    .AddQuery("language", "pt-BR");
                var strQuery = (watch.Title != string.Empty) ? uriQuery.ToString() + $"&query={Uri.EscapeUriString(watch.Title)}" : uriQuery.ToString() + $"&query={Uri.EscapeUriString(watch.TitleOriginal)}";
                ResultQuery = GetResult(new Uri(strQuery));
                TVInformationQuery ResultsFromQuery = JsonConvert.DeserializeObject<TVInformationQuery>(ResultQuery);
                if (ResultsFromQuery.results != null)
                {
                    if (ResultsFromQuery.results.Length > 0)
                    {
                        List<int> numbersComputed = new List<int>();

                        foreach (var info in ResultsFromQuery.results)
                        {
                            if (watch.Title != string.Empty)
                            {
                                numbersComputed.Add(Helper.LevenshteinDistance.Compute(watch.Title, info.name));
                            }
                            else if (watch.TitleOriginal != string.Empty)
                            {
                                numbersComputed.Add(Helper.LevenshteinDistance.Compute(watch.TitleOriginal, info.original_name));
                            }
                        }
                        //return int[0] = index
                        //return int[1] = value
                        int x = GetMinNumber(numbersComputed.ToArray())[0];
                        var Info = ResultsFromQuery.results[x];

                        Uri baseQueryExternalId = new Uri("https://api.themoviedb.org/3/tv/");
                        Uri uriExternalId = new Uri(baseQueryExternalId, $"{ResultsFromQuery.results[0].id}/external_ids")
            .AddQuery("api_key", apiKey);
                        string ResultId = GetResult(uriExternalId);

                        ExternalID.Series infoSeries = JsonConvert.DeserializeObject<ExternalID.Series>(ResultId);
                        watch.IDTheMovieDB = infoSeries.id;
                        watch.IDIMDb = infoSeries.imdb_id;
                        var genres = new List<string>();
                        //if (watch.Title.Contains("fear"))
                        //{

                        //}
                        foreach (var genre in Info.genre_ids)
                        {
                            genres.Add(Genre.TVWatch[genre]);
                        }
                        //Do not make this because take along more cicles
                        //watch.Title = (watch.Title == string.Empty) ? info.name : watch.Title;
                        watch.Title = Info.name;
                        //watch.TitleOriginal = (watch.TitleOriginal == string.Empty) ? info.original_name : watch.TitleOriginal;
                        watch.TitleOriginal = Info.original_name;
                        watch.Genres = new List<string>(genres);
                        watch.Synopsis = Info.overview;
                        watch.BackdropPicture = Info.backdrop_path;
                        watch.PosterPicture = Info.poster_path;
                        watch.Date = Info.first_air_date;
                        return watch;
                    }
                }
            }
            throw new NullReferenceException("Nada foi encontrado.");
        }
        static int[] GetMinNumber(int[] numbers)
        {

            int posicao_menor = 0;
            int menor = numbers[0];
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < menor)
                {
                    menor = numbers[i];
                    posicao_menor = i;
                }
            }

            return new int[] { posicao_menor, menor };
        }
        public static string GetResult(Uri url)
        {
            string result = string.Empty;

            using (var clientAPI = new HttpClient())
            {
                ServicePointManager.Expect100Continue = true;
                //SecurityProtocolType.Tls12 missing on Framework 4.0 only
                //SecurityProtocolType.Tls12  == (SecurityProtocolType)(0xc00)
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc00);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url.ToString());
                request.KeepAlive = false;
                request.UserAgent = @"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    result = reader.ReadToEnd();

                    reader.Close();
                }
            }

            return result;
        }
    }
}
