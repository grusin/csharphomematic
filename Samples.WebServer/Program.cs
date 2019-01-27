using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace Samples.WebServer
{
    class Program
    {
        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            var server = new Unosquare.Labs.EmbedIO.WebServer(4000);
            server.RegisterModule(new StaticFilesModule(@"C:\Users\G\npm\homeui\build"));
            // The static files module will cache small files in ram until it detects they have been modified.
            server.Module<StaticFilesModule>().UseRamCache = true;
            server.Module<StaticFilesModule>().DefaultExtension = ".html";
            server.RunAsync();
            


            for(;;)
            {
                new ManualResetEvent(false).WaitOne(1000);
            }
        }
    }
}
