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
        public void GetAlarm()
        {
            this.JsonResponse(AlarmAutomation);
        }

        public bool Arm()
        {
            var armOK = AlarmAutomation.Arm();

            this.JsonResponse(AlarmAutomation);

            return armOK;
        }

        public void Disarm()
        {
            AlarmAutomation.Disarm();

            this.JsonResponse(AlarmAutomation);
        }
    }
}
