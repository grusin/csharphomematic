using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Generic
{
    public class Mastervalue
    {
        public string Name { get; private set; }
        public decimal Value { get; internal set; }
        private Channel Channel { get; set; }


        public Mastervalue(XMLAPI.MastervalueList.mastervalue mv, Channel c)
        {
            Name = mv.name;
            Value = mv.value;
            Channel = c;
        }

        public Mastervalue(string name, decimal value)
        {
            Name = name;
            Value = value;
        }

        public Mastervalue(string name, decimal value, Channel channel)
        {
            Name = name;
            Value = value;
            Channel = channel;
        }

        public void SetValue(decimal newValue, bool throwOnError=true)
        {
            if (Channel == null)
            {
                if (throwOnError)
                    throw new ArgumentNullException("Channel");
                else
                    return;
            }

            List<Mastervalue> list = new List<Mastervalue>();
            list.Add(new Mastervalue(Name, newValue, Channel));
            Channel.SetMasterValues(list);
        }
        public Channel GetChannel()
        {
            return Channel;
        }
    }
}
