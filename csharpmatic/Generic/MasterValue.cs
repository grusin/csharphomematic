using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Generic
{
    public class MasterValue
    {
        public string Name { get; private set; }
        public decimal Value { get; private set; }
        public Channel Channel { get; private set; }

        public MasterValue(XMLAPI.MastervalueList.mastervalue mv, Channel c)
        {
            Name = mv.name;
            Value = mv.value;
            Channel = c;
        }
    }
}
