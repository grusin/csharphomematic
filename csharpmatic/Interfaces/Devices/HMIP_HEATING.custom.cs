using csharpmatic.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Interfaces.Devices
{
    public partial class HMIP_HEATING
    {
        public Mastervalue Valve_Offset
        {
            get
            {
                Mastervalue mv = GetMastervalueByName("VALVE_OFFSET");
                if (mv != null)
                    return mv;
                else
                    return new Mastervalue("VALVE_OFFSET", 0, null);
            }
        }
    }
}
