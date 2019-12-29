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

        [Route(HttpVerbs.Get | HttpVerbs.Post, "/{deviceiseid}")]
        public async Task<Device> GetDevice(string device_iseid)
        {
            Device d;

            if (DeviceManager.DevicesByISEID.TryGetValue(device_iseid, out d))
                return d;
            else
                return null;
        }        

        [Route(HttpVerbs.Get | HttpVerbs.Post, "/{deviceiseid}/setdatapointbyname/{datapointname}/{value}")]
        public async Task<Device> SetDatapointByName(string deviceiseid, string datapointname, string value)
        {
            var d = await GetDevice(deviceiseid);

            if (d == null)
                return null;

            var dp = d.GetDatapointByName(datapointname);

            if(dp == null)
                return null;

            await dp.SetValueAsync(value);

            return d;
        }

        [Route(HttpVerbs.Get | HttpVerbs.Post, "/{deviceiseid}/setdatapointbyid/{datapointiseid}/{value}")]
        public async Task<Device> SetDatapointById(string deviceiseid, string datapointiseid, string value)
        {
            var d = await GetDevice(deviceiseid);

            if (d == null)
                return null;

            var dp = d.GetDatapointById(datapointiseid);

            if (dp == null)
                return null;

            await dp.SetValueAsync(value);

            return d;
        }

        [Route(HttpVerbs.Get | HttpVerbs.Post, "/{deviceiseid}/setmastervaluebyname/{mastervaluename}/{value}")]
        public async Task<Device> SetMastervalueByName(string deviceiseid, string mastervaluename, string value)
        {
            var d = await GetDevice(deviceiseid);

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
