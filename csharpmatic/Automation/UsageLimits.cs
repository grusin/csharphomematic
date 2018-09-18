using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Automation
{
    public class UsageLimits
    {
        public TimeSpan TimeSpan { get; private set; }
        public int MaximumTrue { get; private set; }

        public UsageLimits(TimeSpan ts, int limit)
        {
            TimeSpan = ts;
            MaximumTrue = limit;
        }    
    }
}
