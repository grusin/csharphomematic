using csharpmatic.Automation;
using csharpmatic.Generic;
using csharpmatic.Interfaces;
using csharpmatic.Automation.RestApi;
using HouseAutomationService.Properties;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using csharpmatic.Notify;
using csharpmatic.Automation.Alarm;
using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO.Files;

namespace HouseAutomationService
{
    class HomeAutomationService
    {
        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            int restartTries = 0;

            for (;;)
            {
                try
                {                   
                    var dm = new DeviceManager(Settings.Default.HomematicServerAddress);

                    LOGGER.Info("Starting external Slack notification service");
                    Slack s = Slack.TryFromCCU(dm);
                    if (s != null)
                        dm.RegisterNotificationService(s);

                    LOGGER.Info("Starting alarm automation");
                    var alarmAutomation = new AlarmAutomation(dm, AutomationNames.AlarmAutomation);

                    LOGGER.Info("Starting humidity automation");                    
                    var humidityAutomation = new ActuatorSensorAutomation<IHumidityControlDevice>(dm, AutomationNames.HumidityAutomation, Function.Humidity, (a, d) => d.Humidity.Value);
                    humidityAutomation.RefencePoint = Settings.Default.HumidityAutomationRefencePoint;
                    humidityAutomation.Hysteresis = Settings.Default.HumidityAutomationHysteresis;
                    humidityAutomation.MaxOnTime = Settings.Default.HumidityAutomationMaxOnTime;
                    humidityAutomation.MinOnTime = Settings.Default.HumidityAutomationMinOnTime;
                    humidityAutomation.MinOffTime = Settings.Default.HumidityAutomationMinOffTime;
 
                    LOGGER.Info("Starting heating automation");                    
                    var heatingAutomation = new ActuatorSensorAutomation<ITempControlDevice>(dm, AutomationNames.HeatingAutomation, Function.Heating, (a, d) =>
                        {                           
                            if (d.Boost_Mode.Value)
                            {
                                a.IgnoreLimits = true;
                                return 100;
                            }

                            int target = Convert.ToInt32(Math.Round(d.Set_Point_Temperature.Value * 10M));
                            int actual = Convert.ToInt32(Math.Round(d.Actual_Temperature.Value * 10M));
                            
                            int diff = target - actual;
                            return diff;
                        }                   
                    );
                    heatingAutomation.RefencePoint = Settings.Default.HeatingAutomationRefencePoint;
                    heatingAutomation.Hysteresis = Settings.Default.HeatingAutomationHysteresis;
                    heatingAutomation.MaxOnTime = Settings.Default.HeatingAutomationMaxOnTime;
                    heatingAutomation.MinOnTime = Settings.Default.HeatingAutomationMinOnTime;
                    heatingAutomation.MinOffTime = Settings.Default.heatingAutomationMinOffTime;

                    LOGGER.Info("Starting window open/close automation");
                    var windowAutomation = new WindowOpenAutomation(dm, AutomationNames.WindowOpenAutomation);

                    LOGGER.Info("Starting heating master valus sync automation");
                    var syncHeatingMastervaluesAutomation = new SyncHeatingMasterValuesAutomation(dm, AutomationNames.SyncHeatingMastervaluesAutomation);

                    LOGGER.Info("Starting webserver");
                    //init web server
                    //web server runs in async mode, locking is required around all DM objects
                    var server = new WebServer(Settings.Default.WebServerListenPort);
                    server.WithCors(); 

                    RoomController.DeviceManager = dm;
                    server.WithWebApi("/api", m => m.WithController<RoomController>());

                    AlarmControler.AlarmAutomation = alarmAutomation;
                    server.WithWebApi("/api", m => m.WithController<AlarmControler>());

                    server.WithStaticFolder("/", Settings.Default.WebServerRoot, true, m => m.WithContentCaching(true));

                    server.RunAsync();

                    LOGGER.Info("Starting entering main loop");
                    for (;;)
                    {
                        //this will refresh data from HM api and run all automations
                        dm.Work();

                        //log events
                        foreach (var e in dm.Events)
                        {
                            LOGGER.InfoFormat("{0} EVENT {1}: ({2}) => ({3})",
                                e.EventTimestamp.ToString("o"),
                                e.Current.Name,
                                e.Previous == null ? "" : e.Previous.Value, e.Current.Value
                            );
                        }

                        //refresh data every so often
                        new ManualResetEvent(false).WaitOne(200);

                        restartTries = 0;
                    }
                }
                catch (Exception e)
                {
                    restartTries++;

                    if (restartTries < 5)
                    {
                        LOGGER.Error($"Got unhandled exception {restartTries} in a row. Restarting imediately", e);
                    }
                    else
                    {
                        LOGGER.Error($"Got unhandled exception {restartTries} in a row. Restarting in 3s..", e);
                        new ManualResetEvent(false).WaitOne(3000);
                    }
                }
            }
        }
    }
}
