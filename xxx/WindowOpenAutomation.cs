using csharpmatic.Generic;
using csharpmatic.Interfaces.Devices;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmaticAutomation
{
    public class WindowOpenAutomation
    {
        public static TimeSpan WindowOpenDelay = new TimeSpan(0, 0, 5);
        public static TimeSpan WindowCloseDelay = new TimeSpan(0, 1, 0);

        public DeviceManager DeviceManager { get; private set; }

        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowOpenAutomation(DeviceManager dm)
        {
            DeviceManager = dm;
        }

        public void Work()
        {
            //get all window open sensors assigned to heating function
            var sensor = DeviceManager.Devices.Where(w => w is HMIP_SWDO && w.Functions.Contains(Function.Heating)).Cast<HMIP_SWDO>().ToList();
                       
            var rooms = sensor.SelectMany(s => s.Rooms).Distinct();

            foreach(string r in rooms)
            {
                var sr = sensor.Where(w => w.Rooms.Contains(r));

                bool windowOpen = sr.Where(w => w.State.Value != 0 && DateTime.UtcNow - w.State.Timestamp >= WindowOpenDelay).Count() > 0;
                bool windowClosed = sr.Where(w => w.State.Value == 0 && DateTime.UtcNow - w.State.Timestamp >= WindowCloseDelay).Count() > 0;

                if(windowOpen)
                {
                    var devices = DeviceManager.Devices.Where(w => w.Rooms.Contains(r) && w.DatapointByType.ContainsKey("WINDOW_STATE"));

                    foreach(Device d in devices)
                    {
                        Datapoint dp = d.DatapointByType["WINDOW_STATE"];
                        if (dp.GetValue<int>() == 0)
                        {
                            LOGGER.Info($"Window opened ({d.Name}) in {r}, setting WINDOW_OPEN=1 on {dp.Channel.Device.Name}");
                            dp.SetValue(1);
                        }
                    }

                }
                else if(windowClosed)
                {
                    var devices = DeviceManager.Devices.Where(w => w.Rooms.Contains(r) && w.DatapointByType.ContainsKey("WINDOW_STATE"));

                    foreach (Device d in devices)
                    {
                        Datapoint dp = d.DatapointByType["WINDOW_STATE"];
                        if (dp.GetValue<int>() != 0)
                        {
                            LOGGER.Info($"Window closed ({d.Name}) in {r}, setting WINDOW_OPEN=0 on {dp.Channel.Device.Name}");
                            dp.SetValue(0);
                        }
                    }
                }
                else
                {
                    //in transition state, do nothing
                }
            }
        }
    }
}
