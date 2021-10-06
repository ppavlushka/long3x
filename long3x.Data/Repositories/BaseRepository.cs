using System;
using System.Collections.Generic;
using Dapper;
using long3x.Common.ConfigurationModels;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Renci.SshNet;

namespace long3x.Data.Repositories
{
    public abstract class BaseRepository
    {
        private readonly DatabaseConnectionSettings databaseOptions;
        private readonly SshConnectionSettings sshConnectionOptions;
        //private readonly ILogger logger;
       
        protected BaseRepository(IOptions<DatabaseConnectionSettings> databaseOptions, IOptions<SshConnectionSettings> sshConnectionOptions)
        {
            this.databaseOptions = databaseOptions.Value;
            this.sshConnectionOptions = sshConnectionOptions.Value;
        }

        protected IEnumerable<T> ExecuteQuery<T>(string query)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            var (sshClient, localPort) = ConnectSsh(
                sshConnectionOptions.HostName,
                sshConnectionOptions.UserName,
                sshConnectionOptions.SshKeyFile,
                databaseServer: databaseOptions.DatabaseServer);
            using (sshClient)
            {
                var connectionStringBuilder = new MySqlConnectionStringBuilder
                {
                    Server = "127.0.0.1",
                    Port = localPort,
                    UserID = databaseOptions.UserId,
                    Password = databaseOptions.Password,
                    Database = databaseOptions.Database
                };
                using var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
                try
                {
                    connection.Open();
                    //logger.LogInformation($"[SQL] Executing query: {query}");
                    var result = connection.Query<T>(query);

                    return result;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private (SshClient SshClient, uint Port) ConnectSsh(string sshHostName, string sshUserName,
            string sshKeyFile = null, int sshPort = 22, string databaseServer = "localhost", int databasePort = 3306)
        {
            if (string.IsNullOrEmpty(sshHostName))
                throw new ArgumentException($"{nameof(sshHostName)} must be specified.", nameof(sshHostName));
            if (string.IsNullOrEmpty(databaseServer))
                throw new ArgumentException($"{nameof(databaseServer)} must be specified.", nameof(databaseServer));

            var authenticationMethods = new List<AuthenticationMethod>();
            if (!string.IsNullOrEmpty(sshKeyFile))
            {
                authenticationMethods.Add(new PrivateKeyAuthenticationMethod(sshUserName, new PrivateKeyFile(sshKeyFile)));
            }
            var sshClient = new SshClient(new ConnectionInfo(sshHostName, sshPort, sshUserName, authenticationMethods.ToArray()));
            sshClient.Connect();

            var forwardedPort = new ForwardedPortLocal("127.0.0.1", databaseServer, (uint)databasePort);
            sshClient.AddForwardedPort(forwardedPort);
            forwardedPort.Start();

            return (sshClient, forwardedPort.BoundPort);
        }
	}
}
