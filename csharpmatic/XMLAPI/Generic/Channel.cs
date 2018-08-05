using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Generic
{
    public class Channel
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        public string Address { get; private set; }

        public string ISEID { get; private set; }

        public string Direction { get; private set; }

        public string ParentDeviceISEID{ get; private set; }

        public int ChannelIndex { get; private set; }

        public string GroupParnter { get; private set; }

        public bool AESAvailable { get; private set; }

        public string TransmissionMode { get; private set; }

        public bool Visible { get; private set; }

        public bool ReadyConfig { get; private set; }

        public bool Operate { get; private set; }

        public Dictionary<string, Datapoint> Datapoints { get; private set; }

        public HashSet<string> Rooms { get; private set; }

        public HashSet<string> Functions { get; private set; }

        [JsonIgnore]
        public Device Device { get; private set; }

        public Channel(CGI.DeviceList.Channel dlc, Device d)
        {
            Device = d;
            Rooms = new HashSet<string>();
            Functions = new HashSet<string>();
            Datapoints = new Dictionary<string, Datapoint>();
            FillFromDeviceListChannel(dlc);          
        }
        
        public void UpdateDataPoints(IEnumerable<CGI.StateList.Datapoint> list)
        {
            Datapoints = new Dictionary<string, Datapoint>();

            foreach (var dp in list)
            {
                var gdp = new Datapoint(dp, this);

                Datapoints.Add(gdp.Type, gdp);
            }
        }

        private void FillFromDeviceListChannel(CGI.DeviceList.Channel c)
        {
            Name = c.Name;
            Type = c.Type;
            Address = c.Address;
            ISEID = c.Ise_id;
            Direction = c.Direction;
            ParentDeviceISEID = c.Parent_device;
            ChannelIndex = Convert.ToInt32(c.Index);
            GroupParnter = null; //todo
            AESAvailable = String.IsNullOrWhiteSpace(c.Aes_available) ? false : Convert.ToBoolean(c.Aes_available);
            TransmissionMode = c.Transmission_mode;
            Visible = String.IsNullOrWhiteSpace(c.Visible) ? false : Convert.ToBoolean(c.Visible);
            Operate = String.IsNullOrWhiteSpace(c.Operate) ? false : Convert.ToBoolean(c.Operate);                
        }               

        public override string ToString()            
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
