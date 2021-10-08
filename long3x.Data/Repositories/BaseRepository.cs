using System.Collections.Generic;
using Dapper;
using long3x.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace long3x.Data.Repositories
{
    public abstract class BaseRepository
    {
        private readonly ILogger logger;
        private readonly IDatabaseConnectionHelper databaseConnectionHelper;

        protected BaseRepository(ILogger logger, IDatabaseConnectionHelper databaseConnectionHelper)
        {
            this.logger = logger;
            this.databaseConnectionHelper = databaseConnectionHelper;
        }

        protected IEnumerable<T> ExecuteQuery<T>(string query)
        {
            logger.LogInformation($"[SQL] Execute query: {query}");
            var result = databaseConnectionHelper.Execute(connection => connection.Query<T>(query));

            return result;
        }

        protected T ExecuteScalar<T>(string query)
        {
            logger.LogInformation($"[SQL] Execute scalar: {query}");
            var result = databaseConnectionHelper.Execute(connection => connection.ExecuteScalar<T>(query));

            return result;
        }

    }
}
