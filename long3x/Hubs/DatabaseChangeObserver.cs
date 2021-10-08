using System.Linq;
using long3x.Data.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace long3x.Hubs
{
    public class DatabaseChangeObserver : ICustomObserver
    {
        private readonly IHubContext<SignalsUpdatesHub> hubContext;
        private readonly ISignalRepository signalRepository;

        public DatabaseChangeObserver(IHubContext<SignalsUpdatesHub> hubContext, ISignalRepository signalRepository)
        {
            this.hubContext = hubContext;
            this.signalRepository = signalRepository;
        }

        public void Update()
        {
            var signals = signalRepository.GetAllSignals().Reverse();
            var json = JsonConvert.SerializeObject(signals);

            hubContext.Clients.All.SendAsync("UpdateSignals", json);
        }
    }
}
