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

        public string Value { get; private set; }

        public string ValueType { get; private set; }
  
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

        public string ToCSharpPropertyTemplate()
        {
            string propname = Type.Replace("_", " ").ToLower();
            propname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(propname);
            propname = propname.Replace(" ", "");

            string csharpdatatype = null;

            switch(ValueType)
            {
                //boolean
                case "2":
                    csharpdatatype = "bool";
                    break;
                case "16":
                    csharpdatatype = "int";
                    break;
                case "4":
                    csharpdatatype = "decimal";
                    break;
                case "20":
                    csharpdatatype = "DateTime";
                    break;
                default:
                    csharpdatatype = "string";
                    break;
            }

            string code =  String.Format(@"public {0} {1} {{ get {{ return ({0}) Convert.ChangeType(GetDatapointByType(""{2}""), typeof({0})); }} }} ", csharpdatatype, propname, Type);

            return code;
        }


        public Datapoint(CGI.StateList.Datapoint dp, Channel c)
        {
            Channel = c;
            Name = dp.Name;
            ISEID = dp.Ise_id;
            Type = dp.Type;
            Value = dp.Value;
            ValueType = dp.Valuetype;
            ValueUnit = dp.Valueunit;
            InternalTimestamp = String.IsNullOrWhiteSpace(dp.Timestamp) ? 0 : Convert.ToInt64(dp.Timestamp);
            Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            Timestamp = Timestamp.AddSeconds(InternalTimestamp);
        }


    }
}
