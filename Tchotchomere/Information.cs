using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tchotchomere
{
    //MOVIE -> movie
    //https://api.themoviedb.org/3/search/movie?api_key=3cc7aa7a8972f7e07bba853a11fbd66f&page=1&include_adult=true&query=homem%20aranha%20de%20volta%20ao%20lar&language=pt-BR
    //SERIES -> tv
    //https://api.themoviedb.org/3/search/tv?api_key=3cc7aa7a8972f7e07bba853a11fbd66f&language=pt-BR&page=1&include_adult=true&query=breaking%20bad
    public class MovieInformationQuery
    {
        public int page { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
        public MovieResultQuery[] results { get; set; }
    }
    public class MovieResultQuery
    {
        public float popularity { get; set; }
        public int id { get; set; }
        public bool video { get; set; }
        public int vote_count { get; set; }
        public float vote_average { get; set; }
        public string title { get; set; }
        public string release_date { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public int[] genre_ids { get; set; }
        public string backdrop_path { get; set; }
        public bool adult { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
    }
    public class TVInformationQuery
    {
        public int page { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
        public TVResultQuery[] results { get; set; }
    }
    public class TVResultQuery
    {
        public string original_name { get; set; }
        public int[] genre_ids { get; set; }
        public string name { get; set; }
        public float popularity { get; set; }
        public string[] origin_country { get; set; }
        public int vote_count { get; set; }
        public string first_air_date { get; set; }
        public string backdrop_path { get; set; }
        public string original_language { get; set; }
        public int id { get; set; }
        public float vote_average { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
    }
    public class ExternalID
    {
        public class Series
        {
            public int id { get; set; }
            public string imdb_id { get; set; }
            public string freebase_mid { get; set; }
            public string freebase_id { get; set; }
            public int tvdb_id { get; set; }
            public int tvrage_id { get; set; }
            public string facebook_id { get; set; }
            public string instagram_id { get; set; }
            public string twitter_id { get; set; }
        }

        public class Movie
        {
            public int id { get; set; }
            public string imdb_id { get; set; }
            public object facebook_id { get; set; }
            public object instagram_id { get; set; }
            public object twitter_id { get; set; }
        }

    }
}
