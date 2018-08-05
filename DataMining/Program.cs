using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining
{
    class Program
    {
        static void Main(string[] args)
        {
            DeviceManager dm = new DeviceManager("192.168.1.200");
            dm.Refresh();

            //<datapoint type, interface, channel index

            var data = new List<List<string>>();
            
            foreach (var d in dm.Devices)
            {
                foreach (var c in d.Channels)
                {
                    foreach (var dp in c.Datapoints.Values)
                    {
                        data.Add(new List<string>() { d.Interface, c.ChannelIndex.ToString(), dp.Type });
                    }
                }
            }

            //data.Where(w => w.in)

        } 
    }
}
