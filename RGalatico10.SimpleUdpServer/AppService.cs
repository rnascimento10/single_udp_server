using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RGalatico10.SimpleUdpServer.Core.Common.Ioc;
using RGalatico10.SimpleUdpServer.Core.Common.Listener;
using System.ServiceProcess;

namespace RGalatico10.SimpleUdpServer
{
    public class AppService : ServiceBase
    {
        private readonly ServiceProvider _serviceProvider;

        public AppService()
        {
            ServiceName = "UDP Listener";

            //Startup Services
            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appSettings.json")
                                .Build();

            var services = new ServiceCollection();

            //Configurando Serilog
            services.AddSerilogServices(configuration);
            //Configuração para obter os valores de configurações em arquivo appSettings.json via container de DI (Ver o padrão IOptions)
            services.AddConfiguration(configuration);
            //Configurando serviços no Container
            services.AddServices();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStart(string[] args)
        {
            _serviceProvider.GetService<IListener>().Listen();
           
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

    }
}
