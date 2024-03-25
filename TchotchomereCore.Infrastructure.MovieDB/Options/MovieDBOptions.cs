namespace TchotchomereCore.Infrastructure.MovieDB.Options;
public class MovieDBOptions
{
    public string ApiKey { get; set; }
    //public string BaseUrl { get; set; }
    public Uri BaseUriQuery { get; set; }
    public Uri ExternalIdUriForFilm { get; set; }
    public Uri ExternalIdUriForSerie { get; set; }
}
