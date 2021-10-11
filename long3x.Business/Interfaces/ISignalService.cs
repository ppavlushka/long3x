using System.Collections.Generic;
using long3x.ViewModels;

namespace long3x.Business.Interfaces
{
    public interface ISignalService
    {
        IEnumerable<SignalViewModel> GetSignalViewModels();

        Dictionary<string, decimal> GetPricesForCurrentSignals();
    }
}
