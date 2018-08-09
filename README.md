# C# homematic IP API
C# interface to homematic ip smart home IoT devices.

This is very new code, just few days old so it changes quite quickly - lots of refactoring, so if you follow this development expect a lot of changes in next few weeks.

## Requirements:
- XML API addon 
- Raspberymatic compiled with mono support (you need git the latest version of raspberymatic, do make menuconig and select mono support; make dist and flash to sdcard, restore the backup of your devices, you should be able to sftp .exe files to your rpi3)

## Sample code

```Csharp
//connect to the XML API using csharpmatic IP ;-)
DeviceManager dm = new DeviceManager("192.168.1.200");

//refresh data set, and obtain collection of all the events
//each event contains before and after
var events = dm.Refresh(); 

//list all devices implementing temperature control interface
var hmTemp = dm.GetDevicesImplementingInterface<ITempControlDevice>();

foreach (var d in hmTemp)
	Console.WriteLine($"- [{d.Name}]: Device Type: {d.DeviceType}; Actual Temp: {d.Actual_Temperature.Value}{d.Actual_Temperature.ValueUnit}; Set Temp: {d.Set_Point_Temperature.Value}{d.Set_Point_Temperature.ValueUnit}; Boost Mode: {d.Boost_Mode.Value} {d.Boost_Time.Value}{d.Boost_Time.ValueUnit}; Window State: {d.Window_State.Value}");

//additionaly valve, switch, dimmer, humidity and generic hmip (signal, voltage) interfaces are available
```

More code is available in the samples folder (Samples.ShowInterfaces is the most juicy one)

## Supported devices

Suported devices in generic mode (developer needs to iterate over channels and datapoints of each of devices, so it's a bit akward way)
-  Homematic IP - all devices - well tested
-  possibly even older Homematic devies, I do not see reason why they would fail to work, the XML API is the same for both

Supported in Temp Control Mode:
- HmIP-ETRV (raditor thermostat)
- HmIP-ETRV-2 (raditor thermostat v2) 
- HmIP-WTH-2 (wall thermostat)
- HmIP-HEATING (virtual device)
- HmIP-SWDO (window open detector) 

Supported in Valve Control Mode:
- HmIP-ETRV (raditor thermostat)
- HmIP-ETRV-2 (raditor thermostat)
- HmIP-HEATING (virtual device)

Supported in Humidity Control Mode:
- HmIP-WTH-2 (wall thermostat with humidity sensor)

Suported Switch Actuators
- HmIP-PSM (wall switch with 1 socket)
- HmIP-PCBS (PCB board with 5-12V relay)
- HmIP-BDT (wall dimmable switch)

Supported Misc devices without group assignment:
- HmIP-SWDO (window open detector) 

In theory all devices can be supported, I just need to know the XMLAPI outputs to run them via the code generator. If you happen to be in need of getting more devives, please let me know, and I can sort it out or show you how to do it. Generator is of course included in the git.

## Roadmap

What is working:
- I can download XMLs for device, room, function and state lists and parse them into XMLAPI.CGI objects
- I can consolidate the XMLAPI.CGI objects into XMLAPI.Generic objects which contain information from all the xmls combined
- I can detect events, that is state changes of each of the datapoints.
- Code works without any delays or resouce issues on rpi3
- Dev friendly interfaces for Heating, Humidity, Lights, Security control <= built by generator based on the XMLAPI object. It should allow addition of new objects with ease.
- Support of writing commands to devices (turn on switch, set temp, etc..)

Todo:
- Algo for handling heating control, moisture control and lights/motion sensors with some predefine tresholds
- Some nice web UI with simple feedback (change temp zones/boost, lights) 
- Kafka message bus datapoints sink
- Hadoop temp zone data crunching
- Machine learning AI telling you which room is causing most of heating events
- Handling of bulgrary based on event
 
