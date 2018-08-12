using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Interfaces
{
    public interface ISingleSwitchControlDevice : IHmDevice
    {
        TypedDatapoint<ISingleSwitchControlDevice_Process> Process { get; }

        TypedDatapoint<Int32> Section { get; }

        TypedDatapoint<ISingleSwitchControlDevice_Section_Status> Section_Status { get; }

        TypedDatapoint<Boolean> State { get; }

        TypedDatapoint<ISingleSwitchControlDevice_ActivityState> Activity_State { get; }
    }

    public enum ISingleSwitchControlDevice_Section_Status
    {
        NORMAL,
        UNKNOWN
    }

    public enum ISingleSwitchControlDevice_Process
    {
        STABLE,
        NOT_STABLE
    }

    public enum ISingleSwitchControlDevice_ActivityState
    {
        UNKNOWN,
        UP,
        DOWN,
        STABLE,
    }
}
