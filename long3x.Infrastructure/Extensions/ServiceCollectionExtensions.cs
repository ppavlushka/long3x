using AutoMapper;
using long3x.Business.Interfaces;
using long3x.Business.Services;
using long3x.Common.ConfigurationModels;
using long3x.Data.ApiHandler;
using long3x.Data.Helpers;
using long3x.Data.Interfaces;
using long3x.Data.Repositories;
using long3x.Mappings;
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
            serviceCollection.AddTransient<ISignalService, SignalService>();
            serviceCollection.AddTransient<IBinanceApiHandler, BinanceApiHandler>();
            serviceCollection.AddSingleton<ICustomObserversHelper, CustomObserversHelper > ();
            serviceCollection.AddSingleton<ICurrentCoinsHelper, CurrentCoinsHelper>();
        }

        public static void AddAutoMapper(this IServiceCollection serviceCollection)
        {
            var mapperConfig = new MapperConfiguration(x => x.AddProfile(new SignalMappingProfile()));
            var mapper = mapperConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);
        }

        public static void RegisterConfigurationSections(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.Configure<SshConnectionSettings>(configuration.GetSection("SshConnectionSettings"));
            serviceCollection.Configure<DatabaseConnectionSettings>(configuration.GetSection("DatabaseConnectionSettings"));
            serviceCollection.Configure<ApiConnectionSettings>(configuration.GetSection("ApiConnectionSettings"));
        }
    }
}
