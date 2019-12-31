using csharpmatic.Generic;
using csharpmatic.Automation.RestApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO.Files;

namespace Samples.RestApi
{
    class SampleRestApi
    {
        static void Main(string[] args)
        {
            DeviceManager dm = new DeviceManager("192.168.1.200");

            //start webserver listening on 
            var server = new WebServer(80);
            server.WithCors();

            RoomController.DeviceManager = dm;
            server.WithWebApi("/api", m => m.WithController<RoomController>());

            server.WithStaticFolder("/", "www", true, m => m.WithContentCaching(true));

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
