using MySql.Data.MySqlClient;
using System.Data;

namespace LibraryManager.Repositories
{
    /// <summary>
    /// Base class for all repositories, providing database connection management.
    /// </summary>
    public abstract class BaseRepository
    {
        /// <summary>
        /// The connection string used to connect to the database.
        /// </summary>
        protected string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// Retrieves the connection string from the application configuration.
        /// </summary>
        public BaseRepository()
        {
            _connectionString = AppConfig.ConnectionString;
        }

        /// <summary>
        /// Creates and returns a new MySQL database connection.
        /// </summary>
        /// <returns>A new <see cref="MySqlConnection"/> instance.</returns>
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
