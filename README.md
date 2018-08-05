# C# homematic IP API
C# interface to homematic ip smart home IoT devices, just wrote it beacause I hate writing software in "IDE" that does no support syntax highlihting or debugging or generaly anything that just give you QA.

This is very new code, just few days old, so i did not have time polish it up.

Requirements:
- XML API addon 
- Raspberymatic compiled with mono support (git the latest version, menu gconig and select mono support; make dist and flash; - manual in progress)

Suported devices in generic mode (developer needs to iterate over channels and datapoints of each of devices)
-  Homematic IP - well tested
-  possibly even older Homematic devies, I do not see reason why they would fail to work, the XML API is the same for both

Supported in Temp Control Mode:
- HmIP-ETRV (read only so far, raditor thermostat)
- HmIP-ETRV-2 (read only so far raditor thermostat v2) 
- HmIP-WTH-2 (read only so far, wall thermostat)
- HmIP-HEATING (read only so far, virtual device)

Supported in Valve Control Mode:
- HmIP-ETRV (read only so far, raditor thermostat)
- HmIP-ETRV-2 (read only so far, raditor thermostat)
- HmIP-HEATING (read only so far, virtual device)

Supported in Humidity Control Mode:
- HmIP-WTH-2 (read only so far, wall thermostat with humidity sensor)

Not Suported Switch Actuators
- HmIP-PSM (wall switch with 1 socket)
- HmIP-Cant-Remember-Name (Development board with 12V relay)

Not Supported Security:
- HmIP-SWDO (window open detector) 

In theory all devices can be supported, I just need to know the XMLAPI outputs to run them via the code generator. If you happen to be in need of getting more devives, please let me know, and we can sort it out.

What is working:
- I can download XMLs for device, room, function and state lists and parse them into XMLAPI.CGI objects
- I can consolidate the XMLAPI.CGI objects into XMLAPI.Generic objects which contain information from all the xmls combined
- I can detect events, that is state changes of each of the datapoints.
- Code works without any delays or resouce issues on rpi3
- Dev friendly interfaces for Heating, Humidity, Lights, Security control <= built by generator based on the XMLAPI object. It should allow addition of new objects with ease.

Todo:
- Support of writing commands to devices (turn on switch, set temp, etc..)
- Algo for handling heating control, moisture control and lights/motion sensors with some predefine tresholds
- Some nice web UI with simple feedback (change temp zones/boost, lights) 
- Kafka message bus datapoints sink
- Hadoop temp zone data crunching
- Machine learning AI telling you which room is causing most of heating events
- Handling of bulgrary based on event
 
