using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Interfaces
{
    public interface IValveControlDevice : IHmDevice
    {
        TypedDatapoint<Decimal> Level { get; }

        TypedDatapoint<IValveControlDevice_Level_Status> Level_Status { get; }
        TypedDatapoint<Boolean> Valve_Adaption { get; }

        TypedDatapoint<IValveControl_Valve_State_Enum> Valve_State { get; }
    }

    public enum IValveControl_Valve_State_Enum
    {
        STATE_NOT_AVAILABLE,
        RUN_TO_START,
        WAIT_FOR_ADAPTION,
        ADAPTION_IN_PROGRESS,
        ADAPTION_DONE,
        TOO_TIGHT,
        ADJUSTMENT_TOO_BIG,
        ADJUSTMENT_TOO_SMALL,
        ERROR_POSITION,
    }

    public enum IValveControlDevice_Level_Status
    {
        NORMAL,
        UNKNOWN,
        OVERFLOW,
        UNDERFLOW,
        ERROR
    }
}
