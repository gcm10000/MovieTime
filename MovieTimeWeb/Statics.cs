namespace MovieTimeWeb
{
    public class Statics
    {
        public static string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionStringName"].ConnectionString;
    }
}