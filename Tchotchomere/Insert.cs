using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tchotchomere
{
    class Commands
    {
        Connection Connection = new Connection();
        public int InsertWatch(Watch watch, int idDownload)
        {
            try
            {
                System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
                sqlCommand.CommandText = "INSERT INTO [dbo].[Watch] ([Title],[TitleOriginal],[Duration],[Synopsis],[idDownload],[IDTheMovieDB],[IDIMdb],[Genre],[PosterPicture],[BackdropPicture],[Date],[IsMovie]) " +
                    "VALUES(@Title,@TitleOriginal,@Duration,@Synopsis,@idDownload,@IDTheMovieDB,@IDIMdb,@Genre,@PosterPicture,@BackdropPicture,@Date,@IsMovie)";
                sqlCommand.Parameters.AddWithValue("@Title", watch.Title);
                sqlCommand.Parameters.AddWithValue("@TitleOriginal", watch.TitleOriginal);
                sqlCommand.Parameters.AddWithValue("@Duration", watch.Duration);
                sqlCommand.Parameters.AddWithValue("@Synopsis", watch.Synopsis);
                sqlCommand.Parameters.AddWithValue("@idDownload", idDownload);
                sqlCommand.Parameters.AddWithValue("@IDTheMovieDB", watch.IDTheMovieDB);
                sqlCommand.Parameters.AddWithValue("@IDIMDb", watch.IDIMDb);
                sqlCommand.Parameters.AddWithValue("@Genre", watch.Genre);
                sqlCommand.Parameters.AddWithValue("@PosterPicture", watch.PosterPicture);
                sqlCommand.Parameters.AddWithValue("@BackdropPicture", watch.BackdropPicture);
                sqlCommand.Parameters.AddWithValue("@Date", watch.Date);
                sqlCommand.Parameters.AddWithValue("@IsMovie", Convert.ToBoolean((int)watch.Type));
                System.Data.SqlClient.SqlConnection connection = Connection.Connect();
                sqlCommand.Connection = connection;
                sqlCommand.ExecuteNonQuery();

                System.Data.SqlClient.SqlCommand CommandSelect = new System.Data.SqlClient.SqlCommand();
                CommandSelect.Connection = connection;
                CommandSelect.CommandText = "SELECT TOP 1 [idWatch] FROM [dbo].[Download] ORDER BY ID DESC";
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
                //INSERT watch_download
                foreach (var download in watch.Downloads)
                {
                    int _idDownload = InsertDownload(connection, download);
                    InsertWatchDownload(connection, idWatch, _idDownload);

                }
                Connection.Disconnect();
                return idWatch;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int InsertWatch(Watch watch, int idDownload, int idSubtitle)
        {
            try
            {
                System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
                sqlCommand.CommandText = "INSERT INTO [dbo].[Watch] ([Title],[TitleOriginal] ,[Duration],[Synopsis],[idDownload],[IDTheMovieDB],[IDIMdb],[Genre],[PosterPicture],[BackdropPicture],[Date],[idSubtitle],[IsMovie]) " +
                    "VALUES (@Title,@TitleOriginal,@Duration,@Synopsis,@idDownload,@IDTheMovieDB,@IDIMdb,@Genre,@PosterPicture,@BackdropPicture,@Date,@idSubtitle,@IsMovie)";
                sqlCommand.Parameters.AddWithValue("@Title", watch.Title);
                sqlCommand.Parameters.AddWithValue("@TitleOriginal", watch.TitleOriginal);
                sqlCommand.Parameters.AddWithValue("@Duration", watch.Duration);
                sqlCommand.Parameters.AddWithValue("@Synopsis", watch.Synopsis);
                sqlCommand.Parameters.AddWithValue("@idDownload", idDownload);
                sqlCommand.Parameters.AddWithValue("@IDTheMovieDB", watch.IDTheMovieDB);
                sqlCommand.Parameters.AddWithValue("@IDIMDb", watch.IDIMDb);
                sqlCommand.Parameters.AddWithValue("@Genre", watch.Genre);
                sqlCommand.Parameters.AddWithValue("@PosterPicture", watch.PosterPicture);
                sqlCommand.Parameters.AddWithValue("@BackdropPicture", watch.BackdropPicture);
                sqlCommand.Parameters.AddWithValue("@Date", watch.Date);
                sqlCommand.Parameters.AddWithValue("@idSubtitle", idSubtitle);
                sqlCommand.Parameters.AddWithValue("@IsMovie", Convert.ToBoolean((int)watch.Type));
                System.Data.SqlClient.SqlConnection connection = Connection.Connect();
                sqlCommand.Connection = connection;
                sqlCommand.ExecuteNonQuery();

                System.Data.SqlClient.SqlCommand CommandSelect = new System.Data.SqlClient.SqlCommand();
                CommandSelect.Connection = connection;
                CommandSelect.CommandText = "SELECT TOP 1 [idWatch] FROM [dbo].[Download] ORDER BY ID DESC";
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
                //INSERT watch_download
                foreach (var download in watch.Downloads)
                {
                    int _idDownload = InsertDownload(connection, download);
                    InsertWatchDownload(connection, idWatch, _idDownload);
                }
                //INSERT watch_subtitle
                if (watch.Subtitles.Count > 0)
                    foreach (var subtitle in watch.Subtitles)
                    {
                        int _idSubtitle = InsertSubtitle(connection, subtitle);
                        InsertWatchSubtitle(connection, idWatch, _idSubtitle);
                    }
                return idWatch;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int InsertDownload(System.Data.SqlClient.SqlConnection connection, DownloadData DownloadData)
        {
            try
            {
                //SELECT TOP 1 * FROM Table ORDER BY ID DESC
                System.Data.SqlClient.SqlCommand CommandInsert = new System.Data.SqlClient.SqlCommand();
                CommandInsert.CommandText = "INSERT INTO [dbo].[Download] ([Quality],[Audio],[Format],[Size],[DownloadText]) " +
                    "VALUES (@Quality,@Audio,@Format,@Size,@DownloadText)";
                CommandInsert.Parameters.AddWithValue("@Quality", DownloadData.Quality);
                CommandInsert.Parameters.AddWithValue("@Audio", DownloadData.Audio);
                CommandInsert.Parameters.AddWithValue("@Format", DownloadData.Format);
                CommandInsert.Parameters.AddWithValue("@Size", DownloadData.Size);
                CommandInsert.Parameters.AddWithValue("@DownloadText", DownloadData.DownloadText);

                CommandInsert.Connection = connection;
                CommandInsert.ExecuteNonQuery();

                System.Data.SqlClient.SqlCommand CommandSelect = new System.Data.SqlClient.SqlCommand();
                CommandSelect.Connection = connection;
                CommandSelect.CommandText = "SELECT TOP 1 [idDownload] FROM [dbo].[Download] ORDER BY ID DESC";
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
        public int InsertSubtitle(System.Data.SqlClient.SqlConnection connection, Subtitle Subtitle)
        {
            try
            {
                //SELECT TOP 1 * FROM Table ORDER BY ID DESC
                System.Data.SqlClient.SqlCommand CommandInsert = new System.Data.SqlClient.SqlCommand();
                CommandInsert.CommandText = "INSERT INTO [dbo].[Subtitle] ([Subtitle] ,[DownloadText]) " +
                    "VALUES (@Subtitle,@DownloadText)";
                CommandInsert.Parameters.AddWithValue("@Subtitle", Subtitle.Lang);
                CommandInsert.Parameters.AddWithValue("@DownloadText", Subtitle.DownloadText);

                CommandInsert.Connection = connection;
                CommandInsert.ExecuteNonQuery();

                System.Data.SqlClient.SqlCommand CommandSelect = new System.Data.SqlClient.SqlCommand();
                CommandSelect.Connection = connection;
                CommandSelect.CommandText = "SELECT TOP 1 [idDownload] FROM [dbo].[Download] ORDER BY ID DESC";
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
        private void InsertWatchDownload(System.Data.SqlClient.SqlConnection connection, int idWatch, int idDownload)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandText = "INSERT INTO [dbo].[Watch_Download] ([idWatch] ,[idDownload]) VALUES (@idWatch,@idDownload)";
            sqlCommand.Parameters.AddWithValue("@idWatch", idWatch.ToString());
            sqlCommand.Parameters.AddWithValue("@idDownload", idDownload.ToString());
            sqlCommand.Connection = connection;
            sqlCommand.ExecuteNonQuery();

        }
        private void InsertWatchSubtitle(System.Data.SqlClient.SqlConnection connection, int idWatch, int idSubtitle)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandText = "INSERT INTO [dbo].[Watch_Subtitle] ([idWatch] ,[idSubtitle]) VALUES (@idWatch,@idSubtitle) ";
            sqlCommand.Parameters.AddWithValue("@idWatch", idWatch.ToString());
            sqlCommand.Parameters.AddWithValue("@idSubtitle", idSubtitle.ToString());
            sqlCommand.Connection = connection;
            sqlCommand.ExecuteNonQuery();

        }

    }
}
