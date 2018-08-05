using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Interfaces
{
    public interface ISingleSwitchControlDevice
    {
        ManagedDatapoint<Int32> Process { get; }

        ManagedDatapoint<Int32> Section { get; }

        ManagedDatapoint<Int32> Section_Status { get; }

        ManagedDatapoint<Boolean> State { get; }
    }
}
