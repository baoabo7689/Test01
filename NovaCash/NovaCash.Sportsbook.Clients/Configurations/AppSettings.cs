using System.IO;
using System.Linq;
using Fanex.Data;
using Fanex.Data.MySql;
using Microsoft.Extensions.Configuration;

namespace NovaCash.Sportsbook.Clients.Configurations
{
    public class AppSettings
    {
        public static string APIURL = "http://tsa.nova88.net/api/";
        public static string APIVendorId = "aakx8ncolf";
        public static int Currency = 20;
        public static string HangfireConnection = string.Empty;
        public static string BetDetailConnection = string.Empty;

        public static void Load()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

            APIURL = configuration.GetValue<string>("APIURL");
            APIVendorId = configuration.GetValue<string>("APIVendorId");
            Currency = configuration.GetValue<int>("Currency");
            HangfireConnection = configuration.GetValue<string>("HangfireConnection");
            BetDetailConnection = configuration.GetValue<string>("BetDetailConnection");

            var connections = configuration
                 .GetSection("ConnectionStrings")
                 .GetChildren()
                 .ToDictionary(connection => connection.Key,
                               connection => new ConnectionConfiguration(connection.Key, connection.Value));

            DbSettingProviderManager
                .StartNewSession()
                .Use(connections)
                .WithMySql(resourcePath: configuration["StoreProceduresPath"])
                .Run();
        }
    }
}