using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Generic
{
    public class Datapoint
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        public string ISEID { get; private set; }

        public string InternalValue { get; private set; }

        public object Value { get; private set; }

        public string InternalValueType { get; private set; }

        public Type ValueType { get; private set; }
  
        public string ValueUnit { get; private set; }

        public long InternalTimestamp { get; private set; }
        public DateTime Timestamp { get; private set; }              

        public int OperationsCounter { get; private set; }

        private string InterfacePropertyName;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        
        [JsonIgnore]
        public Channel Channel { get; private set; }
                
        public Datapoint(CGI.StateList.Datapoint dp, Channel c)
        {
            Channel = c;
            Name = c.Name + "." + dp.Type;
            ISEID = dp.Ise_id;
            Type = MapDatapointType(dp, c);
            InternalValue = dp.Value;
            InternalValueType = dp.Valuetype;
            ValueUnit = dp.Valueunit;
            InternalTimestamp = String.IsNullOrWhiteSpace(dp.Timestamp) ? 0 : Convert.ToInt64(dp.Timestamp);
            Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            Timestamp = Timestamp.AddSeconds(InternalTimestamp);

            Init_ValueType();
            Init_Value();
        }

        public string GetInterfacePropertyName(bool useCached=true)
        {
            if (useCached == false)
                InterfacePropertyName = null;
            else if (InterfacePropertyName != null)
                return InterfacePropertyName;

            string propname = Type.Replace("_", " ").ToLower();
            propname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(propname);
            propname = propname.Replace(" ", "_");

            //just take the first one, seems that all devices use this rule ;-)

            //int cnt = Channel.Device.Channels.SelectMany(c => c.Datapoints.Where(w => w.Value.Type == Type)).Count();
            
            //if(cnt == 1)
            InterfacePropertyName = propname;
            //else
            //    InterfacePropertyName = String.Format("{0}_C{1}", propname, Channel.ChannelIndex);            
            
            return InterfacePropertyName;
        }

        private string MapDatapointType(CGI.StateList.Datapoint dp, Channel c)
        {
            //rename switching devices internal temperature, so that it does not conflic with heating systems temperatures naming. Channel index logic should suffice.
            if(dp.Type == "ACTUAL_TEMPERATURE" && c.ChannelIndex == 0)
                return "ACTUATOR_ACTUAL_TEMPERATURE";

            if (dp.Type == "ACTUAL_TEMPERATURE_STATUS" && c.ChannelIndex == 0)
                return "ACTUATOR_ACTUAL_TEMPERATURE_STATUS";

            //HmIP-HEATING virtual device heating conflicting names
            if (c.Device.DeviceType == "HmIP-HEATING" && dp.Type == "STATE")
            {
                if (c.ChannelIndex == 3)
                    return "HANDLE_STATE";
                if (c.ChannelIndex == 4)
                    return "RELAY_STATE";
            }
   
            return dp.Type;
        }

        private void Init_Value()
        {
            if (String.IsNullOrWhiteSpace(InternalValue))
                Value = null;

            try
            {
                if (ValueType == typeof(DateTime))
                {
                    //2000_01_01 00:00
                    Value = DateTime.ParseExact(InternalValue, "YYYY_MM_DD HH:mm", CultureInfo.CurrentCulture.DateTimeFormat);
                }
                else
                    Value = Convert.ChangeType(InternalValue, ValueType);
            }
            catch
            {

            }

            if(Value == null)
                Value = default(ValueType);
        }

        private void Init_ValueType()
        {
            switch (InternalValueType)
            {
                //boolean
                case "2":
                    ValueType = typeof(Boolean);
                    break;
                case "16":
                    ValueType = typeof(int);
                    break;
                case "4":
                    ValueType = typeof(decimal);
                    break;
                case "20":
                    ValueType = typeof(DateTime);
                    break;
                default:
                    ValueType = typeof(string);
                    break;
            }
        }         
    }
}
