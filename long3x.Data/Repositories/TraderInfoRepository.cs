using System.Collections.Generic;
using long3x.Common.ConfigurationModels;
using long3x.Data.Entities;
using long3x.Data.Interfaces;
using Microsoft.Extensions.Options;

namespace long3x.Data.Repositories
{
    public class TraderInfoRepository: BaseRepository, ITraderInfoRepository
    {
        public TraderInfoRepository(
            IOptions<DatabaseConnectionSettings> databaseOptions,
            IOptions<SshConnectionSettings> sshOptions) : base(databaseOptions, sshOptions)
        {
        }

        public IEnumerable<TraderInfoEntity> GetAllTraderInfo()
        {
            return ExecuteQuery<TraderInfoEntity>("SELECT * FROM signals");
        }
    }
}
