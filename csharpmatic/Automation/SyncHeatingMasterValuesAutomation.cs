using csharpmatic.Generic;
using csharpmatic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Automation
{
    public class SyncHeatingMasterValuesAutomation
    {
        public static void SyncHeatingMastervalues(ITempControlDevice leader, List<ITempControlDevice> devicesInScope, HashSet<string> masterValuesInScope)
        {
            if (leader == null || leader.PendingConfig)
                return;

            decimal eps = 0.000001M;

            Dictionary<Channel, List<MasterValue>> toChange = new Dictionary<Channel, List<MasterValue>>();

            foreach (var lmv in leader.Channels[1].MasterValues.Values.Where(w => masterValuesInScope.Contains(w.Name)))
            {
                foreach (var d in devicesInScope)
                {
                    if (d == leader || d.Channels.Count() < 2 || d.PendingConfig)
                        continue;

                    MasterValue mv = null;

                    if (d.Channels[1].MasterValues.TryGetValue(lmv.Name, out mv))
                    {
                        if (Math.Abs(mv.Value - lmv.Value) > eps)
                        {
                            if (!toChange.ContainsKey(d.Channels[1]))
                                toChange.Add(d.Channels[1], new List<MasterValue>());

                            toChange[d.Channels[1]].Add(lmv);
                            Console.WriteLine($"{d.Name} {mv.Name} = {mv.Value} where leader ({leader.Name}) has {lmv.Value}");
                        }
                    }
                }
            }

            foreach (var kvp in toChange)
            {
                kvp.Key.SetMasterValues(kvp.Value);
            }
        }

        public static void SyncHeatingMastervalues(DeviceManager dm)
        {
            var allDevices = dm.GetDevicesImplementingInterface<ITempControlDevice>();
            var houseLeader = allDevices.Where(w => w.ISEID == allDevices.Min(min => min.ISEID)).FirstOrDefault();
            var houseMasterValues = new HashSet<string>(houseLeader.Channels[1].MasterValues.Values.Where(w => !w.Name.Contains("TEMPERATURE")).Select(s => s.Name));
            var roomMasterValues = new HashSet<string>(houseLeader.Channels[1].MasterValues.Values.Where(w => w.Name.Contains("TEMPERATURE")).Select(s => s.Name));

            Console.WriteLine("Syncing house mastervalues...");
            SyncHeatingMastervalues(houseLeader, allDevices, houseMasterValues);

            var allRooms = allDevices.SelectMany(d => d.Rooms).Distinct().ToList();

            foreach (var r in allRooms)
            {
                var roomDevices = allDevices.Where(w => w.Rooms.Contains(r)).ToList();
                var roomLeader = allDevices.Where(w => w.ISEID == roomDevices.Min(min => min.ISEID)).FirstOrDefault();

                Console.WriteLine($"Syncing {r} mastervalues...");
                SyncHeatingMastervalues(roomLeader, roomDevices, roomMasterValues);
            }

            Console.WriteLine("Mastervalues across devices are in sync!");
        }
    }
}
