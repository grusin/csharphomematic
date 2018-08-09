using csharpmatic.XMLAPI.Generic;
using csharpmatic.XMLAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.ShowInterfaces
{
    class Program
    {
        static void Main(string[] args)
        {
            //replace with IP of your rasperymatic with XML API addon
            DeviceManager dm = new DeviceManager("192.168.1.200");

            for (;;)
            {
                dm.Refresh();
                Console.Clear();
                Console.WriteLine("{0}\n", "".PadRight(80, '#'));

                //IHmIPDevice devices (should be all devices on my network)
                var hmIp = dm.GetDevicesImplementingInterface<IHmIPDevice>();
                Console.WriteLine("Devices implementing IHmIPDevice interface: {0}", hmIp.Count);

                foreach (var d in hmIp)
                    Console.WriteLine($"- [{d.Name}]: Room: {d.Rooms.FirstOrDefault()}; Function: {d.Functions.FirstOrDefault()}; Device Type: {d.DeviceType}; RSSI: {d.Rssi_Device.Value}; Voltage: {d.Operating_Voltage.Value}");

                //IValveControlDevice 
                var hmValve = dm.GetDevicesImplementingInterface<IValveControlDevice>();
                Console.WriteLine("\nDevices implementing IValveControlDevice interface: {0}", hmValve.Count);

                foreach (var d in hmValve)
                    Console.WriteLine($"- [{d.Name}]: Device Type: {d.DeviceType}; Valve%: {d.Level.Value}; Valve Status: {d.Level_Status.Value}; Valve Adaptation: {d.Valve_Adaption.Value}; Valve State: {d.Valve_State.Value}");

                //ITempControlDevice 
                var hmTemp = dm.GetDevicesImplementingInterface<ITempControlDevice>();
                Console.WriteLine("\nDevices implementing ITempControlDevice interface: {0}", hmTemp.Count);

                foreach (var d in hmTemp)
                    Console.WriteLine($"- [{d.Name}]: Device Type: {d.DeviceType}; Actual Temp: {d.Actual_Temperature.Value}{d.Actual_Temperature.ValueUnit}; Set Temp: {d.Set_Point_Temperature.Value}{d.Set_Point_Temperature.ValueUnit}; Boost Mode: {d.Boost_Mode.Value} {d.Boost_Time.Value}{d.Boost_Time.ValueUnit}; Window State: {d.Window_State.Value}");

                //ISingleSwitchControlDevice 
                var hm1Switch = dm.GetDevicesImplementingInterface<ISingleSwitchControlDevice>();
                Console.WriteLine("\nDevices implementing ISingleSwitchControlDevice interface: {0}", hm1Switch.Count);

                foreach (var d in hm1Switch)
                    Console.WriteLine($"- [{d.Name}]: Device Type: {d.DeviceType}; Switch State: {d.State}");

                //IDimmerDevice
                var hmDimmer = dm.GetDevicesImplementingInterface<IDimmerDevice>();
                Console.WriteLine("\nDevices implementing IDimmerDevice interface: {0}", hmDimmer.Count);

                foreach (var d in hmDimmer)
                    Console.WriteLine($"- [{d.Name}]: Device Type: {d.DeviceType}; Dimmer%: {d.Level.Value}; Ramp time: {d.Ramp_Time.Value}");

                //IHumidityControlDevice 
                var hmHumidity = dm.GetDevicesImplementingInterface<IHumidityControlDevice>();
                Console.WriteLine("\nDevices implementing IHumidityControlDevice interface: {0}", hmHumidity.Count);

                foreach (var d in hmHumidity)
                    Console.WriteLine($"- [{d.Name}]: Device Type: {d.DeviceType}; Humidity: {d.Humidity.Value}{d.Humidity.ValueUnit}; Humidity Status: {d.Humidity_Status.Value}");

                Thread.Sleep(1000);
                Console.WriteLine("\n\n");
            }
        }
    }
}
