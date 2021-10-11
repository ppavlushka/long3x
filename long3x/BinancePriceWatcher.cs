using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using long3x.Business.Interfaces;
using long3x.Common.ConfigurationModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Threading.Timer;
using Microsoft.AspNetCore.SignalR;
using long3x.Hubs;
using Microsoft.Extensions.Options;

namespace long3x
{
    public class BinancePriceWatcher: IHostedService, IDisposable
    {
        private readonly ISignalService signalService;
        private readonly ILogger<BinancePriceWatcher> logger;
        private Timer timer;
        private readonly IHubContext<SignalsUpdatesHub> hubContext;
        private readonly ApiConnectionSettings apiConnectionSettings;

        public BinancePriceWatcher(
            ILogger<BinancePriceWatcher> logger,
            ISignalService signalService,
            IHubContext<SignalsUpdatesHub> hubContext,
            IOptions<ApiConnectionSettings> apiConnectionSettings)
        {
            this.logger = logger;
            this.signalService = signalService;
            this.hubContext = hubContext;
            this.apiConnectionSettings = apiConnectionSettings.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("BinancePriceWatcher is running.");
            timer = new Timer(UpdatePrice, null, TimeSpan.Zero, TimeSpan.FromSeconds(apiConnectionSettings.RequestInterval));

            return Task.CompletedTask;
        }

        public void UpdatePrice(object state)
        {
            var prices = signalService.GetPricesForCurrentSignals();
            hubContext.Clients.All.SendAsync("UpdatePrices", prices);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("BinancePriceWatcher is stopping.");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
