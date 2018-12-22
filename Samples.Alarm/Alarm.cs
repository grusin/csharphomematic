using csharpmatic.Generic;
using csharpmatic.Interfaces.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Alarm
{
    public class Alarm
    {
        public bool Armed { get; private set; }
        public bool AlarmTriggered { get; private set; }
        public DeviceManager DeviceManager { get; private set; }

        public Dictionary<string, int> lastDeviceStateTimestamp;

        //private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Work()
        {
            var devices = DeviceManager.Devices.Where(w => w.Functions.Contains("Alarm")).ToList();

            var sensors = devices.Where(w => w is HMIP_SWDO && w.Reachable && !w.PendingConfig && !w.Functions.Contains(Device.NoSensorDeviceFunction)).Cast<HMIP_SWDO>();

            foreach(var s in sensors)
            {
                bool trigger = false;

                if(s.Sabotage.Value)
                {
                    TriggerAlarm(s.Sabotage);
                }                
                
                if(lastDeviceStateTimestamp.ContainsKey(s.Address))
                {
                    int lastTimestamp = lastDeviceStateTimestamp[s.Address];
                }    
            }
        }

        public void TriggerAlarm<T>(TypedDatapoint<T> dp)
        {
            //if not armed, ignore this
            if (!Armed)
                return;

            AlarmTriggered = true;
        }
    }
}
