using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Generic
{
    public interface IAutomation: IDisposable
    {
        string Name { get;  }
        void Work();
    }
}
