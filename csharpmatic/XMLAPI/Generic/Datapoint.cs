using csharpmatic.XMLAPI.CGI;
using csharpmatic.XMLAPI.Interfaces;
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
        static string DateTimeFormatPattern { get { return "yyyy_MM_dd HH:mm"; } }

        public string Name { get; private set; }

        public string Type { get; private set; }

        public string ISEID { get; private set; }

        private object _InternalValue;

        public object Value { get { return GetValue(); } set { SetValue(value); } }

        private string _InternalValueType;

        public Type ValueType { get; private set; }
  
        public string ValueUnit { get; private set; }

        private long _InternalTimestamp { get; }
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
            ValueUnit = dp.Valueunit;
            _InternalTimestamp = String.IsNullOrWhiteSpace(dp.Timestamp) ? 0 : Convert.ToInt64(dp.Timestamp);
            Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            Timestamp = Timestamp.AddSeconds(_InternalTimestamp);

            _InternalValueType = dp.Valuetype;
            Init_ValueType();

            SetInternalValue(dp.Value);
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

            InterfacePropertyName = propname; 
            
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

        private void Init_ValueType()
        {
            //special handling of dimmer level, it's reported as a string.
            if (Type == "LEVEL" && Channel.Device.DeviceType == "HmIP-BDT")
                _InternalValueType = "4"; //decimal

            switch (_InternalValueType)
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
        
        public string GetValueString()
        {
            if (_InternalValue == null)
                return "";

            if (ValueType == typeof(DateTime))
                return ((DateTime)_InternalValue).ToString(DateTimeFormatPattern);
            else
                return Convert.ToString(_InternalValue);
        }

        public T GetValue<T>()
        {
            if (_InternalValue == null)
                return default(T);
            else
                return (T)GetValue();
        }

        private object GetValue()
        {
            //_InternalValue is always representing c# object of ValueType type
            return _InternalValue;
        }
        
        private void SetInternalValue(object value)
        {
            string valueStr = Convert.ToString(value);

            if (value == null || String.IsNullOrWhiteSpace(valueStr))
            {
                _InternalValue = default(ValueType);
            }
            else if (ValueType == typeof(DateTime))
            {
                if (value is DateTime)
                    _InternalValue = value;
                else
                    _InternalValue = DateTime.ParseExact(valueStr, DateTimeFormatPattern, CultureInfo.CurrentCulture.DateTimeFormat);
            }
            else
                _InternalValue = Convert.ChangeType(value, ValueType);
        }

        public void SetValue(object value)
        {
            SetInternalValue(value);
                          
            CGIClient cgi = new CGIClient(Channel.Device.DeviceManager.HttpServerUri);            
            cgi.SetISEIDValue(ISEID, GetValueString());
        }

        public void SetRoomValue(object value, Type deviceTypeFilter=null)
        {
            //get list all all devices in scope          

        }
    }
}
