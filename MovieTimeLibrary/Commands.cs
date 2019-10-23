using System;

namespace LibraryShared
{
    public class Commands
    {
        Connection Connection;
        public Commands(string ConnectionString)
        {
            Connection = new Connection(ConnectionString);
        }
        public int InsertWatch(Watch watch)
        {
            try
            {
                System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
                sqlCommand.CommandText = "INSERT INTO [dbo].[Watch] ([Title],[TitleOriginal],[Duration],[Synopsis],[IDTheMovieDB],[IDIMdb],[PosterPicture],[BackdropPicture],[Date],[IsMovie]) " +
                    "VALUES(@Title,@TitleOriginal,@Duration,@Synopsis,@IDTheMovieDB,@IDIMdb,@PosterPicture,@BackdropPicture,@Date,@IsMovie)";
                sqlCommand.Parameters.AddWithValue("@Title", watch.Title);
                sqlCommand.Parameters.AddWithValue("@TitleOriginal", watch.TitleOriginal);
                sqlCommand.Parameters.AddWithValue("@Duration", watch.Duration);
                sqlCommand.Parameters.AddWithValue("@Synopsis", watch.Synopsis);
                sqlCommand.Parameters.AddWithValue("@IDTheMovieDB", watch.IDTheMovieDB);
                sqlCommand.Parameters.AddWithValue("@IDIMDb", watch.IDIMDb);
                sqlCommand.Parameters.AddWithValue("@PosterPicture", watch.PosterPicture);
                sqlCommand.Parameters.AddWithValue("@BackdropPicture", watch.BackdropPicture);
                sqlCommand.Parameters.AddWithValue("@Date", watch.Date);
                sqlCommand.Parameters.AddWithValue("@IsMovie", Convert.ToBoolean((int)watch.Type));
                System.Data.SqlClient.SqlConnection connection = Connection.Connect();
                sqlCommand.Connection = connection;
                sqlCommand.ExecuteNonQuery();

                System.Data.SqlClient.SqlCommand CommandSelect = new System.Data.SqlClient.SqlCommand();
                CommandSelect.Connection = connection;
                CommandSelect.CommandText = "SELECT TOP 1 [idWatch] FROM [dbo].[Watch] ORDER BY idWatch DESC";
                int idWatch = 0;
                using (System.Data.SqlClient.SqlDataReader reader = CommandSelect.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idWatch = Convert.ToInt32(reader["idWatch"].ToString());
                    }
                    else
                    {
                        throw new ArgumentNullException("Nada foi encontrado");
                    }
                }
                Connection.Disconnect();
                return idWatch;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int InsertDownload(DownloadData DownloadData)
        {
            try
            {
                //SELECT TOP 1 * FROM Table ORDER BY ID DESC
                System.Data.SqlClient.SqlCommand CommandInsert = new System.Data.SqlClient.SqlCommand();
                CommandInsert.CommandText = "INSERT INTO [dbo].[Download] ([Quality],[Audio],[Format],[Size],[SeasonTV],[EpisodeTV],[DownloadText]) " +
                    "VALUES (@Quality,@Audio,@Format,@Size,@SeasonTV,@EpisodeTV,@DownloadText)";
                CommandInsert.Parameters.AddWithValue("@Quality", DownloadData.Quality);
                CommandInsert.Parameters.AddWithValue("@Audio", DownloadData.Audio);
                CommandInsert.Parameters.AddWithValue("@Format", DownloadData.Format);
                CommandInsert.Parameters.AddWithValue("@Size", DownloadData.Size);
                CommandInsert.Parameters.AddWithValue("@SeasonTV", DownloadData.SeasonTV);
                CommandInsert.Parameters.AddWithValue("@EpisodeTV", DownloadData.EpisodeTV);
                CommandInsert.Parameters.AddWithValue("@DownloadText", DownloadData.DownloadText);

                var conn = Connection.Connect();
                CommandInsert.Connection = conn;
                CommandInsert.ExecuteNonQuery();

                System.Data.SqlClient.SqlCommand CommandSelect = new System.Data.SqlClient.SqlCommand();
                CommandSelect.Connection = conn;
                CommandSelect.CommandText = "SELECT TOP 1 [idDownload] FROM [dbo].[Download] ORDER BY idDownload DESC";
                int id = 0;
                using (System.Data.SqlClient.SqlDataReader reader = CommandSelect.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = Convert.ToInt32(reader["idDownload"].ToString());
                    }
                    else
                    {
                        throw new ArgumentNullException("Nada foi encontrado");
                    }
                }
                Connection.Disconnect();
                return id;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertSubtitle(Subtitle Subtitle)
        {
            try
            {
                //SELECT TOP 1 * FROM Table ORDER BY ID DESC
                System.Data.SqlClient.SqlCommand CommandInsert = new System.Data.SqlClient.SqlCommand();
                CommandInsert.CommandText = "INSERT INTO [dbo].[Subtitle] ([Subtitle] ,[DownloadText]) " +
                    "VALUES (@Subtitle,@DownloadText)";
                CommandInsert.Parameters.AddWithValue("@Subtitle", Subtitle.Lang);
                CommandInsert.Parameters.AddWithValue("@DownloadText", Subtitle.DownloadText);

                var conn = Connection.Connect();
                CommandInsert.Connection = conn;
                CommandInsert.ExecuteNonQuery();

                System.Data.SqlClient.SqlCommand CommandSelect = new System.Data.SqlClient.SqlCommand();
                CommandSelect.Connection = conn;
                CommandSelect.CommandText = "SELECT TOP 1 [idSubtitle] FROM [dbo].[Subtitle] ORDER BY idSubtitle DESC";
                int id = 0;
                using (System.Data.SqlClient.SqlDataReader reader = CommandSelect.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = Convert.ToInt32(reader["idSubtitle"].ToString());
                    }
                    else
                    {
                        throw new ArgumentNullException("Nada foi encontrado");
                    }
                }
                Connection.Disconnect();
                return id;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertWatchDownload(int idWatch, int idDownload)
        {
            try
            {
                System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
                sqlCommand.CommandText = "INSERT INTO [dbo].[Watch_Download] ([idWatch] ,[idDownload]) VALUES (@idWatch,@idDownload)";
                sqlCommand.Parameters.AddWithValue("@idWatch", idWatch.ToString());
                sqlCommand.Parameters.AddWithValue("@idDownload", idDownload.ToString());
                sqlCommand.Connection = Connection.Connect();
                sqlCommand.ExecuteNonQuery();
                Connection.Disconnect();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void InsertGenre(string Genre, int idWatch)
        {
            try
            {
                System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
                sqlCommand.CommandText = "INSERT INTO [dbo].[Genre] ([idWatch] ,[Genre]) VALUES (@idWatch,@Genre)";
                sqlCommand.Parameters.AddWithValue("@idWatch", idWatch);
                sqlCommand.Parameters.AddWithValue("@Genre", Genre);
                sqlCommand.Connection = Connection.Connect();
                sqlCommand.ExecuteNonQuery();
                Connection.Disconnect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertWatchSubtitle(int idWatch, int idSubtitle)
        {
            try
            {
                System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
                sqlCommand.CommandText = "INSERT INTO [dbo].[Watch_Subtitle] ([idWatch] ,[idSubtitle]) VALUES (@idWatch,@idSubtitle) ";
                sqlCommand.Parameters.AddWithValue("@idWatch", idWatch.ToString());
                sqlCommand.Parameters.AddWithValue("@idSubtitle", idSubtitle.ToString());
                sqlCommand.Connection = Connection.Connect();
                sqlCommand.ExecuteNonQuery();
                Connection.Disconnect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int CountWatch(string Title, string TitleOriginal)
        {
            int count;
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = $"SELECT COUNT(*) FROM [dbo].[Watch] WHERE Title = '{Title}' AND TitleOriginal = '{TitleOriginal}'";
            sqlCommand.Connection = Connection.Connect();
            count = (int)sqlCommand.ExecuteScalar();
            Connection.Disconnect();
            return count;
        }
        public int IDWatch(string Title, string TitleOriginal)
        {
            int id;
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = $"SELECT idWatch FROM [dbo].[Watch] WHERE Title = '{Title}' AND TitleOriginal = '{TitleOriginal}'";
            sqlCommand.Connection = Connection.Connect();
            using (System.Data.SqlClient.SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    id = Convert.ToInt32(reader["idWatch"].ToString());
                }
                else
                {
                    throw new ArgumentNullException("Nada foi encontrado");
                }
            }
            Connection.Disconnect();
            return id;
        }
        public Watch GetWatch(int idWatch)
        {
            Watch watch = new Watch();

            System.Data.SqlClient.SqlCommand CommandSelect1 = new System.Data.SqlClient.SqlCommand();
            CommandSelect1.Connection = Connection.Connect();
            CommandSelect1.CommandText = "SELECT [Title] ,[TitleOriginal] ,[Duration] ,[Synopsis] ,[IDTheMovieDB] ,[IDIMdb] ,[PosterPicture] ,[BackdropPicture] ,[Date] ,[IsMovie] FROM [dbo].[Watch] " +
                "WHERE idWatch = " + idWatch.ToString();
            using (System.Data.SqlClient.SqlDataReader reader = CommandSelect1.ExecuteReader())
            {
                if (reader.Read())
                {
                    watch.Title = reader["Title"].ToString();
                    watch.TitleOriginal = reader["TitleOriginal"].ToString();
                    watch.Duration = reader["Duration"].ToString();
                    watch.Synopsis = reader["Synopsis"].ToString();
                    watch.IDTheMovieDB = Convert.ToInt32(reader["IDTheMovieDB"]);
                    watch.IDIMDb = reader["IDIMdb"].ToString();
                    watch.PosterPicture = reader["PosterPicture"].ToString();
                    watch.BackdropPicture = reader["BackdropPicture"].ToString();
                    watch.Date = reader["Date"].ToString();
                    watch.Type = GetType((bool)reader["IsMovie"]);
                }
                else
                {
                    throw new ArgumentNullException("Not found.");
                }
            }
            Connection.Disconnect();

            System.Data.SqlClient.SqlCommand CommandSelect2 = new System.Data.SqlClient.SqlCommand();
            CommandSelect2.Connection = Connection.Connect();
            CommandSelect2.CommandText = "SELECT [Genre] FROM [dbo].[Genre]" +
                "WHERE idWatch = " + idWatch.ToString();
            var listGenre = new System.Collections.Generic.List<string>();
            using (System.Data.SqlClient.SqlDataReader reader = CommandSelect2.ExecuteReader())
            {
                while (reader.Read())
                {
                    listGenre.Add(reader["Genre"].ToString());
                }
            }
            watch.Genres = listGenre.ToArray();
            Connection.Disconnect();

            System.Data.SqlClient.SqlCommand CommandSelect3 = new System.Data.SqlClient.SqlCommand();
            CommandSelect3.Connection = Connection.Connect();
            CommandSelect3.CommandText = "SELECT [Download].[Quality] ,[Download].[Audio] ,[Download].[Format] ,[Download].[Size] ,[Download].[SeasonTV] ,[Download].[EpisodeTV] ,[Download].[DownloadText] FROM [dbo].[Watch_Download] " +
                "INNER JOIN [Download] ON [Watch_Download].[idDownload] = [Download].[idDownload] " +
                "WHERE idWatch = " + idWatch.ToString();
            using (System.Data.SqlClient.SqlDataReader reader = CommandSelect3.ExecuteReader())
            {
                while (reader.Read())
                {
                    DownloadData downloadData = new DownloadData();
                    downloadData.Audio = reader["Audio"].ToString();
                    downloadData.Format = reader["Format"].ToString();
                    downloadData.Size = reader["Size"].ToString();
                    downloadData.SeasonTV = (int)reader["SeasonTV"];
                    downloadData.EpisodeTV = reader["EpisodeTV"].ToString();
                    downloadData.DownloadText = reader["DownloadText"].ToString();
                    watch.Downloads.Add(downloadData);
                }
            }
            Connection.Disconnect();

            return watch;
        }
        public SearchWatch[] SearchWatch(string query)
        {
            System.Collections.Generic.List<SearchWatch> searchWatches = new System.Collections.Generic.List<SearchWatch>();
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = $"SELECT idWatch, Title, PosterPicture FROM [dbo].[Watch] " +
                $"WHERE Title like '%{query}%' OR TitleOriginal like '%{query}%'";
            sqlCommand.Connection = Connection.Connect();
            using (System.Data.SqlClient.SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    var search = new SearchWatch();
                    search.ID = Convert.ToInt32(reader["idWatch"].ToString());
                    search.Title = reader["Title"].ToString();
                    search.PosterPicture = reader["PosterPicture"].ToString();
                    searchWatches.Add(search);
                }
            }
            Connection.Disconnect();
            return searchWatches.ToArray();
        }
        private Watch.TypeWatch GetType(bool IsMovie)
        {
            return (IsMovie) ? Watch.TypeWatch.Movie : Watch.TypeWatch.Series;
        }
    }
}
