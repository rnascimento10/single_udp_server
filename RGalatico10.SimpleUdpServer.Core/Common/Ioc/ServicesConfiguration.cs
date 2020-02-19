using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RGalatico10.SimpleUdpServer.Core.Common.Listener;
using Serilog;
using System;

namespace RGalatico10.SimpleUdpServer.Core.Common.Ioc
{
    public static class ServicesConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IListener, UdpListener>();
        }

        public static IServiceCollection AddSerilogServices(this IServiceCollection services, IConfigurationRoot configuration)
        {

            var configurationLog = new LoggerConfiguration()
                .ReadFrom
                .Configuration(configuration);

            Log.Logger = configurationLog.CreateLogger();

            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
            
            return services.AddSingleton(Log.Logger);

           
        }

        public static void AddConfiguration(this IServiceCollection services, IConfigurationRoot configuration)
        {

            services.AddOptions();
            services.Configure<UdpListenerConfiguration>(configuration.GetSection("UdpListenerConfiguration"));
           
        }




    }
}
