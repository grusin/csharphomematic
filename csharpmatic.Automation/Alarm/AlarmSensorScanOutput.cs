using csharpmatic.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmaticAutomation.Alarm
{
    public class AlarmSensorScanOutput
    {
        
        public string DeviceISEID { get; private set; }
        public bool Triggered { get; private set; }
        public string DeviceName { get; private set; }
        public string DeviceAddress { get; private set; }
        public string DatapointName { get; private set; }
        public string DatapointValue { get; private set; }
        public DateTime DatapointTimestamp { get; private set; }
        
        public AlarmSensorScanOutput(Device d, Datapoint dp)
        {
            DeviceISEID = d.ISEID;
            DeviceName = d.Name;

            if (dp != null)
            {
                DatapointName = dp.Type;
                DatapointValue = dp.GetValueString();
                DatapointTimestamp = dp.Timestamp;
                Triggered = true;
            }
            else
                Triggered = false;
        }

        public string GetTriggeredMessage()
        {
            if (Triggered)
                return String.Format($"{DatapointTimestamp.ToString("o")} Datapoint '{DeviceName}'.'{DatapointName}' has been triggered: '{DatapointValue}'");
            else
                return "";
        }
    }
}
