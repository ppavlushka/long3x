using System;
using System.Collections.Generic;
using System.Drawing;
using long3x.Data.Entities;
using long3x.Data.Interfaces;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509;

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
            return ExecuteQuery<SignalEntity>("SELECT signal_id, channel_id, upper(coin1) as coin1, upper(coin2) as coin2, leverage, long_short, risk, entry_zone_min, entry_zone_max, targets, stop_loss, trading_type, additional_info, ts FROM long3x_db.signals");
        }

        public IEnumerable<string> GetDistinctSignalCoins()
        {
            return ExecuteQuery<string>("SELECT DISTINCT UPPER(CONCAT(coin1, coin2)) FROM signals");
        }
    }
}
