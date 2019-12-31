using csharpmatic.Generic;
using csharpmatic.Interfaces;
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
    public class DeviceController : WebApiController
    {
        public static DeviceManager DeviceManager { get; set; }

        public DeviceController()
        {
                
        }

        [Route(HttpVerbs.Get, "/{deviceiseid}")]
        public Device GetDevice(string deviceiseid)
        {
            Device d;

            if (DeviceManager.DevicesByISEID.TryGetValue(deviceiseid, out d))
                return d;
            else
                return null;
        }        

        [Route(HttpVerbs.Get, "/{deviceiseid}/setdatapointbyname/{datapointname}/{value}")]
        public Device SetDatapointByName(string deviceiseid, string datapointname, string value)
        {            
            lock (DeviceManager.RefreshLock)
            {
                var d = GetDevice(deviceiseid);

                if (d == null)
                    return null;

                var dp = d.GetDatapointByName(datapointname);

                if (dp == null)
                    return null;

                dp.SetValue(value);

                return d;
            }
        }

        [Route(HttpVerbs.Get, "/{deviceiseid}/setdatapointbyid/{datapointiseid}/{value}")]
        public Device SetDatapointById(string deviceiseid, string datapointiseid, string value)
        {
            lock (DeviceManager.RefreshLock)
            {
                var d = GetDevice(deviceiseid);

                if (d == null)
                    return null;

                var dp = d.GetDatapointById(datapointiseid);

                if (dp == null)
                    return null;

                dp.SetValue(value);

                return d;
            }
        }

        [Route(HttpVerbs.Get, "/{deviceiseid}/setmastervaluebyname/{mastervaluename}/{value}")]
        public Device SetMastervalueByName(string deviceiseid, string mastervaluename, string value)
        {
            lock (DeviceManager.RefreshLock)
            {
                var d = GetDevice(deviceiseid);

                if (d == null)
                    return null;

                var mv = d.GetMastervalueByName(mastervaluename);

                if (mv == null)
                    return null;

                mv.SetValue(Convert.ToDecimal(value));

                return d;
            }
        }
    }
}
