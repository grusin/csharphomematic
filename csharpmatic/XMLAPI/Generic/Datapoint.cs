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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        
        [JsonIgnore]
        public Channel Channel { get; private set; }
                
        public Datapoint(CGI.StateList.Datapoint dp, Channel c)
        {
            Channel = c;
            Name = dp.Name;
            ISEID = dp.Ise_id;
            Type = dp.Type;
            InternalValue = dp.Value;
            InternalValueType = dp.Valuetype;
            ValueUnit = dp.Valueunit;
            InternalTimestamp = String.IsNullOrWhiteSpace(dp.Timestamp) ? 0 : Convert.ToInt64(dp.Timestamp);
            Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            Timestamp = Timestamp.AddSeconds(InternalTimestamp);

            Init_ValueType();
            Init_Value();
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
