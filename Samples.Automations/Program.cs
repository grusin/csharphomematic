using csharpmatic.Generic;
using csharpmatic.Interfaces;
using csharpmaticAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.Automations
{
    class Program
    {
        static void Main(string[] args)
        {
            //instanciate device manager
            DeviceManager dm = new DeviceManager("192.168.1.200");

            //create humidty automation group. both humidity sensors and actuators must be assigned to Humidity function and the same rooms
            //actuator has to be in all rooms that should have humidity automation enabled
            var humidityAutomation = new ActuatorSensorAutomation<IHumidityControlDevice>(dm, Function.Humidity, (a, d) => d.Humidity.Value);
            humidityAutomation.RefencePoint = 55; //you should check humidity across all devices at home, and take mean + 10% to be safe.
            humidityAutomation.Hysteresis = 5;
            humidityAutomation.MaxOnTime = new TimeSpan(0, 1, 0);
            humidityAutomation.MinOnTime = new TimeSpan(0, 0, 15);
            humidityAutomation.MinOffTime = new TimeSpan(0, 0, 15);

            //create heating automation group. both heating sensors and actuators must be assigned to Heating function and the same rooms
            //actuator has to be in all rooms that should have heating automation enabled
            var heatingAutomation = new ActuatorSensorAutomation<IValveControlDevice>(dm, Function.Heating, (a, d) => Convert.ToInt32(Math.Round(d.Level.Value * 100)));
            heatingAutomation.RefencePoint = 20; //20% valve open
            heatingAutomation.Hysteresis = 2;
            heatingAutomation.MaxOnTime = new TimeSpan(0, 5, 0);
            heatingAutomation.MinOnTime = new TimeSpan(0, 0, 30);
            heatingAutomation.MinOffTime = new TimeSpan(0, 3, 0);

            var windowAutomation = new WindowOpenAutomation(dm);

            for (;;)
            {
                //pull latest data from the web services
                dm.Refresh();

                //syncs heating master values across the house and across the rooms
                //house is synced with all master values except ones related to temperature
                //each toom is synced with just the temperature related master values
                //this setup allows very flexiable management of the time zones, but the heat zones are seperate for each room :)
                SyncHeatingMasterValuesAutomation.SyncHeatingMastervalues(dm);

                //control the actuators based on the values
                //logic:
                //- each control group is controlled by multiple actuators assigned to the room that sensors are in. both sensors and acturators must be in the same function group
                //- if in any room, function's datapoint goes above a ref point, all function actuators assigned to that room will be turned OFF
                //- if in any room, function's datapoint goes belove ref point, all function actuators assigned to that room will be turned ON
                //  - actuators that are needed for multiple rooms, will be kept alive, till they are not needed to be ON in all rooms
                //- hysteresis will be guarding against too frequent on and off

                //DEBUG these are commented out if you don't need to test them/use them
                //humidityAutomation.Work();
                //heatingAutomation.Work();

                windowAutomation.Work();

                //wait a bit before running again
                Thread.Sleep(1000);
            }
        }
    }
}
