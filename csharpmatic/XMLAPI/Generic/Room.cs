
/*
 * 
using csharpmatic.XMLAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Generic
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
            
            var list = dm.Devices.Where(d => d.Rooms.Contains(roomName)).ToList();

            HmDevices = new DeviceGroup<IHmDevice>(dm.GetDevicesImplementingInterface<IHmDevice>());
            HmIPDevices = new DeviceGroup<IHmIPDevice>(dm.GetDevicesImplementingInterface<IHmIPDevice>());
            ValveControlDevices = new DeviceGroup<IValveControlDevice>(dm.GetDevicesImplementingInterface<IValveControlDevice>());
            TempControlDevices = new DeviceGroup<ITempControlDevice>(dm.GetDevicesImplementingInterface<ITempControlDevice>());
            SingleSwitchControlDevices = new DeviceGroup<ISingleSwitchControlDevice>(dm.GetDevicesImplementingInterface<ISingleSwitchControlDevice>());
            DimmerDevices = new DeviceGroup<IDimmerDevice>(dm.GetDevicesImplementingInterface<IDimmerDevice>());
            HumidityControlDevices = new DeviceGroup<IHumidityControlDevice>(dm.GetDevicesImplementingInterface<IHumidityControlDevice>());
        }
    }
}

*/