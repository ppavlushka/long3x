using long3x.Common.ConfigurationModels;
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
            serviceCollection.AddTransient<ITraderInfoRepository, TraderInfoRepository>();
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
