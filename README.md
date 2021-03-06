# C# homematic IP API
C# interface to homematic ip [smart home IoT devices](https://www.eq-3.com/products/homematic-ip.html).

Code has been tested only on Homematic IP devices, but I believe it should work also on other, non HmIP devices that are supported by XMLAPI.

## Sample code
I know you all care about what you can do, so here it goes... I hope you like it.

```Csharp
//connect to the XML API using csharpmatic IP ;-)
DeviceManager dm = new DeviceManager("192.168.1.200");

//refresh data set, and obtain collection of all the events
//each event contains before and after datapoint
var events = dm.Refresh(); 

//list all devices implementing temperature control interface
var hmTemp = dm.GetDevicesImplementingInterface<ITempControlDevice>();

foreach (var d in hmTemp)
	Console.WriteLine($"- [{d.Name}]: Device Type: {d.DeviceType}; Actual Temp: {d.Actual_Temperature.Value}{d.Actual_Temperature.ValueUnit}; Set Temp: {d.Set_Point_Temperature.Value}{d.Set_Point_Temperature.ValueUnit}; Boost Mode: {d.Boost_Mode.Value} {d.Boost_Time.Value}{d.Boost_Time.ValueUnit}; Window State: {d.Window_State.Value}");

//additionaly valve, switch, dimmer, humidity and generic hmip (signal, voltage) interfaces are available

//change state of all the switches
var listSwitches = dm.GetDevicesImplementingInterface<ISingleSwitchControlDevice>();
foreach (var sw in listSwitches)
	sw.State.Value = !sw.State.Value;

//turn on heating automation (you can instantiate automation for humidity, water level, etc.., with the same generic logic too!)
var heatingAutomation = new ActuatorSensorAutomation<IValveControlDevice>(dm, "Heating", "LEVEL");
heatingAutomation.RefencePoint = 20; //20% valve open
heatingAutomation.Hysteresis = 2;
heatingAutomation.MaxOnTime = new TimeSpan(0, 5, 0);
heatingAutomation.MinOnTime = new TimeSpan(0, 0, 30);
heatingAutomation.MinOnTime = new TimeSpan(0, 3, 0);

for (;;)
{
    //pull latest data from the web services
    dm.Refresh();

    //Make sure that devices have synced master values, so that temp setup is not broken
    SyncHeatingMasterValuesAutomation.SyncHeatingMastervalues(dm);

    //Check if heating needs to be turned on/off
    heatingAutomation.Work();

    //wait a bit before running again
    Thread.Sleep(3000);
}
```

More code is available in the samples folder

## Requirements:
- [XML API addon](https://github.com/grusin/XML-API) - forked repo, original does not support HmIP port numbers, I made PR to author to merge changes.
- [RPI HmIP RF addon board - Pi HM-MOD-RPI-PCB](https://www.elv.de/homematic-funkmodul-fuer-raspberry-pi-bausatz.html) and some basic soldering skills (it comes in two pieces, RF module and addon board with caps allowing it to connect to rpi3 directly)
- [Raspberymatic](https://github.com/jens-maus/RaspberryMatic) compiled with mono support (you need git the latest version of raspberymatic, do make menuconig and select mono support; make dist and flash obtained sdcard.img, then restore the backup of your devices, at this point you should be able to sftp & mono your.exe files on rpi3)
- About 90 MB - 150MB of free RAM on your RPI. CPU is not an issue at all (0.1% avg). It should be OK for all default installs. I am regulary checking for mem/resource leaks and I find none, so I think my code is good quality ;-)
- Code also runs on windows/linux, so you can run all code on your desktop/laptop in visual studio/monodevelop and debug it there just as if code is running on rpi3. There are no local/filesystem dependencies and I will keep it this way, because I value debugability.

## Supported devices

### Suported devices in generic mode (developer needs to iterate over channels and datapoints of each of devices, so it's a bit akward way, but it seems all APIs use it, so I have it too)
-  Homematic IP - all devices - well tested
-  possibly even older Homematic devies, I do not see reason why they would fail to work, the XML API is the same for both HmIP and older models.

### Supported in Temp Control Mode:
- HmIP-ETRV (raditor thermostat)
- HmIP-ETRV-2 (raditor thermostat v2) 
- HmIP-WTH-2 (wall thermostat)
- HmIP-HEATING (virtual device)
- HmIP-SWDO (window open detector) 

### Supported in Valve Control Mode:
- HmIP-ETRV (raditor thermostat)
- HmIP-ETRV-2 (raditor thermostat)
- HmIP-HEATING (virtual device)

### Supported in Humidity Control Mode:
- HmIP-WTH-2 (wall thermostat with humidity sensor)

### Suported Switch Actuators
- HmIP-PSM (wall switch with 1 socket)
- HmIP-PCBS (PCB board with 5-12V relay)
- HmIP-BDT (wall dimmable switch)

### Supported Misc devices without group assignment:
- HmIP-SWDO (window open detector) 

In theory all devices can be supported, I just need to know the XML API outputs to run them via the interface generator. If you happen to be in need of getting more devives, please let me know (realy, don't hasitate) and I can sort it out or show you how to do it if you are developer. Generator is of course included in the git.

**Note** Generator often needs Interface/Devices folder to be cleaned - all files deleted. This is required if there is mismatch of types, fields, c# interfaces, etc... Since parent project will fail to compile, generator will fail to build and it wont generate anything. Since generator does not use any of the interfaces, it's fine to delete all these files and rebuild csharpmatic. Once generator is run, it will create all files again. I do not know yet how to automate that event, so if you happen to know how to do it, drop me a note. Please!

## Roadmap

### What is working:
- I can download XMLs for device, room, function and state lists and parse them into XMLAPI.CGI objects
- I can consolidate the XMLAPI.CGI objects into XMLAPI.Generic objects which contain information from all the xmls combined
- I can detect events, that is state changes of each of the datapoints.
- Code works without any delays or resouce issues on rpi3
- Dev friendly interfaces for Heating, Humidity, Lights, Security control, Switches <= built by generator based on the XMLAPI object. It should allow addition of new objects with ease as long as developer has actual devices connected to this rpi3
- Support of writing commands to devices (turn on switch, set temp, etc..) using simple setter.
- Simple algo for for handling heating control, moisture control. It uses logic of detecting actuators and sensors allocated to the same function, across different rooms, and then determines based on given inital conditions if actuators needs to be turned on or off. Algo uses hysteresis and min/max on/off times, to make sure you don't get disco effect :)
- Synchronization of master values related to heating. The time ranges for temp profiles are synced across whole house, but the temperature in each of the rooms is kept seperate and synced. House leader (the one whose time ranges are a referece) is heating device with lowest ISEID. Simillary each room leader is a device in a room with the lowest ISEID.

### Todo:
- lights/motion sensors with some predefine tresholds
- Some nice web UI with simple feedback (change temp zones/boost, lights) - [onsen.io](https://onsen.io/) is my choice
- Kafka message bus datapoints sink
- Hadoop temp zone data crunching
- Machine learning AI telling you which room is causing most of heating events
- Handling of bulgrary based on event
 
 ## License
 All code is open source, licensed as GPLv3. That means that if you fry your hardware, or blow up your light bulbs, or make your house cold like north pole, or sahara desert, cause alarm that will annoy neighbours, etc... it's on you, not me, yes on you!. Enjoy :)
