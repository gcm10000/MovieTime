using System.Data.SqlClient;

namespace LibraryShared
{
    class Connection
    {
        SqlConnection conn = new SqlConnection();

        public Connection()
        {
            conn.ConnectionString = @"Data Source=GABRIEL-PC\SQLEXPRESS;Initial Catalog=movietime_database;Integrated Security=True";
        }

        public Connection(string ConnectionString)
        {
            conn.ConnectionString = ConnectionString;
        }

        public SqlConnection Connect()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            return conn;
        }
        public void Disconnect()
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }
}
