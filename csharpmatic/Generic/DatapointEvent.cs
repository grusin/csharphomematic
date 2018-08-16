using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Generic
{
    public class DatapointEvent
    {
        public DateTime EventTimestamp { get { return Current.Timestamp; } }
        public Datapoint Current { get; private set;  }
        public Datapoint Previous { get; private set; }

        public DatapointEvent(Datapoint c, Datapoint p)
        {
            Current = c;
            Previous = p;
        }
    }
}
