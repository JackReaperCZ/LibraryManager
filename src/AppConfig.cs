using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace LibraryManager
{
    public static class AppConfig
    {
        public static string ConnectionString { get; private set; }
        public static string ImportExportPath { get; private set; }
        public static string DateFormat { get; private set; }

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
