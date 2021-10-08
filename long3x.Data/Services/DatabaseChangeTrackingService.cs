using long3x.Data.Interfaces;
using System;
using Microsoft.Extensions.Logging;

namespace long3x.Data.Services
{
    public class DatabaseChangeTrackingService: IDatabaseChangeTrackingService
    {
        private readonly int trackingPeriod;
        private readonly ILogger<DatabaseChangeTrackingService> logger;
        private readonly ISignalRepository signalRepository;

        public DatabaseChangeTrackingService(ILogger<DatabaseChangeTrackingService> logger, ISignalRepository signalRepository)
        {
            this.trackingPeriod = 5;
            this.logger = logger;
            this.signalRepository = signalRepository;
        }

        public void Run()
        {

        }
    }
}
