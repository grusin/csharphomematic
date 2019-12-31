using csharpmatic.Interfaces;
using csharpmatic.XMLAPI.MastervalueList;
using log4net;
using Swan.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Generic
{
    public class Device : IHmDevice
    {
        [JsonProperty("Channels", true)]
        public Channel[] Channels { get; private set; }        

        public string Name { get; private set; }

        public string ShortName => GetShortName();

        public string ISEID { get; private set; }

        public bool Reachable { get; internal set; }

        public bool PendingConfig { get; internal set; }

        public string Address { get; private set; }
                
        public string Interface { get; private set; }        

        public string DeviceType { get; private set; }

        public HashSet<string> Rooms { get { return new HashSet<string>(Channels.SelectMany(c => c.Rooms)); } }

        public HashSet<string> Functions { get { return new HashSet<string>(Channels.SelectMany(c => c.Functions)); } }

        private ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DateTime LastCommunicationTest { get; private set; } = DateTime.MinValue;
                
        internal DeviceManager DeviceManager { get; private set; }

        internal Dictionary<string, Channel> ChannelByISEID { get; set; }
        internal Dictionary<string, Datapoint> DatapointByName { get; set; }
        internal Dictionary<string, Datapoint> DatapointByISEID { get; set; }
        internal Dictionary<string, Mastervalue> MastervalueByName { get; set; }

        public Device(XMLAPI.DeviceList.Device d, XMLAPI.Client CGIClient, DeviceManager dm)
        {
            DeviceManager = dm;

            FillFromDeviceListDevice(d);
            FillFromRoomList(CGIClient.RoomList);
            FillFromFunctionList(CGIClient.FunctionList);
            FillFromStateList(CGIClient.StateList);
            FillFromMasterValueList(CGIClient.MasterValueListByChannel);

            InitDatapointLookups();
        }
            

        public List<Datapoint> GetDatapoints()
        {
            return DatapointByName.Values.ToList();
        }

        public List<string> GetDatapointNames()
        {
            return DatapointByName.Keys.ToList();
        }

        public List<string> GetDatapointIds()
        {
            return DatapointByISEID.Keys.ToList();
        }

        public Datapoint GetDatapointById(string iseid)
        {
            Datapoint dp = null;
            if (DatapointByISEID.TryGetValue(iseid, out dp))
                return dp;
            else
                return null;
        }

        public Datapoint GetDatapointByName(string name)
        {
            Datapoint dp = null;
            if (DatapointByName.TryGetValue(name, out dp))
                return dp;
            else
                return null;
        }

        public Mastervalue GetMastervalueByName(string name)
        {
            Mastervalue mv = null;
            if (MastervalueByName.TryGetValue(name, out mv))
                return mv;
            else
                return null;
        }

        private void FillFromMasterValueList(Dictionary<string, mastervalue[]> masterValueListByChannel)
        {
            if (MastervalueByName == null)
                MastervalueByName = new Dictionary<string, Mastervalue>();

            foreach (var c in Channels)
            {
                c.MasterValues.Clear();

                mastervalue[] raw_mvl = null;                               
                
                if (masterValueListByChannel.TryGetValue(c.ISEID, out raw_mvl))
                {
                    foreach (var raw_mv in raw_mvl)
                    {
                        //update channel master values
                        Mastervalue mv = null;
                        if (c.MasterValues.TryGetValue(raw_mv.name, out mv))
                            mv.Value = raw_mv.value;
                        else
                        {
                            mv = new Mastervalue(raw_mv, c);
                            c.MasterValues.Add(raw_mv.name, mv);
                        }

                        //update global dict                       
                        if (!MastervalueByName.ContainsKey(mv.Name))
                            MastervalueByName.Add(mv.Name, mv);
                    }
                }                
            }
        }

        private void InitDatapointLookups()
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

            DatapointByName = new Dictionary<string, Datapoint>();

            foreach (var kvp in dict)
            {
                if (kvp.Value.Count == 1)
                    DatapointByName.Add(kvp.Key, kvp.Value[0]);
                else
                {
                    var first = kvp.Value.Where(w => w.GetChannel().Direction == "RECEIVER").FirstOrDefault();
                    if (first == null)
                    {
                        //nothing interesting. ignoring.
                    }
                    else
                    {
                        DatapointByName.Add(first.Type, first);
                    }
                }              
            }

            //build dictionary by id...
            DatapointByISEID = new Dictionary<string, Datapoint>();
            foreach (var kvp in DatapointByName)            
                DatapointByISEID.Add(kvp.Value.ISEID, kvp.Value);
        }

        internal void UpdateFromDeviceXML()
        {

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
                string rn = room.Name.Trim();

                if (rn.StartsWith("room"))
                    rn = rn.Substring(4);

                foreach (var channel in room.Channel)
                {
                    Channel c = null;
                    if (ChannelByISEID.TryGetValue(channel.Ise_id, out c))
                    {
                        if (!c.Rooms.Contains(rn))
                            c.Rooms.Add(rn);
                    }                   
                }
            }
        }

        private void FillFromFunctionList(XMLAPI.FunctionList.FunctionList funcList)
        {
            foreach(var function in funcList.Function)
            {
                //make sure function names have "func" stripped from it (legacy homematic setup leaves them like this)
                //also make sure that the name is capitalized.

                string fn = function.Name.ToLower();
                if (fn.StartsWith("func"))
                    fn = fn.Substring(4);

                char[] arr = fn.ToCharArray();
                arr[0] = Char.ToUpper(arr[0]);

                fn = new string(arr);                

                foreach (var channel in function.Channel)
                {
                    Channel c = null;
                    if (ChannelByISEID.TryGetValue(channel.Ise_id, out c))
                    {
                        if(!c.Functions.Contains(fn))                                                                                   
                            c.Functions.Add(fn);                        
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

            Datapoint rssiDp;

            if(DatapointByName != null && DatapointByName.TryGetValue("RSSI_DEVICE", out rssiDp))
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

        /*
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
        */

        private string GetShortName()
        {
            try
            {
                int idx = Name.IndexOf('-');
                if (idx != 0)
                    return Name.Substring(idx+1).Trim();
                else
                    return Name;
            }
            catch
            {
                return Name;
            }
        }
    }  
}
