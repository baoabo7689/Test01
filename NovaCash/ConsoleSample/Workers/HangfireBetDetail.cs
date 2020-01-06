using System;
using System.Transactions;
using Hangfire;
using Hangfire.MySql;
using NovaCash.Sportsbook.Clients.Configurations;
using NovaCash.Sportsbook.Clients.Criteria;
using NovaCash.Sportsbook.Clients.Repositories;
using NovaCash.Sportsbook.Clients.Services;

namespace ConsoleSample.Workers
{
    public static class HangfireBetDetail
    {
        public static void Run()
        {
            var storage = new MySqlStorage(AppSettings.HangfireConnection, new MySqlStorageOptions
            {
                TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                QueuePollInterval = TimeSpan.FromSeconds(15),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 50000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
                TablesPrefix = "Hangfire"
            });

            GlobalConfiguration.Configuration.UseStorage(storage);

            using (var server = new BackgroundJobServer())
            {
                RecurringJob.AddOrUpdate(() => GetBetDetails(), Cron.Minutely);
            }
        }

        public static void GetBetDetails()
        {
            var service = new SportsbookService();

            var criteria = new InsertBetDetailBatchCriteria
            {
                BetDetailResult = service.DoGetBetDetail().GetAwaiter().GetResult()
            };

            var repository = new BetDetailRepository();
            repository.InsertBetDetailBatch(criteria);
        }
    }
}