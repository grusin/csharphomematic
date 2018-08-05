using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Interfaces
{
    public interface ITempControlDevice
    {
        ManagedDatapoint<Int32> Active_Profile { get; }

        ManagedDatapoint<Decimal> Actual_Temperature { get; }

        ManagedDatapoint<ITempControl_Actual_Temperature_Status> Actual_Temperature_Status { get; }

        ManagedDatapoint<Boolean> Boost_Mode { get; }

        ManagedDatapoint<Int32> Boost_Time { get; }

        ManagedDatapoint<Int32> Set_Point_Mode { get; }

        ManagedDatapoint<Decimal> Set_Point_Temperature { get; }

        ManagedDatapoint<Boolean> Switch_Point_Occured { get; }

        ManagedDatapoint<ITempControl_Windows_State_Enum> Window_State { get; }

        ManagedDatapoint<Boolean> Frost_Protection { get; }
    }

    public enum ITempControl_Windows_State_Enum
    {
        CLOSED,
        OPEN
    }

    public enum ITempControl_Actual_Temperature_Status
    {
        NORMAL,
        UNKNOWN,
        OVERFLOW,
        UNDERFLOW,
    }
}
