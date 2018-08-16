using csharpmatic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Generic
{
    public class Room
    {
        public string Name { get; private set; }

        public string ISEID { get; private set; }

        public DeviceGroup<IHmDevice> HmDevices { get; private set; }

        public DeviceGroup<IHmIPDevice> HmIPDevices { get; private set; }

        public DeviceGroup<IValveControlDevice> ValveControlDevices { get; private set; }

        public DeviceGroup<ITempControlDevice> TempControlDevices { get; private set; }

        public DeviceGroup<ISingleSwitchControlDevice> SingleSwitchControlDevices { get; private set; }

        public DeviceGroup<IDimmerDevice> DimmerDevices { get; private set; }

        public DeviceGroup<IHumidityControlDevice> HumidityControlDevices { get; private set; }

        public Room(string roomName, string roomIseId, DeviceManager dm)
        {
            Name = roomName;
            ISEID = roomIseId;
          
            HmDevices = new DeviceGroup<IHmDevice>(dm.GetDevicesImplementingInterface<IHmDevice>().Where(w=>w.Rooms.Contains(roomName)));
            HmIPDevices = new DeviceGroup<IHmIPDevice>(dm.GetDevicesImplementingInterface<IHmIPDevice>().Where(w => w.Rooms.Contains(roomName)));
            ValveControlDevices = new DeviceGroup<IValveControlDevice>(dm.GetDevicesImplementingInterface<IValveControlDevice>().Where(w => w.Rooms.Contains(roomName)));
            TempControlDevices = new DeviceGroup<ITempControlDevice>(dm.GetDevicesImplementingInterface<ITempControlDevice>().Where(w => w.Rooms.Contains(roomName)));
            SingleSwitchControlDevices = new DeviceGroup<ISingleSwitchControlDevice>(dm.GetDevicesImplementingInterface<ISingleSwitchControlDevice>().Where(w => w.Rooms.Contains(roomName)));
            DimmerDevices = new DeviceGroup<IDimmerDevice>(dm.GetDevicesImplementingInterface<IDimmerDevice>().Where(w => w.Rooms.Contains(roomName)));
            HumidityControlDevices = new DeviceGroup<IHumidityControlDevice>(dm.GetDevicesImplementingInterface<IHumidityControlDevice>().Where(w => w.Rooms.Contains(roomName)));
        }
    }
}

