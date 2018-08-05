using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Generic
{
    public class Device
    {
        public Channel[] Channels { get; private set; }
        [JsonIgnore]
        public Dictionary<string, Channel> ChannelsByISEID { get; private set; }
        public string Name { get; private set; }
        public string ISEID { get; private set; }

        public bool Reachable { get; private set; }

        public bool PendingConfig { get; private set; }

        public string Address { get; private set; }
                
        public string Interface { get; private set; }
        

        public string DeviceType { get; private set; }

        public bool ReadyConfig { get; private set; }            

        HashSet<string> Rooms { get { return new HashSet<string>(Channels.SelectMany(c => c.Rooms)); } }

        HashSet<string> Functions { get { return new HashSet<string>(Channels.SelectMany(c => c.Functions)); } }

        ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Device(CGI.DeviceList.Device d, CGI.CGIClient CGIClient)
        {
            FillFromDeviceListDevice(d);
            FillFromRoomList(CGIClient.RoomList);
            FillFromFunctionList(CGIClient.FunctionList);
            FillFromStateList(CGIClient.StateList);
        }

        public Datapoint GetDatapointByType(string type)
        {
            Datapoint dp = null;

            foreach(Channel c in Channels)
            {
                if (c.Datapoints.TryGetValue(type, out dp))
                    return dp;
            }

            return null;            
        }

        private void FillFromDeviceListDevice(CGI.DeviceList.Device d)
        {
            Name = d.Name;
            ISEID = d.Ise_id;
            Reachable = false; //todo from state list
            PendingConfig = true; //todo from state list
            Address = d.Address;
            Interface = d.Interface;
            DeviceType = d.Device_type;
            ReadyConfig = String.IsNullOrWhiteSpace(d.Ready_config) ?  false : Convert.ToBoolean(d.Ready_config);

            ChannelsByISEID = new Dictionary<string, Channel>();

            Channels = new Channel[d.Channel.Count];
            for (int i = 0; i < Channels.Length; i++)
            {
                Channels[i] = new Channel(d.Channel[i], this);
                if(!ChannelsByISEID.ContainsKey(Channels[i].ISEID))
                    ChannelsByISEID.Add(Channels[i].ISEID, Channels[i]);
                else
                    throw new Exception(String.Format("Duplicated channel iseid {0} on device {1}.", Channels[i].ISEID, d.Name));                   
            }  
        }

        private void FillFromRoomList(CGI.RoomList.RoomList roomList)
        {
            foreach(var room in roomList.Room)
            {
                foreach(var channel in room.Channel)
                {
                    Channel c = null;
                    if (ChannelsByISEID.TryGetValue(channel.Ise_id, out c))
                    {
                        if (!c.Rooms.Contains(room.Name))
                        {
                            c.Rooms.Add(room.Name);
                        }
                    }                   
                }
            }
        }

        private void FillFromFunctionList(CGI.FunctionList.FunctionList funcList)
        {
            foreach(var function in funcList.Function)
            {
                foreach (var channel in function.Channel)
                {
                    Channel c = null;
                    if (ChannelsByISEID.TryGetValue(channel.Ise_id, out c))
                    {
                        if(!c.Functions.Contains(function.Name))
                        {
                            c.Functions.Add(function.Name);
                        }
                    }
                }
            }
        }

        private void FillFromStateList(CGI.StateList.Device d)
        {
            PendingConfig = String.IsNullOrWhiteSpace(d.Config_pending) ? false : Convert.ToBoolean(d.Config_pending);
            Reachable = String.IsNullOrWhiteSpace(d.Unreach) ? true : !Convert.ToBoolean(d.Unreach);

            foreach (var c in d.Channel)
            {
                var dc = ChannelsByISEID[c.Ise_id];
                dc.UpdateDataPoints(c.Datapoint);
            }
        }

        private void FillFromStateList(CGI.StateList.StateList stateList)
        {
            foreach(var d in stateList.Device)
                if(d.Ise_id == ISEID)
                    FillFromStateList(d);    
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }  
}
