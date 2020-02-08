using Microsoft.Extensions.Configuration;
using System.ServiceProcess;

namespace RGalatico10.SimpleUdpServer
{
    public class AppService : ServiceBase
    {
        public AppService()
        {
            ServiceName = "UDP Listener";

            //Startup Services
            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appSettings.json")
                                .Build();

        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

    }
}
