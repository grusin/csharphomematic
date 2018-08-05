﻿using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Interfaces
{
    interface IHmIP
    {
        TypedDatapoint<Boolean> Config_Pending { get;  }

        TypedDatapoint<Boolean> Duty_Cycle { get; }

        TypedDatapoint<Boolean> Low_Bat { get; }

        TypedDatapoint<Decimal> Operating_Voltage { get; }

        TypedDatapoint<Int32> Operating_Voltage_Status { get; }

        TypedDatapoint<String> Rssi_Device { get; }

        TypedDatapoint<String> Rssi_Peer { get;  }

        TypedDatapoint<Boolean> Unreach { get; }

        TypedDatapoint<Boolean> Update_Pending { get; }
    }
}
