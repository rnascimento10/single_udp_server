using System;
using System.ServiceProcess;

namespace RGalatico10.SimpleUdpServer
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var appService = new AppService())
            {
                ServiceBase.Run(appService);
            }

        }

    }
}
