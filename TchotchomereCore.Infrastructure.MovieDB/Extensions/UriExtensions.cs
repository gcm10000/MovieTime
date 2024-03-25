//using System.Web;

//namespace TchotchomereCore.Infrastructure.MovieDB.Extensions;
//public static class UriExtensions
//{
//    public static Uri AddQuery(this Uri uri, string name, string value)
//    {
//        var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

//        httpValueCollection.Remove(name);
//        httpValueCollection.Add(name, value);

//        var ub = new UriBuilder(uri);
//        ub.Query = httpValueCollection.ToString();

//        return ub.Uri;
//    }
//}
