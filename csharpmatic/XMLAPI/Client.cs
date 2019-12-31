using csharpmatic.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csharpmatic.XMLAPI
{
    public class Client
    {
        public Uri HttpServerUri { get; private set; }
        public DeviceList.DeviceList DeviceList { get; private set; }
        public FunctionList.FunctionList FunctionList { get; private set; }
        public RoomList.RoomList RoomList { get; private set; }
        public StateList.StateList StateList { get; private set;  }
        public Dictionary<string, MastervalueList.mastervalue[]> MasterValueListByChannel { get; private set; }
        private MastervalueList.mastervalue MasterValueListTemp;
        private Task MasterValueListNewRefreshTask;

        public SysvarList.SystemVariables SystemVariablesList { get; private set; } 

        private DateTime lastFullUpdateTimestamp = DateTime.MinValue;

        public TimeSpan FullRecheckInternval = new TimeSpan(0, 0, 15);

        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private T XMLGetRequest<T>(Uri getUrl)
        {
            var task = XMLGetRequestAsync<T>(getUrl);
            task.Wait();
            return task.Result;
        }

        private async Task<T> XMLGetRequestAsync<T>(Uri getUrl)
        {
            using (var wc = new WebClient())
            {
                var str = await wc.DownloadStringTaskAsync(getUrl.ToString());

                using (var rdr = new StringReader(str))
                {
                    //Stopwatch s = new Stopwatch();
                    //s.Start();

                    T dl = default(T);

                    Task d = new Task(() =>
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        dl = (T)serializer.Deserialize(rdr);
                    });

                    d.Start();

                    await d;

                    //s.Stop();
                    //Console.WriteLine("XMLGetRequestAsync<{0}> deserialization time: {1} ms", typeof(T), s.ElapsedMilliseconds);
                    return dl;
                }
            }
        }

        private T SafeXMLGetRequest<T>(Uri getUrl, int attempts=3, int retryWaitMs=1000)
        {
            var task = SafeXMLGetRequestAsync<T>(getUrl, attempts, retryWaitMs);
            task.Wait();
            return task.Result;
        }

        private async Task<T> SafeXMLGetRequestAsync<T>(Uri getUrl, int attempts = 3, int retryWaitMs = 1000)
        {
            Exception e = null;

            for (int i = 0; i < attempts; ++i)
            {
                try
                {
                    T ret = await XMLGetRequestAsync<T>(getUrl);
                    return ret;
                }
                catch (Exception ex)
                {
                    LOGGER.Warn(String.Format("Error while fetching {0}. Attempt {1} of {2}.", getUrl, i + 1, attempts), ex);
                    e = ex;
                }

                Thread.Sleep(retryWaitMs);
            }

            throw new Exception("HTTP Get Request failed: " + getUrl, e);
        }

        public Client(Uri httpServerUri)
        {
            HttpServerUri = httpServerUri;
        }

        public Client(string httpServerUri)
        {
            HttpServerUri = new Uri(httpServerUri);
        }

        private async Task FetchDeviceListAsync()
        {
            //Console.WriteLine("FetchDeviceListAsync() started");

            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/devicelist.cgi");
            DeviceList = await SafeXMLGetRequestAsync<DeviceList.DeviceList>(uri);

            //Console.WriteLine("FetchDeviceListAsync() finished");
        }
        
        private async Task FetchRoomListAsync()
        {
            //Console.WriteLine("FetchRoomListAsync() started");

            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/roomlist.cgi");
            RoomList = await SafeXMLGetRequestAsync<RoomList.RoomList>(uri);

            //Console.WriteLine("FetchMasteFetchRoomListAsyncrValueListAsync() finished");
        }

        private async Task FetchStateListAsync()
        {
            //Console.WriteLine("FetchStateListAsync() started");

            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/statelist.cgi");
            StateList = await SafeXMLGetRequestAsync<StateList.StateList>(uri);

            //Console.WriteLine("FetchStateListAsync() finished");
        }

        private async Task FetchFunctionListAsync()
        {
            //Console.WriteLine("FetchFunctionListAsync() started");

            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/functionlist.cgi");
            FunctionList = await SafeXMLGetRequestAsync<FunctionList.FunctionList>(uri);

            //Console.WriteLine("FetchFunctionListAsync() finished");
        }

        public async Task FetchSysvarListAsync()
        {
            //Console.WriteLine("FetchSysvarListAsync() started");

            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/sysvarlist.cgi");
            SystemVariablesList = await SafeXMLGetRequestAsync<SysvarList.SystemVariables>(uri);

            //Console.WriteLine("FetchSysvarListAsync() finished");
        }

        private async Task FetchMasterValueListAsync(Task deviceListRefreshTask)
        {
            //Console.WriteLine("FetchMasterValueListAsync() started");

            if (deviceListRefreshTask != null)
                await deviceListRefreshTask;

            var idse = this.DeviceList.Device.Where(w => w.Interface == "HmIP-RF").SelectMany(s => s.Channel).Select(s => s.Ise_id).ToList();
            MasterValueListTemp = await FetchMasterValueListAsync(idse);

            //Console.WriteLine("FetchMasterValueListAsync() finished");
        }
        
        private async Task<MastervalueList.mastervalue> FetchMasterValueListAsync(IEnumerable<string> iseids)
        {
            string iseidsString = String.Join(",", iseids);

            //original XMLAPI mastervalue does not work well on HMIP devices I have, so i have my own, that works... 
            Uri uri = new Uri(HttpServerUri, @"/addons/xmlapi/mastervalue2.cgi?tcpport=2010&device_id=" + iseidsString);
            var mv = await SafeXMLGetRequestAsync<MastervalueList.mastervalue>(uri);

            return mv;
        }         

        public async Task<bool> FetchData(bool forceFullReload = false)
        {
            Dictionary<string, Task> taskWait = new Dictionary<string, Task>();
            
            taskWait.Add("FetchStateListAsync()", FetchStateListAsync());                       
            
            if (DateTime.Now - lastFullUpdateTimestamp > FullRecheckInternval || forceFullReload)
            {               
                var dlt = FetchDeviceListAsync();
                taskWait.Add("FetchDeviceListAsync()", dlt);

                //only run master value refresh if previous task has completed
                if (MasterValueListNewRefreshTask == null || MasterValueListNewRefreshTask.IsCompleted)
                    MasterValueListNewRefreshTask = FetchMasterValueListAsync(dlt);

                //if we run first time, block till we get first master value list
                if (MasterValueListByChannel == null)
                    taskWait.Add("FetchMasterValueListAsync(dlt)", MasterValueListNewRefreshTask);

                taskWait.Add("FetchFunctionListAsync()", FetchFunctionListAsync());
                taskWait.Add("FetchRoomListAsync()", FetchRoomListAsync());
                taskWait.Add("FetchSysvarListAsync()", FetchSysvarListAsync());               
                lastFullUpdateTimestamp = DateTime.Now;            
            }

            while (taskWait.Count > 0)
            {
                var arr = taskWait.Values.ToArray();
                int i = Task.WaitAny(arr);
                var task = arr[i];
                string taskName = taskWait.Where(w => w.Value.Id == task.Id).First().Key;

                await task;

                taskWait.Remove(taskName);
             }

            //master value list is long, it takes about 5s to generate and download
            //download is preferemed in the async task, that just gets spawned now and then
            //once tasks finishes the output of the task is made active

            //if master value refresh task completed, swap the variables around
            if (MasterValueListNewRefreshTask != null && MasterValueListNewRefreshTask.IsCompleted)
            {
                if (MasterValueListByChannel == null)
                    MasterValueListByChannel = new Dictionary<string, MastervalueList.mastervalue[]>();

                foreach (var c in MasterValueListTemp.channels)
                {
                    if (c.mastervalue == null)
                        continue;

                    if (!MasterValueListByChannel.ContainsKey(c.ise_id))
                        MasterValueListByChannel.Add(c.ise_id, c.mastervalue);
                    else
                        MasterValueListByChannel[c.ise_id] = c.mastervalue;
                }

                return true;
            }


            return false;
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
                else
                    break;
            }
        }

        public async Task SetISEIDValueAsync(string iseid, string newvalue)
        {
            //FIXME: validate that both iseid and newvalue do contain only a-z and 0-9 values

            Uri uri = new Uri(HttpServerUri, String.Format(@"addons/xmlapi/statechange.cgi?ise_id={0}&new_value={1}", iseid, newvalue));

            int tries = 3;

            for (int i = 0; i < tries; ++i)
            {
                var res = await SafeXMLGetRequestAsync<StateChange.Result>(uri);

                if (res.Changed.Id != iseid || res.Changed.New_value != newvalue)
                {
                    if (i + 1 >= tries)
                        throw new Exception(String.Format("State change failed for {0}={1}. Got {2}={3} instead.", iseid, newvalue, res.Changed.Id, res.Changed.New_value));
                }
                else
                    break;
            }
        }
    }
}
