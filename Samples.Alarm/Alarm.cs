using csharpmatic.Generic;
using csharpmatic.Interfaces.Devices;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Alarm
{
    public class Alarm
    {
        public bool Armed { get => _armed; private set => _armed = value; }
        public bool AlarmTriggered { get => _alarmTriggered; private set => _alarmTriggered = value; }
        public DeviceManager DeviceManager { get; private set; }

        public Dictionary<string, DateTime> lastDeviceStateTimestamp;

        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool _armed;
        private bool _alarmTriggered;

        public Alarm(DeviceManager dm)
        {
            DeviceManager = dm;
            lastDeviceStateTimestamp = new Dictionary<string, DateTime>();

            //FIXME: we should read setting from the homematic software
            Armed = false;
            LOGGER.Info("Initalized alarm: DISARMED");
        }

        public void Work()
        {
            if (Armed)
                Work_Armed();

            if (AlarmTriggered)
                Work_AlarmTriggered();

        }
        
        private void Work_Armed()
        {
            if (!Armed)
                return;

            var devices = DeviceManager.Devices.Where(w => w.Functions.Contains(Function.Alarm)).ToList();

            var sensors = devices.Where(w => w is HMIP_SWDO && w.Reachable && !w.PendingConfig && !w.Functions.Contains(Function.Not_Sensor)).Cast<HMIP_SWDO>();

            foreach (var s in sensors)
            {
                if (s.Sabotage.Value)
                {
                    AlarmTriggered = true;
                    //TriggerAlarm(s.Sabotage);
                }

                if (s.State.Value != 0)
                {
                    AlarmTriggered = true;
                    //TriggerAlarm(s.State);
                }

                if (lastDeviceStateTimestamp.ContainsKey(s.Address))
                {
                    var lastTimestamp = lastDeviceStateTimestamp[s.Address];
                    if (lastTimestamp != s.State.Timestamp)
                    {
                        AlarmTriggered = true;
                        //TriggerAlarm(s.State);
                        lastDeviceStateTimestamp[s.Address] = s.State.Timestamp;
                    }
                }
                else
                    lastDeviceStateTimestamp.Add(s.Address, s.State.Timestamp);
            }
        }

        private void Work_AlarmTriggered()
        {

        }

        //public bool SetAlarmState(bool armed)
        //{
           
        //}
    }
}
