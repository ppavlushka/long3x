using Microsoft.Extensions.Logging;
using Renci.SshNet;
using System.Collections.Generic;
using System;
using long3x.Common.ConfigurationModels;
using long3x.Data.Interfaces;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace long3x.Data.Services
{
    public class DatabaseConnectionHelper: IDatabaseConnectionHelper
    {
        private readonly ILogger<DatabaseConnectionHelper> logger;
        private readonly SshConnectionSettings sshConnectionSettings;
        private readonly DatabaseConnectionSettings databaseConnectionSettings;

        public DatabaseConnectionHelper(ILogger<DatabaseConnectionHelper> logger, IOptions<SshConnectionSettings> sshConnectionSettings, IOptions<DatabaseConnectionSettings> databaseConnectionSettings)
        {
            this.logger = logger;
            this.databaseConnectionSettings = databaseConnectionSettings.Value;
            this.sshConnectionSettings = sshConnectionSettings.Value;
        }

        public T Execute<T>(Func<MySqlConnection, T> action)
        {
            var sshTuple = GetSshClient();
            using (sshTuple.SshClient)
            {
                var connectionStringBuilder = new MySqlConnectionStringBuilder
                {
                    Server = "127.0.0.1",
                    Port = sshTuple.Port,
                    UserID = databaseConnectionSettings.UserId,
                    Password = databaseConnectionSettings.Password,
                    Database = databaseConnectionSettings.Database
                };
                using var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
                try
                {
                    connection.Open();
                    var result = action(connection);
                    return result;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private (SshClient SshClient, uint Port) GetSshClient()
        {
            var authenticationMethods = new List<AuthenticationMethod>
            {
                new PrivateKeyAuthenticationMethod(sshConnectionSettings.UserName,
                    new PrivateKeyFile(sshConnectionSettings.SshKeyFile))
            };


            var sshClient = new SshClient(new ConnectionInfo(sshConnectionSettings.HostName, 22, sshConnectionSettings.UserName, authenticationMethods.ToArray()));

            sshClient.Connect();
            logger.LogInformation("[SSH] Ssh connection established");

            var forwardedPort = new ForwardedPortLocal("127.0.0.1",databaseConnectionSettings.DatabaseServer, (uint)3306);
            sshClient.AddForwardedPort(forwardedPort);
            forwardedPort.Start();

            return (sshClient, forwardedPort.BoundPort);
        }
    }
}
