using csharpmatic.Automation.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace csharpmatic.Automation.RestApi
{
    public class AlarmControler : WebApiController
    {
        public static AlarmAutomation AlarmAutomation { get; set; }

        public AlarmControler(IHttpContext context) : base(context)
        {
        }

        [WebApiHandler(HttpVerbs.Get, "/api/alarm")]
        public bool GetAlarm()
        {
            this.JsonResponse(AlarmAutomation);
            return true;
        }

        [WebApiHandler(HttpVerbs.Get, "/api/alarm/arm")]
        public bool Arm()
        {
            var armOK = AlarmAutomation.Arm();

            this.JsonResponse(AlarmAutomation);

            return armOK;
        }

        [WebApiHandler(HttpVerbs.Get, "/api/alarm/disarm/{code}")]
        public bool Disarm(string code)
        {
            AlarmAutomation.Disarm();

            this.JsonResponse(AlarmAutomation);
            return true;
        }

        // You can override the default headers and add custom headers to each API Response.
        public override void SetDefaultHeaders()
        {
            this.NoCache();
            this.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
