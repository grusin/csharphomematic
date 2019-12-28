using csharpmatic.Automation.Alarm;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Automation.RestApi
{
    public class AlarmControler : WebApiController
    {
        public static AlarmAutomation AlarmAutomation { get; set; }

        public AlarmControler()
        {
        }

        [Route(HttpVerbs.Get, "/alarm")]
        public async Task<AlarmAutomation> GetAlarm()
        {
            return AlarmAutomation;
        }

        [Route(HttpVerbs.Get, "/alarm/arm")]
        public async Task<AlarmAutomation> Arm()
        {
            var armOK = AlarmAutomation.Arm();

            return AlarmAutomation;
        }

        [Route(HttpVerbs.Get, "/alarm/disarm/{code}")]
        public async Task<AlarmAutomation> Disarm(string code)
        {
            AlarmAutomation.Disarm();

            return AlarmAutomation;
        }

        // You can override the default headers and add custom headers to each API Response.
        public void SetDefaultHeaders()
        {
            this.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
