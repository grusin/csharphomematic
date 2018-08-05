using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Interfaces
{
    public interface IValveControlDevice
    {
        ManagedDatapoint<Boolean> Valve_Adaption { get; }

        ManagedDatapoint<IValveControl_Valve_State_Enum> Valve_State { get; }
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
}
