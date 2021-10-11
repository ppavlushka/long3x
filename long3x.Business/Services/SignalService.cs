using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using long3x.Business.Interfaces;
using long3x.Data.Interfaces;
using long3x.ViewModels;

namespace long3x.Business.Services
{
    public class SignalService: ISignalService
    {
        private readonly ISignalRepository signalRepository;
        private readonly IMapper mapper;
        private readonly IBinanceApiHandler binanceApiHandler;
        private readonly ICurrentCoinsHelper currentCoinsHelper;

        public SignalService(ISignalRepository signalRepository, IMapper mapper, IBinanceApiHandler binanceApiHandler, ICurrentCoinsHelper currentCoinsHelper)
        {
            this.signalRepository = signalRepository;
            this.mapper = mapper;
            this.binanceApiHandler = binanceApiHandler;
            this.currentCoinsHelper = currentCoinsHelper;
        }

        public IEnumerable<SignalViewModel> GetSignalViewModels()
        {
            var entities = signalRepository.GetAllSignals().Reverse();
            var viewModel = mapper.Map<IList<SignalViewModel>>(entities);
            var prices = GetPricesForCurrentSignals();

            foreach (var model in viewModel)
            {
                if (prices.ContainsKey(model.FullCoinDescription))
                {
                    model.Price = prices[model.FullCoinDescription];
                }
            }
            return viewModel;
        }

        public Dictionary<string, decimal> GetPricesForCurrentSignals()
        {
            var distinctSignalCoins = currentCoinsHelper.GetCurrentCoins();
            return binanceApiHandler.GetPriceDictionary(distinctSignalCoins);
        }
    }
}
