using csharpmatic.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.ShowEvents
{
    class SampleShowEvents
    {
        static void Main(string[] args)
        {
            //replace with IP of your rasperymatic with XML API addon
            DeviceManager dm = new DeviceManager("192.168.1.200");

            for (;;)
            {
                //get events since last refresh, DeviceManager will determine changed events for you
                //and only return the list of state changes, with current and previous data point values
                Stopwatch t = new Stopwatch();
                t.Start();
                List<DatapointEvent> eventsSinceLastRefresh = dm.Refresh();
                t.Stop();
                Console.WriteLine("Refresh took {0} ms", t.ElapsedMilliseconds);

                foreach (var e in eventsSinceLastRefresh)
                {
                    Console.WriteLine("{0} Event {1}\t\t{2} => {3}",
                        e.EventTimestamp.ToString("o"),
                        e.Current.Name,
                        e.Previous == null ? null : e.Previous.Value, e.Current.Value
                    );

                }

                Thread.Sleep(1000);
            }
        }
    }
}
