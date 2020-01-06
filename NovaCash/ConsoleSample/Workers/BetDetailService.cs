using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NovaCash.Sportsbook.Clients.Criteria;
using NovaCash.Sportsbook.Clients.Repositories;
using NovaCash.Sportsbook.Clients.Services;

namespace ConsoleSample.Workers
{
    public class BetDetailService : IHostedService, IDisposable
    {
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
                (e) => GetBetDetail(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        public void GetBetDetail()
        {
            var service = new SportsbookService();

            var criteria = new InsertBetDetailBatchCriteria
            {
                BetDetailResult = service.DoGetBetDetail().GetAwaiter().GetResult()
            };

            var repository = new BetDetailRepository();
            repository.InsertBetDetailBatch(criteria);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}