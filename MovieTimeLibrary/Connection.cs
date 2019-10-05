using System.Data.SqlClient;

namespace LibraryShared
{
    public class Connection
    {
        SqlConnection conn = new SqlConnection();

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
