using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Interfaces
{
    public interface IHmIPDevice
    {
        ManagedDatapoint<Boolean> Config_Pending { get; }

        ManagedDatapoint<Boolean> Duty_Cycle { get; }

        ManagedDatapoint<Decimal> Operating_Voltage { get; }

        ManagedDatapoint<IHmIP_Operating_Voltage_Status_Enum> Operating_Voltage_Status { get; }

        ManagedDatapoint<String> Rssi_Device { get; }

        ManagedDatapoint<String> Rssi_Peer { get; }

        ManagedDatapoint<Boolean> Unreach { get; }

        ManagedDatapoint<Boolean> Update_Pending { get; }
    }

    public enum IHmIP_Operating_Voltage_Status_Enum
    {
        NORMAL,
        UNKNOWN,
        OVERFLOW,
    }
}
