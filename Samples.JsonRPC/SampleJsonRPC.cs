using csharpmatic.JsonAPI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Samples.JsonRPC
{
    class SampleJsonRPC
    {
        static void Main(string[] args)
        {
            Client rpcClient = new Client("192.168.1.200");
            var res = rpcClient.Device_listAllDetail();
            var res2 = rpcClient.Device_listAllDetail();
            var res3 = rpcClient.Device_listAllDetail();
            var res4 = rpcClient.Device_listAllDetail();
            //var res5 = rpcClient.Interface_GetRSSI();

            bool alarmArmed = rpcClient.GetOrCreateSystemBoolVariable("Alarm Armed", false);
            bool alarmTriggered = rpcClient.GetOrCreateSystemBoolVariable("Alarm Triggered", false);            

            //var x = rpcClient.GetOrCreateSystemBoolVariable("TEST Alarm Armed", false);

            //rpcClient.SetSystemVariable("link", "https://hooks.slack.com/services/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");

            rpcClient.SetSystemVariable("Alarm Armed", "0");
        }        
    }
}
