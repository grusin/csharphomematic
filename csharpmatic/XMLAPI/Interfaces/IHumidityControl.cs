using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Interfaces
{
    public interface IHumidityControl
    {
        TypedDatapoint<Int32> Humidity { get; }

        TypedDatapoint<IHumidityControl_Humidity_Status_Enum> Humidity_Status { get; }
    }

    public enum IHumidityControl_Humidity_Status_Enum
    {
        NORMAL,
        UNKNOWN,
        OVERFLOW,
        UNDERFLOW
    }
}
