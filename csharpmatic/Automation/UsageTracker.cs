using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Automation
{
    public class UsageTracker
    {
        private Dictionary<DateTime, bool> Usage;

        public UsageTracker()
        {
            Usage = new Dictionary<DateTime, bool>();
        }

        public bool Get(DateTime dt)
        {
            if (Usage.ContainsKey(dt))
                return Usage[dt];
            else
                return false;
        }

        public int GetEventCount(TimeSpan ts, bool value)
        {
            DateTime start = GetTimeStampWithMinutesPrecision() - ts;

            return Usage.Where(u => u.Key <= start && u.Value == value).Count();
        }

        public void SetUsageTrueWins(bool state)
        {
            DateTime dt = GetTimeStampWithMinutesPrecision();

            if (Usage.ContainsKey(dt))
            {
                if (state)
                    Usage[dt] = true;   
            }
            else
                Usage.Add(dt, state);
        }
        
        public static DateTime GetTimeStampWithMinutesPrecision()
        {
            DateTime a = DateTime.Now;
            DateTime b = new DateTime(a.Year, a.Month, a.Day, a.Hour, a.Minute, 0);

            return b;
        }
    }
}
