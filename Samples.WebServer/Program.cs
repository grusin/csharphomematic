using csharpmatic.Automation;
using csharpmatic.Automation.Alarm;
using csharpmatic.Automation.RestApi;
using csharpmatic.Generic;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace Samples.WebServer
{
    class Program
    {
        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            var dm = new DeviceManager("192.168.1.200");
            
            //register automations
            var alarmAutomation = new AlarmAutomation(dm, AutomationNames.AlarmAutomation);

            //register web server
            var server = new Unosquare.Labs.EmbedIO.WebServer(4000);

            //serve .js mobile site
            server.RegisterModule(new StaticFilesModule(@"C:\Users\G\npm\homeui\build"));
            server.Module<StaticFilesModule>().UseRamCache = true;
            server.Module<StaticFilesModule>().DefaultExtension = ".html";

            server.RegisterModule(new WebApiModule());
            server.Module<CorsModule>();
            RoomController.DeviceManager = dm;
            server.Module<WebApiModule>().RegisterController<RoomController>();


            server.RunAsync();
            
            for(;;)
            {
                new ManualResetEvent(false).WaitOne(200);
            }
        }
    }
}
