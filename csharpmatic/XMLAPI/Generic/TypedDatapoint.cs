using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Generic
{
    public class TypedDatapoint<T>
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        public string ISEID { get; private set; }

        public T Value { get; private set; }
        
        public string ValueUnit { get; private set; }

        public DateTime Timestamp { get; private set; }

        public int OperationsCounter { get; private set; }

        public TypedDatapoint(Datapoint dp)
        {
            Name = dp.Name;
            Type = dp.Type;
            ISEID = dp.ISEID;

            if (dp.Value == null)
                Value = default(T);
            else
                Value = (T)dp.Value;

            ValueUnit = dp.ValueUnit;
            Timestamp = dp.Timestamp;
            OperationsCounter = dp.OperationsCounter;
        }
    }
}
