using System.Collections.Generic;
using long3x.Data.Entities;

namespace long3x.Data.Interfaces
{
    public interface ITraderInfoRepository
    {
        IEnumerable<TraderInfoEntity> GetAllTraderInfo();
    }
}
