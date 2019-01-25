using csharpmatic.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Interfaces
{
    public interface ILowBatteryInfo : IHmDevice
    {
        TypedDatapoint<Boolean> Low_Bat { get; }
    }
}
