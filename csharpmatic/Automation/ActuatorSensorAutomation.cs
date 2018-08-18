using csharpmatic.Generic;
using csharpmatic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Automation
{
    public class ActuatorSensorAutomation<T> where T : IHmDevice
    {
        public string Function { get; private set; }
        public int RefencePoint { get; set; } = 0;
        public int Hysteresis { get; set; } = 1;

        public TimeSpan MaxOnTime { get; set;} = new TimeSpan(0, 5, 0);

        public TimeSpan MinOnTime { get; set; } = new TimeSpan(0, 0, 30);
        public TimeSpan MinOffTime { get; set; } = new TimeSpan(0, 1, 0);

        public DeviceManager DeviceManager { get; private set; }

        public string DatapointType { get; private set; }

        public ActuatorSensorAutomation(DeviceManager dm, string function, string datapointType)
        {
            Function = function;
            DeviceManager = dm;
            DatapointType = datapointType;
        }

        public void Work()
        {
            if (RefencePoint == 0)
                throw new Exception("Reference point not set!");
            
            //logic:
            //- each control group is controlled by multiple actuators assigned to the room that sensors are in. both sensors and acturators must be in the same function group
            //- if in any room, function's datapoint goes above a ref point, all function actuators assigned to that room will be turned OFF
            //- if in any room, function's datapoint goes belove ref point, all function actuators assigned to that room will be turned ON
            //  - actuators that are needed for multiple rooms, will be kept alive, till they are not needed to be ON in all rooms
            //- hysteresis will be guarding against too frequent on and off

            //this function must exist in your setup
            var devices = DeviceManager.Devices.Where(w => w.Functions.Contains(Function)).ToList();

            //get list of all sensors in all rooms supporting the interface
            var sensors = devices.Where(w => w is T);

            //get list of actuators for all rooms for function. Dimmer actuators are not supported.
            var actuators = devices.Where(w => w is ISingleSwitchControlDevice).Select(s => s as ISingleSwitchControlDevice);

            //get list of all rooms
            var rooms = devices.SelectMany(s => s.Rooms).Distinct().ToList();

            //hashset of all actuators that should remain on, set is empty on start
            var toON = new HashSet<ISingleSwitchControlDevice>();

            //build the reason to be turned ON dict
            foreach (var r in rooms)
            {
                Console.WriteLine($"[{Function} automation] Checking room: {r}");

                //get actuators for the room              
                foreach (var a in actuators.Where(w => w.Rooms.Contains(r)))
                {
                    int offCondition;

                    //if actuators is on, it has to go below refPoint - hysteresis to be switched off
                    if (a.State.Value == true)
                    {
                        offCondition = RefencePoint - Hysteresis;
                    }
                    //if actuators is off, it just needs to be below refPoint to be off
                    else
                    {
                        offCondition = RefencePoint;
                    }

                    Console.WriteLine($"\t{a.Name} in {r} is {a.State.Value}, OFF condition: {offCondition}");

                    foreach (var s in sensors.Where(s => s.Rooms.Contains(r)))
                    {
                        Datapoint dp = s.DatapointByType[DatapointType];

                        if (Convert.ToInt32(dp.Value) >= offCondition)
                        {
                            Console.WriteLine($"\t{s.Name} in {r} did not meet the OFF condition {offCondition}, valve open: {dp.Value}. Marking to turn ***ON*** {a.Name}");
                            if (!toON.Contains(a))
                                toON.Add(a);
                        }
                        else
                            Console.WriteLine($"\t{s.Name} in {r} met OFF condition {offCondition}, valve open: {dp.Value}");
                    }
                }
            }

            //go over list all actuators, turn ON the ones on the list, turn off if not on the list
            //respect safe check values
            foreach (var a in actuators)
            {
                TimeSpan howLongInState = (DateTime.UtcNow - a.State.Timestamp);
                Console.WriteLine($"{a.Name} is {a.State.Value} for {howLongInState}");

                if (a.State.Value == true && howLongInState > MaxOnTime)
                {
                    Console.WriteLine($"Turning OFF {a.Name}, it has been ON for too long  ({howLongInState} vs. {MaxOnTime})");
                    a.State.Value = false;
                }
                else if (toON.Contains(a) && a.State.Value == false)
                {
                    if (MinOffTime > howLongInState)
                        Console.WriteLine($"Cannot turn ON {a.Name}, it has not been OFF long enough ({howLongInState} vs. {MinOffTime})");
                    else
                        a.State.Value = true;
                }
                else if (!toON.Contains(a) && a.State.Value == true)
                {
                    if (MinOnTime > howLongInState)
                        Console.WriteLine($"Cannot turn OFF {a.Name}, it has not been ON long enough ({howLongInState} vs. {MinOnTime})");
                    else
                        a.State.Value = false;
                }
            }
        }
    }
}
