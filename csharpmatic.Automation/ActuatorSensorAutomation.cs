using csharpmatic.Automation.UsageLimits;
using csharpmatic.Generic;
using csharpmatic.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Automation
{
    public class ActuatorSensorAutomation<T> : IAutomation where T : IHmDevice
    {
        public string Name { get; private set; }
        public string DeviceFunction { get; private set; }
        public int RefencePoint { get; set; } = 0;
        public int Hysteresis { get; set; } = 1;
        public TimeSpan MaxOnTime { get; set;} = new TimeSpan(0, 5, 0);
        public TimeSpan MinOnTime { get; set; } = new TimeSpan(0, 0, 30);
        public TimeSpan MinOffTime { get; set; } = new TimeSpan(0, 1, 0);
        public DeviceManager DeviceManager { get; private set; }
               
        public Func<ActuatorSensorAutomation<T>, T, int> DatapointGetter;

        private Dictionary<string, UsageTracker> UsageTracker;
        private List<UsageLimit> UsageLimits;

        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IgnoreLimits = false;
        private DateTime lastWork;

        public ActuatorSensorAutomation(DeviceManager dm, string name, string deviceFunction, Func<ActuatorSensorAutomation<T>, T, int> datapointGetter)
        {
            DeviceFunction = deviceFunction;
            Name = name;
            DeviceManager = dm;
            DatapointGetter = datapointGetter;         
   
            UsageTracker = new Dictionary<string, UsageTracker>();
            UsageLimits = new List<UsageLimit>();
            UsageLimits.Add(new UsageLimit(new TimeSpan(0, 7, 0), 5)); 
            UsageLimits.Add(new UsageLimit(new TimeSpan(0, 15, 0), 10));
            UsageLimits.Add(new UsageLimit(new TimeSpan(0, 60, 0), 20));

            DeviceManager.RegisterAutomation(this);
        }

        private void AddActuatorUsage(ISingleSwitchControlDevice dev)
        {
            if (!UsageTracker.ContainsKey(dev.ISEID))
                UsageTracker.Add(dev.ISEID, new UsageTracker());

            UsageTracker ut = UsageTracker[dev.ISEID];

            ut.SetUsageTrueWins(dev.State.Value);
        }

        public bool CheckIfActuatorIsWithinUsageLimits(ISingleSwitchControlDevice dev)
        {
            foreach(var ul in UsageLimits)
            {
                UsageTracker ut = null;

                if (!UsageTracker.TryGetValue(dev.ISEID, out ut))
                    continue;

                int cnt = ut.GetEventCount(ul.TimeSpan, true);

                if (cnt > ul.MaximumTrue)
                {
                    if (dev.State.Value)
                        LOGGER.InfoFormat($"{dev.Name} is above usage limit: {cnt} (max {ul.MaximumTrue} in {ul.TimeSpan}");

                    return false;
                }
            }

            return true;
        }

        public void Work()
        {
            if (RefencePoint == 0)
                throw new Exception("Reference point not set!");

            if ((DateTime.Now - lastWork).TotalSeconds < 3)
                return;

            lastWork = DateTime.Now;

            IgnoreLimits = false;
            
            //logic:
            //- each control group is controlled by multiple actuators assigned to the room that sensors are in. both sensors and acturators must be in the same function group
            //- if in any room, function's datapoint goes above a ref point, all function actuators assigned to that room will be turned OFF
            //- if in any room, function's datapoint goes belove ref point, all function actuators assigned to that room will be turned ON
            //  - actuators that are needed for multiple rooms, will be kept alive, till they are not needed to be ON in all rooms
            //- hysteresis will be guarding against too frequent on and off

            //this function must exist in your setup
            //skip devices which are not available
            var devices = DeviceManager.Devices.Where(w => w.Functions.Contains(DeviceFunction)).ToList();

            //get list of all sensors in all rooms supporting the interface
            //skip devices which are offline or have their config not up to date
            //skip devices which are having 'Not_sensor' function
            var sensors = devices.Where(w => w is T && w.Reachable && !w.PendingConfig && !w.Functions.Contains(Function.Not_Sensor)).Cast<T>();

            //get list of actuators for all rooms for function. Dimmer actuators are not supported.
            //intentionaly we get all actuators, even the 'down' ones.
            var actuators = devices.Where(w => w is ISingleSwitchControlDevice).Select(s => s as ISingleSwitchControlDevice);

            //get list of all rooms
            var rooms = devices.SelectMany(s => s.Rooms).Distinct().ToList();

            //hashset of all actuators that should remain on, set is empty on start
            var toON = new HashSet<ISingleSwitchControlDevice>();

            //build the reason to be turned ON dict
            foreach (var r in rooms)
            {
                LOGGER.Debug($"[{DeviceFunction} automation] Checking room: {r}");

                //get actuators for the room              
                foreach (var a in actuators.Where(w => w.Rooms.Contains(r)))
                {
                    AddActuatorUsage(a);

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

                    LOGGER.Debug($"\t{a.Name} in {r} is {a.State.Value}, OFF condition: {offCondition}");

                    foreach (var s in sensors.Where(s => s.Rooms.Contains(r)))
                    {
                        int dpValue = DatapointGetter(this, s);

                        if (dpValue >= offCondition)
                        {                           
                            if (!toON.Contains(a))
                                toON.Add(a);

                            if(a.State.Value == false)
                                LOGGER.Info($"\t{s.Name} in {r} did not meet the OFF condition {offCondition}, value: {dpValue}. Marking to turn ***ON*** {a.Name}");                            
                        }
                        else
                            LOGGER.Debug($"\t{s.Name} in {r} met OFF condition {offCondition}, valve open: {dpValue}");
                    }
                }
            }

            //go over list all actuators, turn ON the ones on the list, turn off if not on the list
            //respect safe check values
            foreach (var a in actuators)
            {
                TimeSpan howLongInState = (DateTime.UtcNow - a.State.Timestamp);
                LOGGER.Debug($"{a.Name} is {a.State.Value} for {howLongInState}");

                //bool isWithinLimits = CheckIfActuatorIsWithinUsageLimits(a);

                if (a.State.Value == true && (howLongInState > MaxOnTime) && IgnoreLimits == false)
                {
                    LOGGER.Info($"Turning OFF {a.Name}, it has been ON for too long  ({howLongInState} vs. {MaxOnTime})");
                    a.State.Value = false;
                }
                else if (toON.Contains(a) && a.State.Value == false)
                {
                    if (MinOffTime > howLongInState && IgnoreLimits == false)
                        LOGGER.Debug($"Cannot turn ON {a.Name}, it has not been OFF long enough ({howLongInState} vs. {MinOffTime})");
                    else
                    {
                        LOGGER.InfoFormat($"Turning ON {a.Name}");
                        a.State.Value = true;
                    }
                }
                else if (!toON.Contains(a) && a.State.Value == true)
                {
                    if (MinOnTime > howLongInState)                    
                        LOGGER.Debug($"Cannot turn OFF {a.Name}, it has not been ON long enough ({howLongInState} vs. {MinOnTime})");
                    else
                    {
                        LOGGER.Info($"Turning OFF {a.Name}");
                        a.State.Value = false;
                    }
                }
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ActuatorSensorAutomation() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
