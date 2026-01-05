using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace LibraryManager
{
    /// <summary>
    /// Manages application configuration settings.
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        public static string ConnectionString { get; private set; }

        /// <summary>
        /// Gets the default path for import/export operations.
        /// </summary>
        public static string ImportExportPath { get; private set; }

        /// <summary>
        /// Gets the date format string used in the application.
        /// </summary>
        public static string DateFormat { get; private set; }

        /// <summary>
        /// Loads configuration from appsettings.json.
        /// </summary>
        public static void LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            ConnectionString = configuration.GetConnectionString("DefaultConnection");
            ImportExportPath = configuration["AppSettings:ImportExportPath"];
            DateFormat = configuration["AppSettings:DateFormat"];
        }
    }
}
