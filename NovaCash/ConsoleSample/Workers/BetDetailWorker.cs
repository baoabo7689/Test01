using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NovaCash.Sportsbook.Clients.Criteria;
using NovaCash.Sportsbook.Clients.Repositories;
using NovaCash.Sportsbook.Clients.Services;

namespace ConsoleSample.Workers
{
    public class BetDetailWorker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var service = new SportsbookService();

                var criteria = new InsertBetDetailBatchCriteria
                {
                    BetDetailResult = service.DoGetBetDetail().GetAwaiter().GetResult()
                };

                var repository = new BetDetailRepository();
                repository.InsertBetDetailBatch(criteria);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}