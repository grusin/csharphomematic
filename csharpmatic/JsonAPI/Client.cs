using csharpmatic.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace csharpmatic.JsonAPI
{
    public class Client
    {
        public string SessionID { get; private set; }
        public Uri Endpoint { get; private set; }
        
        private JsonRPC.Client RpcClient { get; }

        public TimeSpan SessionRenewFrequency { get; set; }
        private DateTime LastSessionRenew;

        public Client(string serverAddress)
        {
            Endpoint = new Uri("http://" + serverAddress + "/api/homematic.cgi");

            RpcClient = new JsonRPC.Client(Endpoint.ToString());

            SessionRenewFrequency = new TimeSpan(0, 5, 0);
        }

        private void Session_Renew(bool force = false)
        {
            if (SessionID == null)
                return;

            if (DateTime.Now - LastSessionRenew < SessionRenewFrequency && force == false)
                return;

            var request = RpcClient.NewRequest("Session.renew", JObject.Parse(@"{ _session_id_: '" + SessionID + "'}"));
            var response = RpcClient.Rpc(request);

            if (response.Error != null)
                SessionID = null;

            return;
        }

        public void Session_Login()
        {
            //check if Session ID needs renewal.
            Session_Renew();

            //already logged in if SessionID is filled in
            if (SessionID != null)
                return;

            var request = RpcClient.NewRequest("Session.login", JObject.Parse(@"{ username: 'Admin', password: ''}"));
            var response = RpcClient.Rpc(request);

            if (response.Error != null)
                throw new Exception("Error while loging in: " + response.Error.ToString());

            LastSessionRenew = DateTime.Now;
            SessionID = response.Result.ToString();
        }

        public void Session_Logout()
        {
            var request = RpcClient.NewRequest("Session.logout", JObject.Parse(@"{ _session_id_: '" + SessionID + "'}"));
            var response = RpcClient.Rpc(request);

            SessionID = null;
        }

        public JToken Session_RpcCall(string method, JObject parameters = null)
        {
            Session_Login();

            if (parameters == null)
                parameters = new JObject();

            if (parameters.ContainsKey("_session_id_"))
                parameters["_session_id_"] = SessionID;
            else
                parameters.Add("_session_id_", SessionID);

            var request = RpcClient.NewRequest(method, parameters);
            var response = RpcClient.Rpc(request);

            if (response.Error != null)
            {
                if (response.Error.Code == 400)
                {
                    SessionID = null;
                    Session_Login();
                    parameters["_session_id_"] = SessionID;
                    response = RpcClient.Rpc(request);
                }

                if (response.Error != null)
                    throw new Exception(String.Format("RPC call {0} failed: {1}", JsonConvert.SerializeObject(request), response.Error.Message));
            }

            return response.Result;
        }

        public JToken Device_listAllDetail()
        {
            Session_Login();

            return Session_RpcCall("Device.listAllDetail");
        }

        public JToken Channel_SetMasterValues(Channel c, List<MasterValue> list)
        {
            Session_Login();

            //not excaly json RPC call, but it's dependent on session id, so it fits there
            //http://192.168.1.200/config/ic_ifacecmd.cgi?sid=%40U24JE4vSTI%40&iface=HmIP-RF&address=000A1709AB4C94%3A1&peer=MASTER&ps_type=MASTER&paramid=&pnr=&cmd=set_profile&P1_TEMPERATURE_MONDAY_5=16.5

            var par = HttpUtility.ParseQueryString("");           
            par.Add("sid", "@" + SessionID + "@");
            par.Add("iface", c.Device.Interface);
            par.Add("address", c.Address);
            par.Add("peer", "MASTER");
            par.Add("ps_type", "MASTER");
            par.Add("paramid", "");
            par.Add("pnr", "");
            par.Add("cmd", "set_profile");

            foreach(var mv in list)
                par.Add(mv.Name, mv.Value.ToString());

            var b = new UriBuilder();
            b.Host = Endpoint.Host;
            b.Path = "/config/ic_ifacecmd.cgi";
            b.Query = par.ToString();

            Uri uri = b.Uri;

            using (WebClient wc = new WebClient())
            {
                string str = wc.DownloadString(uri);
            }

            return null;
        }

        public JToken Device_startCommunicationTest(Device d)
        {
            return Session_RpcCall("Device.startComTest", JObject.Parse(@"{ id: '" + d.ISEID + "'}"));
        }

        public JToken Interface_GetRSSI(string interfaceName="HmIP-RF")
        {
            Session_Login();

            return Session_RpcCall("Interface.rssiInfo", JObject.Parse(@"{ interface: '" + interfaceName + "'}"));
        }
    }
}
