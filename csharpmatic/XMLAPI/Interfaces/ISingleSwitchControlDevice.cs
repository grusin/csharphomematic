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
        ManagedDatapoint<ISingleSwitchControlDevice_Process> Process { get; }

        ManagedDatapoint<Int32> Section { get; }

        ManagedDatapoint<ISingleSwitchControlDevice_Section_Status> Section_Status { get; }

        ManagedDatapoint<Boolean> State { get; }

        ManagedDatapoint<ISingleSwitchControlDevice_ActivityState> Activity_State { get; }
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
