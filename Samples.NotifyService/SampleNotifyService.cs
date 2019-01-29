using csharpmatic.Generic;
using csharpmatic.Notify;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.NotifyService
{
    class SampleNotifyService
    {
        static void Main(string[] args)
        {
            BasicConfigurator.Configure();

            var dm = new DeviceManager("192.168.1.200");

            //register notifcation service
            Slack s = Slack.TryFromCCU(dm);
            if (s != null)
                dm.RegisterNotificationService(s);

            var t1 = s.SendTextMessageAsync("message from Slack()");
            var t2 = dm.SendNotificationAsync("message from DeviceManager()");
                                   
            Task.WaitAll(t1, t2);
        }
    }
}
