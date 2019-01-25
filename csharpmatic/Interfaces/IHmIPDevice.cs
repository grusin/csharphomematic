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
        
        TypedDatapoint<int> Rssi_Device { get; }

        TypedDatapoint<int> Rssi_Peer { get; }
        
        TypedDatapoint<Boolean> Unreach { get; }

        TypedDatapoint<Boolean> Update_Pending { get; }
    }
}
