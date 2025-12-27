using MySql.Data.MySqlClient;
using System.Data;

namespace LibraryManager.Repositories
{
    public abstract class BaseRepository
    {
        protected string _connectionString;

        public BaseRepository()
        {
            _connectionString = AppConfig.ConnectionString;
        }

        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
