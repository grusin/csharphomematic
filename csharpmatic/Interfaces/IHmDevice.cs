using csharpmatic.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Interfaces
{
    public interface IHmDevice
    {
        Channel[] Channels { get; }

        string Name { get; }

        string ISEID { get; }

        bool Reachable { get; }

        bool PendingConfig { get; }

        string Address { get; }

        string Interface { get;}

        string DeviceType { get;  }

        HashSet<string> Rooms { get; }
        HashSet<string> Functions { get; }
    }
}
