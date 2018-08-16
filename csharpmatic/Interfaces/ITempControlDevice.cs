using csharpmatic.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Interfaces
{
    public interface ITempControlDevice : IHmDevice
    {
        TypedDatapoint<Int32> Active_Profile { get; }

        TypedDatapoint<Decimal> Actual_Temperature { get; }

        TypedDatapoint<ITempControl_Actual_Temperature_Status> Actual_Temperature_Status { get; }

        TypedDatapoint<Boolean> Boost_Mode { get; }

        TypedDatapoint<Int32> Boost_Time { get; }

        TypedDatapoint<Int32> Set_Point_Mode { get; }

        TypedDatapoint<Decimal> Set_Point_Temperature { get; }

        TypedDatapoint<Boolean> Switch_Point_Occured { get; }

        TypedDatapoint<ITempControl_Windows_State_Enum> Window_State { get; }

        TypedDatapoint<Boolean> Frost_Protection { get; }
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
