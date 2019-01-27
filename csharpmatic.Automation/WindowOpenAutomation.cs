using csharpmatic.Generic;
using csharpmatic.Interfaces.Devices;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Automation
{
    public class WindowOpenAutomation : IAutomation
    {
        public string Name { get; private set; }

        public static TimeSpan WindowOpenDelay = new TimeSpan(0, 0, 5);
        public static TimeSpan WindowCloseDelay = new TimeSpan(0, 1, 0);

        public DeviceManager DeviceManager { get; private set; }

        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowOpenAutomation(DeviceManager dm, string name)
        {
            Name = name;
            DeviceManager = dm;

            DeviceManager.RegisterAutomation(this);
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
        // ~WindowOpenAutomation() {
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
