using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Generic
{
    public class DeviceManager
    {
        private CGI.CGIClient CGIClient;
        public List<Device> Devices { get; private set; }
        public Dictionary<string, Device> DevicesByISEID { get; private set ;}
        private List<Device> PrevDevices { get; set; }

        public Dictionary<string, Device> PrevDevicesByISEID { get; private set; }
        
        public DeviceManager(string serverAddress)
        {
            CGIClient = new XMLAPI.CGI.CGIClient("http://" + serverAddress);
            Devices = new List<Device>();

            CGIClient.FetchData();
        }

        public List<DatapointEvent> Refresh()
        {
            CGIClient.FetchData();
            BuildDeviceList();
            return GetEvents();
        }

        private List<DatapointEvent> GetEvents()
        {
            List<DatapointEvent> list = new List<DatapointEvent>();

            //first run
            if (PrevDevicesByISEID == null)
                return list;
                        
            foreach (var d in Devices)
            {
                foreach (var c in d.Channels)
                {
                    foreach (var dpkvp in c.Datapoints)
                    {
                        Datapoint current = dpkvp.Value;
                                                                              
                        Datapoint prev = FindPreviousDatapoint(current);

                        //new device was just added
                        if (prev == null)
                            list.Add(new DatapointEvent(current, null));
                        else if (current.InternalValue != prev.InternalValue || current.OperationsCounter != prev.OperationsCounter)
                            list.Add(new DatapointEvent(current, prev));                        
                    }
                }
            }

            return list;
        }

        public Datapoint FindPreviousDatapoint(Datapoint current)
        {
            if (PrevDevicesByISEID == null)
                return null;

            Device d = null;

            if(PrevDevicesByISEID.TryGetValue(current.Channel.Device.ISEID, out d))
            {
                Channel c = null;
                if(d.ChannelsByISEID.TryGetValue(current.Channel.ISEID, out c))
                {
                    Datapoint dp = null;
                    if(c.Datapoints.TryGetValue(current.Type, out dp))
                    {
                        return dp;
                    }
                }
            }

            return null;
        }
              

        private void BuildDeviceList()
        {
            PrevDevices = Devices;
            PrevDevicesByISEID = DevicesByISEID;

            Devices = new List<Device>();
            DevicesByISEID = new Dictionary<string, Device>();

            foreach (var d in CGIClient.DeviceList.Device)
            {                
                //Device gd = new Device(d, CGIClient);

                Device gd = Interfaces.DeviceFactory.CreateInstance(d, CGIClient);
                                
                Devices.Add(gd);
                DevicesByISEID.Add(gd.ISEID, gd);
            }                           
        }
    }
}
