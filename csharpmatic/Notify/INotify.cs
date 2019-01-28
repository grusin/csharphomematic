using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Notify
{
    public interface INotify : IDisposable
    {
        string Name { get; }
        Task SendTextMessageAsync(string message);
    }
}
