using csharpmatic.Interfaces;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Generic
{
    public class Device : IHmDevice
    {
        public const String NoSensorDeviceFunction = "NOSENSOR";
        public const String NoActuatorDeviceFunction = "NOACTUATOR";

        public Channel[] Channels { get; private set; }
        [JsonIgnore]

        public Dictionary<string, Channel> ChannelByISEID { get; private set; }
        public Dictionary<string, Datapoint> DatapointByType { get; private set; }

        public string Name { get; private set; }

        public string ISEID { get; private set; }

        public bool Reachable { get; internal set; }

        public bool PendingConfig { get; internal set; }

        public string Address { get; private set; }
                
        public string Interface { get; private set; }        

        public string DeviceType { get; private set; }

        public HashSet<string> Rooms { get { return new HashSet<string>(Channels.SelectMany(c => c.Rooms)); } }

        public HashSet<string> Functions { get { return new HashSet<string>(Channels.SelectMany(c => c.Functions)); } }

        ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DateTime LastCommunicationTest { get; private set; } = DateTime.MinValue;

        [JsonIgnore]
        public DeviceManager DeviceManager { get; private set; }
        
        public Device(XMLAPI.DeviceList.Device d, XMLAPI.Client CGIClient, DeviceManager dm)
        {
            DeviceManager = dm;

            FillFromDeviceListDevice(d);
            FillFromRoomList(CGIClient.RoomList);
            FillFromFunctionList(CGIClient.FunctionList);
            FillFromStateList(CGIClient.StateList);
            FillFromMasterValueList(CGIClient.MasterValueList);

            InitDatapointByType();
        }

        private void FillFromMasterValueList(XMLAPI.MastervalueList.mastervalue masterValueList)
        {
            //MasterValues = new Dictionary<string, MasterValue>();

            foreach (var c in masterValueList.channels.Where(w => w.mastervalue != null))
            {
                var dc = DeviceManager.Devices.SelectMany(d => d.Channels.Where(w => w.ISEID == c.ise_id)).FirstOrDefault();

                if(dc != null)
                {
                    dc.MasterValues.Clear();
                    foreach (var mv in c.mastervalue)
                    {
                        var dmv = new MasterValue(mv, dc);                        
                        dc.MasterValues.Add(dmv.Name, dmv);
                    }
                }
            }
        }

        private void InitDatapointByType()
        {
            //there can be multiple datapoints with the same type
            //if that is the case, only the datapoint with channel of direction RECEIVER is the one we should selected   
              
            var dict = new Dictionary<string, List<Datapoint>>();

            foreach (var c in Channels)
            {
                foreach(var dp in c.Datapoints)
                {
                    if (dict.ContainsKey(dp.Key))
                        dict[dp.Key].Add(dp.Value);
                    else
                    {
                        var list = new List<Datapoint>();
                        list.Add(dp.Value);
                        dict.Add(dp.Key, list);
                    }
                }
            }

            DatapointByType = new Dictionary<string, Datapoint>();

            foreach (var kvp in dict)
            {
                if (kvp.Value.Count == 1)
                    DatapointByType.Add(kvp.Key, kvp.Value[0]);
                else
                {
                    var first = kvp.Value.Where(w => w.Channel.Direction == "RECEIVER").FirstOrDefault();
                    if (first == null)
                    {
                        //nothing interesting. ignoring.
                    }
                    else
                    {
                        DatapointByType.Add(first.Type, first);
                    }
                }              
            }
        }

        private void FillFromDeviceListDevice(XMLAPI.DeviceList.Device d)
        {
            Name = d.Name;
            ISEID = d.Ise_id;
            Address = d.Address;
            Interface = d.Interface;
            DeviceType = d.Device_type;
          
            ChannelByISEID = new Dictionary<string, Channel>();

            Channels = new Channel[d.Channel.Count];
            for (int i = 0; i < Channels.Length; i++)
            {
                Channels[i] = new Channel(d.Channel[i], this);
                if(!ChannelByISEID.ContainsKey(Channels[i].ISEID))
                    ChannelByISEID.Add(Channels[i].ISEID, Channels[i]);
                else
                    throw new Exception(String.Format("Duplicated channel iseid {0} on device {1}.", Channels[i].ISEID, d.Name));                   
            }  
        }

        private void FillFromRoomList(XMLAPI.RoomList.RoomList roomList)
        {
            foreach(var room in roomList.Room)
            {                
                foreach(var channel in room.Channel)
                {
                    Channel c = null;
                    if (ChannelByISEID.TryGetValue(channel.Ise_id, out c))
                    {
                        if (!c.Rooms.Contains(room.Name))
                        {
                            string r = room.Name.Trim();

                            if (r.StartsWith("room"))
                                r = r.Substring(4);

                            c.Rooms.Add(r);
                        }
                    }                   
                }
            }
        }

        private void FillFromFunctionList(XMLAPI.FunctionList.FunctionList funcList)
        {
            foreach(var function in funcList.Function)
            {
                foreach (var channel in function.Channel)
                {
                    Channel c = null;
                    if (ChannelByISEID.TryGetValue(channel.Ise_id, out c))
                    {
                        if(!c.Functions.Contains(function.Name))
                        {
                            string f = function.Name;

                            if (f.StartsWith("func"))
                                f = f.Substring(4);

                            c.Functions.Add(f);
                        }
                    }
                }
            }
        }

        internal void FillFromStateList(XMLAPI.StateList.Device d)
        {
            PendingConfig = String.IsNullOrWhiteSpace(d.Config_pending) ? false : Convert.ToBoolean(d.Config_pending);
            Reachable = String.IsNullOrWhiteSpace(d.Unreach) ? true : !Convert.ToBoolean(d.Unreach);

            foreach (var c in d.Channel)
            {
                var dc = ChannelByISEID[c.Ise_id];
                dc.UpdateFromXMLAPI(c.Datapoint);
            }

            //Datapoint voltageDp;
            
            //if(DatapointByType != null && DatapointByType.TryGetValue("OPERATING_VOLTAGE", out voltageDp))
            //{
            //    if (Convert.ToDecimal(voltageDp.Value) == 0.0M)
            //        Reachable = false;
            //}

            Datapoint rssiDp;

            if(DatapointByType != null && DatapointByType.TryGetValue("RSSI_DEVICE", out rssiDp))
            {
                if (rssiDp.Value as string == "0")
                    Reachable = false;
            }
        }

        private void FillFromStateList(XMLAPI.StateList.StateList stateList)
        {
            foreach(var d in stateList.Device)
                if(d.Ise_id == ISEID)
                    FillFromStateList(d);    
        }

        public string StartCommunicationTest()
        {
            var tkn = DeviceManager.JsonAPIClient.Device_startCommunicationTest(this);
            LastCommunicationTest = DateTime.Now;

            return tkn.ToString();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }  
}
