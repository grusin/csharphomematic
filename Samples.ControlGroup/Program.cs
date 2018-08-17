using csharpmatic.Generic;
using csharpmatic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.ControlGroup
{
    class Program
    {
        static void Main(string[] args)
        {
            DeviceManager dm = new DeviceManager("192.168.1.200");

            Program p = new Program();

            for(;;)
            {
                dm.Refresh();
                //p.sync_heating_values(dm);
                //p.valve_heating_control(dm);
                p.humidity_control(dm);
                Thread.Sleep(500);
            }
        }

        public void sync_heating_values(DeviceManager dm) 
        {
            var devices = dm.GetDevicesImplementingInterface<ITempControlDevice>();

            var houseLeader = devices.Where(w => w.ISEID == devices.Min(min => min.ISEID)).FirstOrDefault();

            if (houseLeader == null)
                return;

            decimal eps = 0.000001M;

            foreach (var lmv in houseLeader.Channels[1].MasterValues.Values)
            {
                foreach(var d in devices)
                {
                    if (d == houseLeader || d.Channels.Count() < 2)
                        continue;

                    MasterValue mv = null;

                    if(d.Channels[1].MasterValues.TryGetValue(lmv.Name, out mv))
                    {
                        if(Math.Abs(mv.Value- lmv.Value) > eps)
                        {
                            Console.WriteLine($"{d.Name} {mv.Name} = {mv.Value} where leader ({houseLeader.Name}) has {lmv.Value}");
                        }
                    }
                }
            }

        }
                              
        public void valve_heating_control(DeviceManager dm)
        {
            int refPoint = 20;
            int hysteresis = 1;
            TimeSpan maxOnTime = new TimeSpan(0, 1, 0);
            TimeSpan minOnTime = new TimeSpan(0, 0, 10);
            TimeSpan minOffTime = new TimeSpan(0, 1, 0);

            //logic:
            //- each control group is controlled by multiple actuators assigned to the room that sensors are in. both sensors and acturators must be in humidty function group
            //- if in any room, heating goes above ref point, all heating actuators assigned to that room will be turned OFF
            //- if in any room, heating goes belove ref point, all heating actuators assigned to that room will be turned ON
            //  - actuators that are needed for multiple rooms, will be kept alive, till they are not needed to be ON in all rooms
            //- hysteresis will be guarding against too frequent on and off

            //this function must exist in your setup
            var heatingDevices = dm.Devices.Where(w => w.Functions.Contains("Heating")).ToList();

            //get list of all valve sensors in all rooms for heating
            var sensors = heatingDevices.Where(w => w is IValveControlDevice).Select(s => s as IValveControlDevice);

            //get list of actuators for all rooms for heating
            var actuators = heatingDevices.Where(w => w is ISingleSwitchControlDevice).Select(s => s as ISingleSwitchControlDevice);

            //get list of all rooms
            var rooms = heatingDevices.SelectMany(s => s.Rooms).Distinct().ToList();

            //hashset of all actuators that should remain on, set is empty on start
            var toON = new HashSet<ISingleSwitchControlDevice>();

            //build the reason to be turned ON dict
            foreach (var r in rooms)
            {
                Console.WriteLine($"Checking room: {r}");

                //get actuators for the room              
                foreach (var a in actuators.Where(w => w.Rooms.Contains(r)))
                {
                    int offCondition;

                    //if actuators is on, it has to go below refPoint - hysteresis to be switched off
                    if (a.State.Value == true)
                    {
                        offCondition = refPoint - hysteresis;
                    }
                    //if actuators is off, it just needs to be below refPoint to be off
                    else
                    {
                        offCondition = refPoint;
                    }

                    Console.WriteLine($"\t{a.Name} in {r} is {a.State.Value}, OFF condition: {offCondition}");

                    foreach (var s in sensors.Where(s => s.Rooms.Contains(r)))
                    {
                        if (s.Level.Value >= offCondition)
                        {
                            Console.WriteLine($"\t{s.Name} in {r} did not meet the OFF condition {offCondition}, valve open: {s.Level.Value}. Marking to turn ***ON*** {a.Name}");
                            if (!toON.Contains(a))
                                toON.Add(a);
                        }
                        else
                            Console.WriteLine($"\t{s.Name} in {r} met OFF condition {offCondition}, valve open: {s.Level.Value}");
                    }
                }
            }
                        
            //go over list all actuators, turn ON the ones on the list, turn off if not on the list
            //respect safe check values
            foreach (var a in actuators)
            {
                TimeSpan howLongInState = (DateTime.UtcNow - a.State.Timestamp);
                Console.WriteLine($"{a.Name} is {a.State.Value} for {howLongInState}");

                if (a.State.Value == true && howLongInState > maxOnTime)
                {
                    Console.WriteLine($"Turning OFF {a.Name}, it has been ON for too long  ({howLongInState} vs. {maxOnTime})");
                    a.State.Value = false;
                }
                else if (toON.Contains(a) && a.State.Value == false)
                {
                    if (minOffTime > howLongInState)
                        Console.WriteLine($"Cannot turn ON {a.Name}, it has not been OFF long enough ({howLongInState} vs. {minOffTime})");
                    else
                        a.State.Value = true;
                }
                else if (!toON.Contains(a) && a.State.Value == true)
                {
                    if (minOnTime > howLongInState)
                        Console.WriteLine($"Cannot turn OFF {a.Name}, it has not been ON long enough ({howLongInState} vs. {minOnTime})");
                    else
                        a.State.Value = false;
                }
            }
        }

        public void humidity_control(DeviceManager dm)
        {
            int refPoint = 50;
            int hysteresis = 3;
            TimeSpan maxOnTime = new TimeSpan(0, 1, 0);
            TimeSpan minOnTime = new TimeSpan(0, 0, 10);
            TimeSpan minOffTime = new TimeSpan(0, 1, 0);

            //logic:
            //- each control group is controlled by multiple actuators assigned to the room that sensors are in. both sensors and acturators must be in humidty function group
            //- if in any room, humidity goes above ref point, all humidity actuators assigned to that room will be turned ON
            //- if in any room, humidity goes belove ref point, all humidity actuators assigned to that room will be HINTED to be turned OFF
            //  - actuators that are needed for multiple rooms, will be kept alive, till they are not needed to be ON in all rooms
            //- hysteresis will be guarding against too frequent on and off

            //this function must exist in your setup
            var humidityDevices = dm.Devices.Where(w => w.Functions.Contains("Humidity")).ToList();
                        
            //get list of all sensors in all rooms for humidity
            var sensors = humidityDevices.Where(w => w is IHumidityControlDevice).Select(s => s as IHumidityControlDevice);

            //get list of actuators for all rooms for humidity
            var actuators = humidityDevices.Where(w => w is ISingleSwitchControlDevice).Select(s => s as ISingleSwitchControlDevice);
            
            //get list of all rooms
            var rooms = humidityDevices.SelectMany(s => s.Rooms).Distinct().ToList();

            //hashset of all actuators that should remain on, empty on start
            var toON = new HashSet<ISingleSwitchControlDevice>();

            //build the reason to be turned ON dict
            foreach (var r in rooms)
            {
                Console.WriteLine($"Checking room: {r}");

                //get actuators for the room              
                foreach (var a in actuators.Where(w => w.Rooms.Contains(r)))
                {
                    int offCondition;

                    //if actuators is on, it has to go bellow setpoint + hysteresis to be switched off
                    if (a.State.Value == true)
                    {
                        offCondition = refPoint - hysteresis;
                    }
                    //if actuators is off, it just needs to go below refPoint to be switched off
                    else
                    {
                        offCondition = refPoint;
                    }                    
                                        
                    Console.WriteLine($"{a.Name} in {r} is {a.State.Value}, OFF condition: {offCondition}");                                     

                    foreach (var s in sensors.Where(s => s.Rooms.Contains(r)))
                    {                        
                        if (s.Humidity.Value >= offCondition)
                        {                          
                            Console.WriteLine($"{s.Name} in {r} meet the condition {offCondition}, humidity: {s.Humidity.Value}. Marking to turn ON {a.Name}");
                            if (!toON.Contains(a))
                                toON.Add(a);
                        }
                    }
                }
            }                            

            //go over list all actuators, turn ON the ones on the list, turn off if not on the list
            //respect safe check values
            foreach(var a in actuators)
            {
                TimeSpan howLongInState = (DateTime.UtcNow - a.State.Timestamp);
                Console.WriteLine($"{a.Name} is {a.State.Value} for {howLongInState}");

                if(a.State.Value == true && howLongInState > maxOnTime)
                {
                    Console.WriteLine($"Turning OFF {a.Name}, it has been ON for too long  ({howLongInState} vs. {maxOnTime})");
                    a.State.Value = false;
                }
                else if (toON.Contains(a) && a.State.Value == false)
                {
                    if (minOffTime > howLongInState)
                        Console.WriteLine($"Cannot turn ON {a.Name}, it has not been OFF long enough ({howLongInState} vs. {minOffTime})");
                    else
                        a.State.Value = true;
                }
                else if (!toON.Contains(a) && a.State.Value == true)
                {
                    if (minOnTime > howLongInState)
                        Console.WriteLine($"Cannot turn OFF {a.Name}, it has not been ON long enough ({howLongInState} vs. {minOnTime})");
                    else
                        a.State.Value = false;
                }
            }
        }
    }
}
