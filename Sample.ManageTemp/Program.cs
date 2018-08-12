using csharpmatic.XMLAPI.Generic;
using csharpmatic.XMLAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.ManageTemp
{
    class Program
    { 

        static void Main(string[] args)
        {
            //both pieces of code assume that room "Living room and Kitchen" contains at least one device of interface ITempControlDevice
            //both pieces of code do the same logic, just with different API

            //test_room_api();
            test_device_api();
        }

        /*
        static void test_room_api()
        {
            DeviceManager dm = new DeviceManager("192.168.1.200");

            bool doOnce = false;

            for (;;)
            {
                dm.Refresh();
                var devs = dm.RoomsByName["Living room and Kitchen"].TempControlDevices;

                foreach (var d in devs)
                    Console.WriteLine($"- [{d.Name}]: Device Type: {d.DeviceType}; Actual Temp: {d.Actual_Temperature.Value}; Set Temp: {d.Set_Point_Temperature.Value}{d.Set_Point_Temperature.ValueUnit}; Boost Mode: {d.Boost_Mode.Value} {d.Boost_Time.Value}{d.Boost_Time.ValueUnit}; Window State: {d.Window_State.Value}");
 
                //just run this once, so that we dont flip the temp every few seconds
                if (doOnce == false)
                {
                    doOnce = true;

                    //get room leader
                    var leader = devs.GroupLeader;
                    Console.Write($"leader is: {leader.Name}; changing temp from: ${leader.Set_Point_Temperature.Value} to ");

                    //temperature "logic"; generaly here it should be something that actualy has sense ;)
                    //but this temp flips will do just fine for demo
                    if (leader.Set_Point_Temperature.Value >= 25)
                        devs.SetDatapoint(leader.Set_Point_Temperature, leader.Set_Point_Temperature.Value -= 6);
                    else
                        devs.SetDatapoint(leader.Set_Point_Temperature, leader.Set_Point_Temperature.Value += 3);

                    Console.WriteLine($"{leader.Set_Point_Temperature.Value}");                    
                }
            }
        }
        */

        //this is the way you would go and change a temp, on all divices in a room, one by one, without using any higher level api
        static void test_device_api()
        {
            //replace with IP of your rasperymatic with XML API addon
            DeviceManager dm = new DeviceManager("192.168.1.200");                     

            bool doOnce = false;

            for (;;)
            {
                dm.Refresh();

                //get devices that implement ISingleSwitchControlDevice interface
                var list = dm.GetDevicesImplementingInterface<ITempControlDevice>();

                //get all devices in Room "Living room and Kitchen"
                var devs = list.Where(w => w.Rooms.Contains("Living room and Kitchen")).OrderBy(o => o.ISEID).ToList();

                foreach (var d in devs)
                    Console.WriteLine($"- [{d.Name}]: Device Type: {d.DeviceType}; Actual Temp: {d.Actual_Temperature.Value}; Set Temp: {d.Set_Point_Temperature.Value}{d.Set_Point_Temperature.ValueUnit}; Boost Mode: {d.Boost_Mode.Value} {d.Boost_Time.Value}{d.Boost_Time.ValueUnit}; Window State: {d.Window_State.Value}");
 
                //just run this once, so that we dont flip the temp every few seconds
                if (doOnce == false)
                {
                    doOnce = true;

                    //get room leader
                    var leader = devs.Where(w => w.ISEID == devs.Min(m => m.ISEID)).FirstOrDefault();
                    Console.Write($"leader is: {leader.Name}; changing temp from: ${leader.Set_Point_Temperature.Value} to ");

                    //temperature "logic"; generaly here it should be something that actualy has sense ;)
                    //but this temp flips will do just fine for demo
                    if (leader.Set_Point_Temperature.Value >= 25)
                        leader.Set_Point_Temperature.Value -= 6;
                    else
                        leader.Set_Point_Temperature.Value += 3;

                    Console.WriteLine($"{leader.Set_Point_Temperature.Value}");

                    //temp was updated only in the first device, iterate over remaning devices to update temp there too
                    foreach(var s in devs)
                    {
                        if (s.ISEID != leader.ISEID)
                            s.Set_Point_Temperature.Value = leader.Set_Point_Temperature.Value;
                    }                    
                }
                
                Console.WriteLine("\n\n");

                Thread.Sleep(2000);
            }
        }
    }
}
