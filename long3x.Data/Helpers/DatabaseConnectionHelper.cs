using System;
using System.Collections.Generic;
using long3x.Common.ConfigurationModels;
using long3x.Data.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Renci.SshNet;

namespace long3x.Data.Helpers
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
            if (sshConnectionSettings.UseSshConnection)
            {
                return ExecuteWithSShConnection(action);
            }

            return ExecuteWithoutSShConnection(action);
        }

        private T ExecuteWithoutSShConnection<T>(Func<MySqlConnection, T> action)
        {
            return Execute(action, databaseConnectionSettings.DatabaseServer, databaseConnectionSettings.DatabasePort);
        }

        private T ExecuteWithSShConnection<T>(Func<MySqlConnection, T> action)
        {
            var sshTuple = GetSshClient();
            using (sshTuple.SshClient)
            {
                return Execute(action, sshConnectionSettings.BoundHost, sshTuple.Port);
            }
        }

        private T Execute<T>(Func<MySqlConnection, T> action, string sqlServer, uint sqlPort)
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder
            {
                Server = sqlServer,
                Port = sqlPort,
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

            //Need to connect to MySql server when MySQL and SSH Server are different
            var forwardedPort = new ForwardedPortLocal(sshConnectionSettings.BoundHost,databaseConnectionSettings.DatabaseServer, databaseConnectionSettings.DatabasePort);
            sshClient.AddForwardedPort(forwardedPort);
            forwardedPort.Start();

            return (sshClient, forwardedPort.BoundPort);
        }
    }
}
