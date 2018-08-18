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
    class Program
    {
        static void Main(string[] args)
        {
            Client rpcClient = new Client("192.168.1.200");
            var res = rpcClient.Device_listAllDetail();
            var res2 = rpcClient.Device_listAllDetail();
            var res3 = rpcClient.Device_listAllDetail();
            var res4 = rpcClient.Device_listAllDetail();
        }
    }
}
