using csharpmatic.XMLAPI.Generic;
using csharpmatic.XMLAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.ToggleButton
{
    class Program
    {        static void Main(string[] args)
        {
            //replace with IP of your rasperymatic with XML API addon
            DeviceManager dm = new DeviceManager("192.168.1.200");

            //get devices that implement ISingleSwitchControlDevice interface
            var list = dm.GetDevicesImplementingInterface<ISingleSwitchControlDevice>();

            foreach (var sw in list)
            {
                //'State' is the datapoint that controls the switch position 
                //Each datapoint always has .Value, which is represending the underlying value. Other fields inside of datapoint show the unit, type, etc... 
                //each of datapoints .Values can be read or set. Setting a value will imediately issue statechange request to XML API.
                sw.State.Value = !sw.State.Value;

                //it is not guaranteed that state has been changed yet, as there might be RF interference, you need to use guarantee API.
                //guarantee API is not implemented yet ;-)
            }
        }
    }
}
