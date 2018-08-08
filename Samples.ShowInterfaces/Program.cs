using csharpmatic.XMLAPI.Generic;
using csharpmatic.XMLAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.ShowInterfaces
{
    class Program
    {
        static void Main(string[] args)
        {
            //replace with IP of your rasperymatic with XML API addon
            DeviceManager dm = new DeviceManager("192.168.1.200");

            ShowIHmIPDevices(dm);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void ShowIHmIPDevices(DeviceManager dm)
        {
            var list = dm.GetDevicesImplementingInterface<IHmIPDevice>();
            Console.WriteLine("Devices implementing IHmIPDevice interface: {0}", list.Count);

            foreach(var d in list)
                Console.WriteLine($"- [{d.Name}]: Room: {d.Rooms.FirstOrDefault()}; Function: {d.Functions.FirstOrDefault()}; Device Type: {d.DeviceType}; RSSI: {d.Rssi_Device.Value}; Voltage: {d.Operating_Voltage.Value}");
        }
    }
}
