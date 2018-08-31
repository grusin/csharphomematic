using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using csharpmatic.Interfaces;

namespace csharpmatic.Generic
{
    public class DeviceGroup<T> : IEnumerable<T> where T : IHmDevice
    {
        public Dictionary<string, T> DevicesByISEID { get; private set; }
        public List<T> Devices { get; private set; }
        public T GroupLeader { get; private set; }

        public DeviceGroup(IEnumerable<T> deviceList)
        {
            Devices = new List<T>(deviceList);
            DevicesByISEID = deviceList
                .ToDictionary(ks => ks.ISEID);

            GroupLeader = deviceList.Where(w => w.Interface == "VirtualDevices").FirstOrDefault();

            if(GroupLeader == null)
                GroupLeader = deviceList.OrderBy(ob => ob.ISEID).FirstOrDefault();
        }

        public void SetRoomValue(Datapoint dp, object newValue)
        {
            dp.SetRoomValue(newValue, typeof(T));
        }

        public void SetRoomValue<U>(TypedDatapoint<U> dp, U newValue)
        {
            dp.SetRoomValue(newValue, typeof(T));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Devices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Devices.GetEnumerator();
        }
    }
}
