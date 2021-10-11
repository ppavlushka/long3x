using System.Collections.Generic;

namespace long3x.Data.Interfaces
{
    public interface IBinanceApiHandler
    {
        Dictionary<string, decimal> GetPriceDictionary(IEnumerable<string> coins);
    }
}
