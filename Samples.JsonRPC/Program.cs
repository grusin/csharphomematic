using JsonRPC;
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
            using (Client rpcClient = new Client("http://192.168.1.200/api/homematic.cgi"))
            {
                Request request;
                GenericResponse response;

                //get session id
                request = rpcClient.NewRequest("Session.login", JObject.Parse(@"{ username: 'Admin', password: ''}"));
                response = rpcClient.Rpc(request);

                string sessionId = response.Result.ToString();

                //log out
                //Session.logout

                //list methods
                request = rpcClient.NewRequest("Session.logout", JObject.Parse(@"{ _session_id_: '" + sessionId + "'}"));
                response = rpcClient.Rpc(request);

                //list interface
                request = rpcClient.NewRequest("Interface.listInterfaces", JObject.Parse(@"{ _session_id_: '" + sessionId + "'}"));
                response = rpcClient.Rpc(request);
            }
        }
    }
}
