using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Automation.UsageLimits
{
    public class UsageLimit
    {
        public TimeSpan TimeSpan { get; private set; }
        public int MaximumTrue { get; private set; }

        public UsageLimit(TimeSpan ts, int limit)
        {
            TimeSpan = ts;
            MaximumTrue = limit;
        }    
    }
}
