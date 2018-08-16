using csharpmatic.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Interfaces
{
    public interface IHmIPDevice : IHmDevice
    {
        TypedDatapoint<Boolean> Config_Pending { get; }

        TypedDatapoint<Boolean> Duty_Cycle { get; }

        TypedDatapoint<Decimal> Operating_Voltage { get; }

        TypedDatapoint<IHmIP_Operating_Voltage_Status_Enum> Operating_Voltage_Status { get; }

        TypedDatapoint<String> Rssi_Device { get; }

        TypedDatapoint<String> Rssi_Peer { get; }
        
        TypedDatapoint<Boolean> Unreach { get; }

        TypedDatapoint<Boolean> Update_Pending { get; }
    }

    public enum IHmIP_Operating_Voltage_Status_Enum
    {
        NORMAL,
        UNKNOWN,
        OVERFLOW,
    }
}
