using csharpmatic.XMLAPI.Generic;
using csharpmatic.XMLAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Interfaces
{
    public interface IDimmerDevice : IHmDevice
    {
        ManagedDatapoint<Decimal> Level { get;  }

        ManagedDatapoint<IDimmerDevice_Level_Status> Level_Status { get; }

        ManagedDatapoint<Decimal> Ramp_Time { get; }
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
