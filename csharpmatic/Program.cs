using csharpmatic.Properties;
using csharpmatic.XMLAPI.Generic;
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
            DeviceManager dm = new DeviceManager(Settings.Default.ServerAddress); //replace with IP of your server

            for (;;)
            {
                foreach (var e in dm.Refresh())
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
