using System;
using System.Collections.Generic;
using long3x.Data.Entities;
using long3x.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace long3x.Data.Repositories
{
    public class SignalRepository: BaseRepository, ISignalRepository
    {
        public SignalRepository(
            ILogger<SignalRepository> logger,
            IDatabaseConnectionHelper databaseConnectionHelper) : base(logger, databaseConnectionHelper)
        {
        }

        public IEnumerable<SignalEntity> GetAllSignals()
        {
            return ExecuteQuery<SignalEntity>("SELECT * FROM signals");
        }

        public DateTime GetLastOperationDate()
        {
            return ExecuteScalar<DateTime>("SELECT (last_date) FROM last_operation_date");
        }
    }
}
