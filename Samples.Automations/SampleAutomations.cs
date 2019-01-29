using csharpmatic.Automation;
using csharpmatic.Generic;
using csharpmatic.Interfaces;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.Automations
{
    class SampleAutomations
    {
        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            
            //instanciate device manager
            DeviceManager dm = new DeviceManager("192.168.1.200");

            //create alarm automation
            var windowAutomation = new WindowOpenAutomation(dm, AutomationNames.WindowOpenAutomation);

            //create humidty automation group. both humidity sensors and actuators must be assigned to Humidity function and the same rooms
            //actuator has to be in all rooms that should have humidity automation enabled
            var humidityAutomation = new ActuatorSensorAutomation<IHumidityControlDevice>(dm, AutomationNames.HumidityAutomation, Function.Humidity, (a, d) => d.Humidity.Value);
            humidityAutomation.RefencePoint = 55; //you should check humidity across all devices at home, and take mean + 10% to be safe.
            humidityAutomation.Hysteresis = 5;
            humidityAutomation.MaxOnTime = new TimeSpan(0, 1, 0);
            humidityAutomation.MinOnTime = new TimeSpan(0, 0, 15);
            humidityAutomation.MinOffTime = new TimeSpan(0, 0, 5);

            //create heating automation group. both heating sensors and actuators must be assigned to Heating function and the same rooms
            //actuator has to be in all rooms that should have heating automation enabled
            var heatingAutomation = new ActuatorSensorAutomation<IValveControlDevice>(dm, AutomationNames.HeatingAutomation, Function.Heating, (a, d) => Convert.ToInt32(Math.Round(d.Level.Value * 100)));
            heatingAutomation.RefencePoint = 20; //20% valve open
            heatingAutomation.Hysteresis = 2;
            heatingAutomation.MaxOnTime = new TimeSpan(0, 6, 0);
            heatingAutomation.MinOnTime = new TimeSpan(0, 0, 30);
            heatingAutomation.MinOffTime = new TimeSpan(0, 5, 0);

            //create master value sync automation, which will sync relevant schedule mastervalues across heating rooms/house.
            var syncHeatingMastervaluesAutomation = new SyncHeatingMasterValuesAutomation(dm, AutomationNames.SyncHeatingMastervaluesAutomation);

            for (;;)
            {
                //this will refresh data from HM api and run all automations
                dm.Work();

                //log events
                List<DatapointEvent> eventsSinceLastRefresh = dm.Refresh();
                foreach (var e in dm.Events)
                {
                    LOGGER.InfoFormat("{0} EVENT {1}: ({2}) => ({3})",
                        e.EventTimestamp.ToString("o"),
                        e.Current.Name,
                        e.Previous == null ? "" : e.Previous.Value, e.Current.Value
                    );
                }

                //wait a bit before running again
                new ManualResetEvent(false).WaitOne(200);
            }
        }
    }
}
