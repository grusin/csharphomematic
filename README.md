# C# homematic IP API
C# interface to homematic ip smart home IoT devices, just wrote it beacause I hate writing software in "IDE" that does no support syntax highlihting or debugging or generaly anything that just give you QA.

This is very new code, just few days old, so i did not have time polish it up.

Requirements:
- XML API addon 
- Raspberymatic compiled with mono support (git the latest version, menu gconig and select mono support; make dist and flash; - manual in progress)

What is working:
- I can download XMLs for device, room, function and state lists and parse them into XMLAPI.CGI objects
- I can consolidate the XMLAPI.CGI objects into XMLAPI.Genric objects which contaain information from all the xmls combined
- I can detect events, that is state changes of each of the datapoints

Todo:
- Dev friendly interfaces for Heating, Humidity, Lights, Security control
- Algo for handling heating control, moisture control and lights/motion sensors with some predefine tresholds
- Some nice angularJs UI with feedback (change temp zones/boost) 
- Kafka message bus datapoints sink
- Hadoop temp zone data crunching
- Machine learning AI telling you which room is causing most of heating events
- Handling of bulgrary based on event
 
