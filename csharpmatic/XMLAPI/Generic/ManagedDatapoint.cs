using csharpmatic.Properties;
using csharpmatic.XMLAPI.CGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Generic
{
    public class ManagedDatapoint<T>
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        public string ISEID { get; private set; }

        private T _value;
        public T Value { get { return _value; } set { SetValue(value); } }
        
        public string ValueUnit { get; private set; }

        public DateTime Timestamp { get; private set; }

        public int OperationsCounter { get; private set; }

        public ManagedDatapoint(Datapoint dp)
        {
            Name = dp.Name;
            Type = dp.Type;
            ISEID = dp.ISEID;

            if (dp.Value == null)
                _value = default(T);
            else
                _value = (T)dp.Value;

            ValueUnit = dp.ValueUnit;
            Timestamp = dp.Timestamp;
            OperationsCounter = dp.OperationsCounter;
        }

        public void SetValue(T newValue)
        {
            _value = newValue;
            
            CGIClient cgi = new CGIClient("http://" + Settings.Default.ServerAddress);

            //FIXME: handle dates nicely
            cgi.SetISEIDValue(ISEID, _value.ToString());
        }
    }
}
