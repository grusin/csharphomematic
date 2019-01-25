using csharpmatic.Generic;
using csharpmatic.Interfaces.Devices;
using csharpmaticAutomations.Alarm;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.Alarm
{
    class Program
    {
        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            BasicConfigurator.Configure();

            DeviceManager dm = new DeviceManager("192.168.1.200");

            AlarmAutomation a = new AlarmAutomation(dm);


            //dm.Refresh();
            //a.Disarm();

            var x = dm.GetDevicesImplementingInterface<HMIP_SWSD>().FirstOrDefault();

            x.Smoke_Detector_Command.Value = csharpmatic.Interfaces.ISmokeDetectorDevice_Smoke_Detector_Command_Enum.INTRUSION_ALARM;

            Thread.Sleep(5000);

            x.Smoke_Detector_Command.Value = csharpmatic.Interfaces.ISmokeDetectorDevice_Smoke_Detector_Command_Enum.INTRUSION_ALARM_OFF;

            for (; ; )
            {
                if (!a.AlarmArmed)
                {
                    for (; ; )
                    {
                        dm.Refresh();
                        var list = a.Arm();

                        if (a.AlarmArmed)
                            break;

                        LOGGER.Info("Retrying ARM in 3 seconds...");

                        Thread.Sleep(3000);
                    }
                }

                int counter = 0;

                for (; ; )
                {
                    dm.Refresh();
                    a.Work();

                    if(a.AlarmArmed)
                        counter++;

                    Thread.Sleep(1000);

                    if (counter >= 30 && a.AlarmArmed)
                    {
                        LOGGER.Info("Auto DISARM after 10 seconds (DEBUG)");
                        a.Disarm();
                        break;
                    }
                }
            }
        }
    }
}
