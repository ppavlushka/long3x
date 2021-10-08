using System;
using System.Collections.Generic;
using long3x.Data.Entities;

namespace long3x.Data.Interfaces
{
    public interface ISignalRepository
    {
        IEnumerable<SignalEntity> GetAllSignals();

        DateTime GetLastOperationDate();
    }
}
