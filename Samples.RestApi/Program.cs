using csharpmatic.Generic;
using csharpmatic.RestApi;
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
        static void Main(string[] args)
        {
            DeviceManager dm = new DeviceManager("192.168.1.200");                  
                     
            //start webserver listening on 
            var server = new WebServer("http://localhost:9696/", RoutingStrategy.Regex);
            server.RegisterModule(new WebApiModule());
            server.Module<CorsModule>();
            RoomController.DeviceManager = dm;
            server.Module<WebApiModule>().RegisterController<RoomController>();
            server.RunAsync();

            //do homemmatic logic 
            for (;;)
            {
                dm.Refresh();
                new ManualResetEvent(false).WaitOne(1000);
            }
        }
    }
}
