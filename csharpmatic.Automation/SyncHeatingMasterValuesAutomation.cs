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
    public class SyncHeatingMasterValuesAutomation : IAutomation
    {
        public string Name { get; private set; }
     
        private Dictionary<string, string> RoomLeadersCache = new Dictionary<string, string>();
        private DeviceManager DeviceManager;

        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DateTime lastWork;

        public SyncHeatingMasterValuesAutomation(DeviceManager dm, string name)
        {
            Name = name;
            DeviceManager = dm;
            DeviceManager.RegisterAutomation(this);
        }

        public void Work()
        {
            if((DateTime.Now - lastWork).TotalSeconds < 15)
                return;

            lastWork = DateTime.Now;

            var allDevices = DeviceManager.GetDevicesImplementingInterface<ITempControlDevice>();
            var houseLeader = allDevices.Where(w => w.ISEID == allDevices.Min(min => min.ISEID)).FirstOrDefault();
            var houseMasterValues = new HashSet<string>(
                houseLeader.Channels[1].MasterValues.Values.Where(w => IsSchedulerEndTime(w)).Select(s => s.Name));


            LOGGER.Debug("Syncing house mastervalues...");
            SyncHeatingMastervalues(houseLeader, allDevices, houseMasterValues);

            var allRooms = allDevices.SelectMany(d => d.Rooms).Distinct().ToList();

            foreach (var r in allRooms)
            {
                var roomDevices = allDevices.Where(w => w.Rooms.Contains(r)).ToList();
                var roomLeader = allDevices.Where(w => w.ISEID == roomDevices.Min(min => min.ISEID)).FirstOrDefault();

                var roomMasterValues = new HashSet<string>(roomLeader.Channels[1].MasterValues.Values.Where(w =>
                    IsSchedulerTemperature(w)
                    || w.Name == "BOOST_TIME_PERIOD"
                    || w.Name == "OPTIMUM_START_STOP"
                ).Select(s => s.Name));

                if (!RoomLeadersCache.ContainsKey(r))
                {
                    RoomLeadersCache.Add(r, roomLeader.ISEID);
                    LOGGER.InfoFormat($"Room '{r}' leader is: '{roomLeader.Name}'");
                }

                LOGGER.Debug($"Syncing {r} mastervalues...");
                SyncHeatingMastervalues(roomLeader, roomDevices, roomMasterValues);
            }

            LOGGER.Debug("Mastervalues across devices are in sync!");
        }

        private void SyncHeatingMastervalues(ITempControlDevice leader, List<ITempControlDevice> devicesInScope, HashSet<string> masterValuesInScope)
        {
            if (leader == null || leader.PendingConfig || !leader.Reachable)
                return;

            decimal eps = 0.000001M;

            Dictionary<Channel, List<MasterValue>> toChange = new Dictionary<Channel, List<MasterValue>>();

            foreach (var lmv in leader.Channels[1].MasterValues.Values.Where(w => masterValuesInScope.Contains(w.Name)))
            {
                foreach (var d in devicesInScope)
                {
                    if (d == leader || d.Channels.Count() < 2 || d.PendingConfig || !d.Reachable)
                        continue;

                    MasterValue mv = null;

                    if (d.Channels[1].MasterValues.TryGetValue(lmv.Name, out mv))
                    {
                        if (Math.Abs(mv.Value - lmv.Value) > eps)
                        {
                            if (!toChange.ContainsKey(d.Channels[1]))
                                toChange.Add(d.Channels[1], new List<MasterValue>());

                            toChange[d.Channels[1]].Add(lmv);
                            LOGGER.InfoFormat($"Master value sync: found {d.Name} {mv.Name} = {mv.Value} where leader ({leader.Name}) has {lmv.Value}. Syncing with leader.");
                        }
                    }
                }
            }

            foreach (var kvp in toChange)
            {
                kvp.Key.SetMasterValues(kvp.Value);
            }
        }

        public static bool IsSchedulerEndTime(MasterValue v)
        {
            return v.Name.Length > 2
                    && v.Name[0] == 'P'
                    && v.Name[1] >= '1' && v.Name[1] <= '9'
                    && v.Name.Substring(2).StartsWith("_ENDTIME_");
        }

        public static bool IsSchedulerTemperature(MasterValue v)
        {
            return v.Name.Length > 2
                    && v.Name[0] == 'P'
                     && v.Name[1] >= '1' && v.Name[1] <= '9'
                    && v.Name.Substring(2).StartsWith("_TEMPERATURE_");
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
        // ~SyncHeatingMasterValuesAutomation() {
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
