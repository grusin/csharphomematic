using System;
using csharpmatic.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Interfaces
{
    public interface IOperatingVoltageInfo : IHmDevice
    {
        TypedDatapoint<Decimal> Operating_Voltage { get; }

        TypedDatapoint<IOperating_Voltage_Status_Enum> Operating_Voltage_Status { get; }
    }

    public enum IOperating_Voltage_Status_Enum
    {
        NORMAL,
        UNKNOWN,
        OVERFLOW,
    }
}
