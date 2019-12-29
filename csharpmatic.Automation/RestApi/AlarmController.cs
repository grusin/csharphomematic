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

        [Route(HttpVerbs.Get, "/")]
        public async Task<AlarmAutomation> GetAlarm()
        {
            return AlarmAutomation;
        }

        [Route(HttpVerbs.Get, "/arm")]
        public async Task<AlarmAutomation> Arm()
        {
            var armOK = AlarmAutomation.Arm();

            return AlarmAutomation;
        }

        [Route(HttpVerbs.Get, "/disarm/{code}")]
        public async Task<AlarmAutomation> Disarm(string code)
        {
            AlarmAutomation.Disarm();

            return AlarmAutomation;
        }
    }
}
