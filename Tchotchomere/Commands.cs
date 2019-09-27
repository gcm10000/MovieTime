using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tchotchomere
{
    class Commands
    {
        Connection Connection = new Connection();
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
            sqlCommand.CommandText = $"SELECT COUNT(*) FROM [movietime_database].[dbo].[Watch] WHERE Title = '{Title}' AND TitleOriginal = '{TitleOriginal}'";
            //sqlCommand.Parameters.AddWithValue("@Title", Title);
            //sqlCommand.Parameters.AddWithValue("@TitleOriginal", TitleOriginal);
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
            sqlCommand.CommandText = $"SELECT idWatch FROM [movietime_database].[dbo].[Watch] WHERE Title = '{Title}' AND TitleOriginal = '{TitleOriginal}'";
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

    }
}
