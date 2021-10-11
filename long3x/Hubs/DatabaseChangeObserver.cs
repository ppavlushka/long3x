using long3x.Business.Interfaces;
using long3x.Data.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace long3x.Hubs
{
    public class DatabaseChangeObserver : ICustomObserver
    {
        private readonly IHubContext<SignalsUpdatesHub> hubContext;
        private readonly ISignalService signalService;

        public DatabaseChangeObserver(IHubContext<SignalsUpdatesHub> hubContext, ISignalService signalService)
        {
            this.hubContext = hubContext;
            this.signalService = signalService;
        }

        public void Update()
        {
            var signals = signalService.GetSignalViewModels();
            var json = JsonConvert.SerializeObject(signals);

            hubContext.Clients.All.SendAsync("UpdateSignals", json);
        }
    }
}
