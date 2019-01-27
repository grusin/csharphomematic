using csharpmatic.Automation;
using csharpmatic.Generic;
using csharpmatic.Interfaces.Devices;
using csharpmaticAutomation.Alarm;
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

            AlarmAutomation a = new AlarmAutomation(dm, AutomationNames.AlarmAutomation);

            for (; ; )
            {
                if (!a.AlarmArmed)
                {
                    for (; ; )
                    {
                        dm.Work();

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
                    dm.Work();

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
