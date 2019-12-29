using csharpmatic.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Generic
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

        private Datapoint UnderlyingDatapoint { get; set; }

        private Channel Channel { get { return UnderlyingDatapoint.GetChannel(); } }

        public TypedDatapoint(Datapoint dp)
        {
            UnderlyingDatapoint = dp;
        }

        public Channel GetChannel()
        {
            return Channel;
        }

        public Datapoint GetUnderlyingDatapoint()
        {
            return UnderlyingDatapoint;
        }

        public void SetRoomValue(object value, Type interfaceFilter = null)
        {
            UnderlyingDatapoint.SetRoomValue(value, interfaceFilter);
        }

        public void SetState(object value)
        {
            UnderlyingDatapoint.SetValue(value);
        }

        public async Task SetValueAsync(object value)
        {
            await UnderlyingDatapoint.SetValueAsync(value);
        }
    }
}
