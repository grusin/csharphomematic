using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csharpmatic.XMLAPI.CGI
{
    public class CGIClient
    {
        public Uri HttpServerUri { get; private set; }
        public DeviceList.DeviceList DeviceList { get; private set; }
        public FunctionList.FunctionList FunctionList { get; private set; }
        public RoomList.RoomList RoomList { get; private set; }
        public StateList.StateList StateList { get; private set;  }
        public MastervalueList.mastervalue MasterValueList { get; private set; }

        public SysvarList.SystemVariables SystemVariablesList { get; private set; }

        private static WebClient WebClient = new WebClient();

        private DateTime lastFullUpdateTimestamp = DateTime.MinValue;

        public TimeSpan FullRecheckInternval = new TimeSpan(0, 0, 15);
                
        private static T XMLGetRequest<T>(Uri getUrl)
        {
            using (Stream rdr = WebClient.OpenRead(getUrl))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                T dl = (T)serializer.Deserialize(rdr);
                return dl;
            }
        }

        private static T SafeXMLGetRequest<T>(Uri getUrl, int attempts=3, int retryWaitMs=1000)
        {
            Exception e = null;

            for(int i=0; i<attempts; ++i)
            {
                try
                {
                    T ret = XMLGetRequest<T>(getUrl);
                    return ret;
                }
                catch(Exception ex)
                {
                    e = ex;
                }

                Thread.Sleep(retryWaitMs);
            }

            throw new Exception("HTTP Get Request failed: " + getUrl, e);
        }

        public CGIClient(Uri httpServerUri)
        {
            HttpServerUri = httpServerUri;
        }

        public CGIClient(string httpServerUri)
        {
            HttpServerUri = new Uri(httpServerUri);
        }

        private void FetchDeviceList()
        {
            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/devicelist.cgi");
            DeviceList = SafeXMLGetRequest<DeviceList.DeviceList>(uri);
        }

        private void FetchRoomList()
        {
            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/roomlist.cgi");
            RoomList = SafeXMLGetRequest<RoomList.RoomList>(uri);
        }

        private void FetchStateList()
        {
            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/statelist.cgi");
            StateList = SafeXMLGetRequest<StateList.StateList>(uri);
        }

        private void FetchFunctionList()
        {
            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/functionlist.cgi");
            FunctionList = SafeXMLGetRequest<FunctionList.FunctionList>(uri);
        }

        private void FetchMasterValueList()
        {
            var idse = this.DeviceList.Device.Where(w => w.Interface == "HmIP-RF").SelectMany(s => s.Channel).Select(s => s.Ise_id).ToList();

            MasterValueList = FetchMasterValueList(idse);
        }
            
    
        private MastervalueList.mastervalue FetchMasterValueList(IEnumerable<string> iseids)
        {
            string iseidsString = String.Join(",", iseids);

            Uri uri = new Uri(HttpServerUri, @"/addons/xmlapi/mastervalue.cgi?tcpport=2010&device_id=" + iseidsString);
            var mv = SafeXMLGetRequest<MastervalueList.mastervalue>(uri);

            return mv;
        }

        public void FetchSysvarList()
        {
            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/sysvarlist.cgi");
            SystemVariablesList = SafeXMLGetRequest<SysvarList.SystemVariables>(uri);
        }

        public void FetchData(bool force=false)
        {             
            if(DateTime.Now - lastFullUpdateTimestamp > FullRecheckInternval || force)
            {
                FetchDeviceList();
                FetchFunctionList();
                FetchRoomList();
                FetchSysvarList();
                FetchMasterValueList();
                lastFullUpdateTimestamp = DateTime.Now;
            }         

            FetchStateList();            
        }      
        
        public void SetISEIDValue(string iseid, string newvalue)
        {
            //FIXME: validate that both iseid and newvalue do contain only a-z and 0-9 values

            Uri uri = new Uri(HttpServerUri, String.Format(@"addons/xmlapi/statechange.cgi?ise_id={0}&new_value={1}", iseid, newvalue));

            int tries = 3;

            for (int i = 0; i < tries; ++i)
            {
                var res = SafeXMLGetRequest<StateChange.Result>(uri);

                if (res.Changed.Id != iseid || res.Changed.New_value != newvalue)
                {
                    if (i + 1 >= tries)
                        throw new Exception(String.Format("State change failed for {0}={1}. Got {2}={3} instead.", iseid, newvalue, res.Changed.Id, res.Changed.New_value));
                }
            }
        } 

        public void SetMasterValue(string iseid, string masterValueName, string newValue)
        {

        }
    }
}
