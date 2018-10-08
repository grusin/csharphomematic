using csharpmatic.Automation;
using csharpmatic.Generic;
using csharpmatic.Interfaces;
using csharpmatic.RestApi;
using HouseAutomationService.Properties;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace HouseAutomationService
{
    class HomeAutomationService
    {
        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            for (;;)
            {
                try
                {
                    LOGGER.Info($"Starting device manager for {Settings.Default.HomematicServerAddress}");
                    
                    //init device manager
                    var dm = new DeviceManager(Settings.Default.HomematicServerAddress);

                    LOGGER.Info($"Starting humidity manager");
                    //init humidity automation, humidity is int from 0 to 100
                    var humidityAutomation = new ActuatorSensorAutomation<IHumidityControlDevice>(dm, "Humidity", (a, d) => d.Humidity.Value);
                    humidityAutomation.RefencePoint = Settings.Default.HumidityAutomationRefencePoint;
                    humidityAutomation.Hysteresis = Settings.Default.HumidityAutomationHysteresis;
                    humidityAutomation.MaxOnTime = Settings.Default.HumidityAutomationMaxOnTime;
                    humidityAutomation.MinOnTime = Settings.Default.HumidityAutomationMinOnTime;
                    humidityAutomation.MinOffTime = Settings.Default.HumidityAutomationMinOffTime;
 
                    LOGGER.Info("Starting heating manager");
                    
                    var heatingAutomation = new ActuatorSensorAutomation<ITempControlDevice>(dm, "Heating", (a, d) =>
                        {
                            a.IgnoreLimits = false;

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

                    LOGGER.Info("Starting webserver");
                    //init web server
                    //web server runs in async mode, locking is required around all DM objects
                    var server = new WebServer(Settings.Default.WebServerListenPort, RoutingStrategy.Regex);
                    server.RegisterModule(new WebApiModule());
                    server.Module<CorsModule>();
                    RoomController.DeviceManager = dm;
                    server.Module<WebApiModule>().RegisterController<RoomController>();
                    server.RegisterModule(new StaticFilesModule(@"www"));
                    server.Module<StaticFilesModule>().UseRamCache = true;
                    server.Module<StaticFilesModule>().DefaultExtension = ".html";

                    server.RunAsync();

                    LOGGER.Info("Starting entering main loop");
                    for (;;)
                    {
                        //has internal smart locking (only locks when data structures are modified, not during xmlapi queries)
                        List<DatapointEvent> eventsSinceLastRefresh = dm.Refresh();
                        foreach (var e in eventsSinceLastRefresh)
                        {
                            LOGGER.InfoFormat("{0} EVENT {1}: ({2}) => ({3})",
                                e.EventTimestamp.ToString("o"),
                                e.Current.Name,
                                e.Previous == null ? "" : e.Previous.Value, e.Current.Value
                            );
                        }

                        //dumb locking for now
                        lock (dm.RefreshLock)
                        {
                            SyncHeatingMasterValuesAutomation.SyncHeatingMastervalues(dm);
                            humidityAutomation.Work();
                            heatingAutomation.Work();
                        }

                        //refresh data every second
                        new ManualResetEvent(false).WaitOne(1000);
                    }

                }
                catch (Exception e)
                {
                    LOGGER.Error("Error. Restarting device manager in 10s..", e);
                    new ManualResetEvent(false).WaitOne(10000);
                }
            }
        }
    }
}
