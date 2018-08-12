using csharpmatic.Properties;
using csharpmatic.XMLAPI.CGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Generic
{
    public class TypedDatapoint<T>
    {
        public string Name { get { return UnderlyingDatapoint.Name; } }

        public string Type { get { return UnderlyingDatapoint.Type; } }

        public string ISEID { get { return UnderlyingDatapoint.ISEID; } }
                
        public T Value { get { return UnderlyingDatapoint.GetValue<T>(); } set { UnderlyingDatapoint.SetValue(value); } }
        
        public string ValueUnit { get { return UnderlyingDatapoint.ValueUnit; } }

        public DateTime Timestamp { get { return UnderlyingDatapoint.Timestamp; } }

        public int OperationsCounter { get { return UnderlyingDatapoint.OperationsCounter; } }

        public Datapoint UnderlyingDatapoint { get; private set; }

        public TypedDatapoint(Datapoint dp)
        {
            UnderlyingDatapoint = dp;
        }

        public void SetRoomValue(object value, Type interfaceFilter = null)
        {
            UnderlyingDatapoint.SetRoomValue(value, interfaceFilter);
        }
    }
}
