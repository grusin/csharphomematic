﻿using csharpmatic.Automation;
using csharpmatic.Automation.Alarm;
using csharpmatic.Automation.RestApi;
using csharpmatic.Generic;
using csharpmatic.Notify;
using log4net;
using log4net.Config;
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
    class SampleWebServer
    {
        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            BasicConfigurator.Configure();

            var dm = new DeviceManager("192.168.1.200");

            //register notifcation service
            Slack s = Slack.TryFromCCU(dm);
            if(s != null)
                dm.RegisterNotificationService(s);
                                  
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

            AlarmControler.AlarmAutomation = alarmAutomation;
            server.Module<WebApiModule>().RegisterController<AlarmControler>();
            
            server.RunAsync();
            
            for(;;)
            {
                dm.Work();
                new ManualResetEvent(false).WaitOne(200);
            }
        }       
    }
}