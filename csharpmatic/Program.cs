using csharpmatic.Properties;
using csharpmatic.XMLAPI.Generic;
using csharpmatic.XMLAPI.Interfaces;
//using csharpmatic.XMLAPI.Interfaces.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csharpmatic
{
    class Program
    {
        static void Main(string[] args)
        {
            //replace with IP of your rasperymatic with XML API addon
            DeviceManager dm = new DeviceManager("192.168.1.200"); 

            for (;;)
            {

                List<DatapointEvent> eventsSinceLastRefresh = dm.Refresh();  
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
