using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ConsoleSample.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NovaCash.Sportsbook.Clients.Configurations;

namespace ConsoleSample
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            AppSettings.Load();

            var builder = Host.CreateDefaultBuilder(args).ConfigureServices(ConfigureServices);
            await builder.RunConsoleAsync();
        }

        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddHostedService<BetDetailWorker>();
        }

        public static async Task StartService(string[] args, IHostBuilder builder)
        {
            var isService = !Debugger.IsAttached && !args.Contains("--console");
            if (isService)
            {
                await builder.RunAsServiceAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }
        }
    }
}