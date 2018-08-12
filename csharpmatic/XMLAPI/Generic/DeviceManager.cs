using csharpmatic.XMLAPI.Interfaces;
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

        public List<DatapointEvent> Events { get; private set; }

        public Dictionary<string, Device> PrevDevicesByISEID { get; private set; }

        public Uri HttpServerUri { get { return CGIClient.HttpServerUri; } }
        
        public List<Room> Rooms { get; private set; }
        public Dictionary<string, Room> RoomsByName { get; private set; }
                
        public DeviceManager(string serverAddress)
        {
            CGIClient = new XMLAPI.CGI.CGIClient("http://" + serverAddress);
            Devices = new List<Device>();

            CGIClient.FetchData();

            Refresh();
        }

        public List<T> GetDevicesImplementingInterface<T>() where T : class
        {
            List<T> list = new List<T>();

            return Devices.Where(w => w is T).Select(s => s as T).ToList();
        }

        public List<DatapointEvent> Refresh()
        {
            CGIClient.FetchData();
            BuildDeviceList();
            BuildRoomList();
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
                        else if (current.GetValueString() != prev.GetValueString()|| current.OperationsCounter != prev.OperationsCounter)
                            list.Add(new DatapointEvent(current, prev));                        
                    }
                }
            }

            Events = list;

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
                if(d.ChannelByISEID.TryGetValue(current.Channel.ISEID, out c))
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

        private void BuildRoomList()
        {
            Rooms = new List<Room>();
            RoomsByName = new Dictionary<string, Room>();

            foreach (var cgiroom in CGIClient.RoomList.Room)
            {
                Room r = new Room(cgiroom.Name, cgiroom.Ise_id, this);
                Rooms.Add(r);
                RoomsByName.Add(r.Name, r);
            }
        }

        private void BuildDeviceList()
        {
            PrevDevices = Devices;
            PrevDevicesByISEID = DevicesByISEID;

            Devices = new List<Device>();
            DevicesByISEID = new Dictionary<string, Device>();

            foreach (var d in CGIClient.DeviceList.Device)
            {                
                Device gd = DeviceFactory.CreateInstance(d, CGIClient, this);
                                
                Devices.Add(gd);
                DevicesByISEID.Add(gd.ISEID, gd);
            }                           
        }
    }
}
