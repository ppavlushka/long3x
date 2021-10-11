using System.Collections.Generic;

namespace long3x.Data.Interfaces
{
    public interface ICurrentCoinsHelper
    {
        IEnumerable<string> GetCurrentCoins();
    }
}
