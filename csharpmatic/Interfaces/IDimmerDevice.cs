using csharpmatic.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Interfaces
{
    public interface IDimmerDevice : IHmDevice
    {
        TypedDatapoint<Decimal> Level { get;  }

        TypedDatapoint<IDimmerDevice_Level_Status> Level_Status { get; }

        TypedDatapoint<Decimal> Ramp_Time { get; }
    }

    public enum IDimmerDevice_Level_Status
    {
        NORMAL,
        UNKNOWN,
        OVERFLOW,
        UNDERFLOW,
        ERROR,
    }
}
