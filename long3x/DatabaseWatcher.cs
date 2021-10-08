using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using long3x.Data.Interfaces;
using long3x.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace long3x
{
    public class DatabaseWatcher: IHostedService, IDisposable
    {
        private readonly ILogger<DatabaseWatcher> logger;
        private readonly ISignalRepository signalRepository;
        private readonly IEnumerable<ICustomObserver> customObservers;

        private Timer timer;
        private readonly int pollsPeriod;
        private DateTime actualLastDate;

        public DatabaseWatcher(
            ILogger<DatabaseWatcher> logger,
            IConfiguration configuration,
            ISignalRepository signalRepository,
            IEnumerable<ICustomObserver> customObservers)
        {
            this.logger = logger;
            this.signalRepository = signalRepository;
            this.customObservers = customObservers;
            pollsPeriod = Int16.Parse(configuration["DatabasePollsPeriod"]);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("DatabaseWatcher is running.");
            actualLastDate = signalRepository.GetLastOperationDate();
            timer = new Timer(CheckDatabase, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(pollsPeriod));

            return Task.CompletedTask;
        }

        private void CheckDatabase(object state)
        {
            logger.LogInformation("[DatabaseWatcher] Check database changes");
            var currentLastDate = signalRepository.GetLastOperationDate();
            if (currentLastDate > actualLastDate)
            {
                actualLastDate = currentLastDate;
                logger.LogInformation("[DatabaseWatcher] Database change detected");
                foreach (var observer in customObservers)
                {
                    observer.Update();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Timed Hosted Service is stopping.");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
