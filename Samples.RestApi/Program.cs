using csharpmatic.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace Samples.RestApi
{
    class Program
    {
        public static DeviceManager DeviceManager { get; set; }

        static void Main(string[] args)
        {
            DeviceManager = new DeviceManager("192.168.1.200");                   

            //start webserver listening on 
            var server = new WebServer("http://localhost:9696/", RoutingStrategy.Regex);
            server.RegisterModule(new WebApiModule());
            server.Module<CorsModule>();
            server.Module<WebApiModule>().RegisterController<RoomController.RoomController>();
            server.RunAsync();

            //do homemmatic logic 
            for (;;)
            {
                DeviceManager.Refresh();
                new ManualResetEvent(false).WaitOne(10000);
            }
        }
    }
}
