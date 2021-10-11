using System.Collections.Generic;
using long3x.Data.Interfaces;

namespace long3x.Data.Helpers
{
    //This class is needed to remove the asking the database for every API price request
    public class CurrentCoinsHelper: ICustomObserver, ICurrentCoinsHelper
    {
        private readonly ISignalRepository signalRepository;

        private IEnumerable<string> currentCoins;

        public CurrentCoinsHelper(ISignalRepository signalRepository)
        {
            this.signalRepository = signalRepository;
        }

        public void Update()
        {
            UpdateCurrentCoinsCollection();
        }

        public IEnumerable<string> GetCurrentCoins()
        {
            if (currentCoins == null)
            {
                UpdateCurrentCoinsCollection();
            }

            return currentCoins;
        }

        private void UpdateCurrentCoinsCollection()
        {
            currentCoins = signalRepository.GetDistinctSignalCoins();
        }
    }
}
