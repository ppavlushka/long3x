using long3x.Common.ConfigurationModels;
using long3x.Data.Helpers;
using long3x.Data.Interfaces;
using long3x.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace long3x.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<ISignalRepository, SignalRepository>();
            serviceCollection.AddTransient<IDatabaseConnectionHelper, DatabaseConnectionHelper>();
            serviceCollection.AddSingleton<ICustomObserversHelper, CustomObserversHelper > ();
        }

        public static void RegisterConfigurationSections(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.Configure<SshConnectionSettings>(configuration.GetSection("SshConnectionSettings"));
            serviceCollection.Configure<DatabaseConnectionSettings>(
                configuration.GetSection("DatabaseConnectionSettings"));
        }
    }
}
