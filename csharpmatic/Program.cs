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
            DeviceManager dm = new DeviceManager("192.168.1.200");

            Stopwatch watch = new Stopwatch();


            for (;;)
            {
                watch.Start();

                var events = dm.Refresh();

                watch.Stop();
                Console.WriteLine("");
                Console.WriteLine("###### download time: {0}", watch.Elapsed);
                watch.Restart();

                if (events.Count > 0)
                {
                    Console.WriteLine("Got {0} events", events.Count);

                    foreach(var e in events)
                    {
                        Console.WriteLine("--- Event ---");
                        Console.WriteLine("  Current: {0}", e.Current);
                        Console.WriteLine("  Prev: {0}", e.Previous == null ? null : e.Previous);
                    }
                }

                watch.Stop();
                Console.WriteLine("###### compute time: {0}", watch.Elapsed);
                Console.WriteLine("");

                Thread.Sleep(1000);            
            }            
        }      
    }
}
