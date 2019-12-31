using csharpmatic.Interfaces;
using csharpmatic.Notify;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace csharpmatic.Generic
{
    public class DeviceManager
    {
        internal XMLAPI.Client LegacyXMLAPIClient;
        internal JsonRPCAPIClient.Client JsonAPIClient;
        internal XMLAPIClient.Client XMLAPIClient;

        //private 
        public List<Device> Devices { get; private set; }
        public Dictionary<string, Device> DevicesByISEID { get; private set; }
        private Dictionary<string, Datapoint> PrevDataPointsByISEID { get; set; }
        public List<DatapointEvent> Events { get; private set; }
        public Uri HttpServerUri { get { return LegacyXMLAPIClient.HttpServerUri; } }
        public List<Room> Rooms { get; private set; }
        public Dictionary<string, Room> RoomsByName { get; private set; }

        private Dictionary<string, IAutomation> RegisteredAutomations;
        private Dictionary<string, INotify> RegisteredNotificationServices;

        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public object RefreshLock { get; set; }

        public DeviceManager(string serverAddress, bool useLegacyAPI = true)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

            LOGGER.Info($"Starting device manager for {serverAddress}");

            RefreshLock = new object();

            if (useLegacyAPI)
                LegacyXMLAPIClient = new XMLAPI.Client("http://" + serverAddress);
            else
                XMLAPIClient = new XMLAPIClient.Client("http://" + serverAddress);

            JsonAPIClient = new JsonRPCAPIClient.Client(serverAddress);
            RegisteredAutomations = new Dictionary<string, IAutomation>();
            RegisteredNotificationServices = new Dictionary<string, INotify>();
            Devices = new List<Device>();

            Refresh();
        }

        public IAutomation RegisterAutomation(IAutomation a)
        {
            if (a == null)
                throw new ArgumentNullException();

            if (RegisteredAutomations.ContainsKey(a.Name))
                throw new ArgumentException($"Canot register automation '{a.Name}' of type '{a.GetType()}'. It's already registered.");

            RegisteredAutomations.Add(a.Name, a);

            LOGGER.Info($"Registered automation '{a.Name}' of type '{a.GetType()}'");

            return a;
        }

        public INotify RegisterNotificationService(INotify n)
        {
            if (n == null)
                throw new ArgumentNullException();

            if (RegisteredAutomations.ContainsKey(n.Name))
                throw new ArgumentException($"Canot register notification service '{n.Name}' of type '{n.GetType()}'. It's already registered.");

            RegisteredNotificationServices.Add(n.Name, n);

            LOGGER.Info($"Registered automation '{n.Name}' of type '{n.GetType()}'");

            return n;
        }

        public async Task SendNotificationAsync(string message)
        {
            if (RegisteredNotificationServices.Count == 0)
                return;

            List<Task> list = new List<Task>();

            foreach (var ns in RegisteredNotificationServices)
                list.Add(ns.Value.SendTextMessageAsync(message));
            
            await Task.WhenAll(list);
        }

        public IAutomation GetAutomation(string name)
        {
            RegisteredAutomations.TryGetValue(name, out IAutomation a);

            return a;
        }

        public List<T> GetDevicesImplementingInterface<T>() where T : class
        {
            List<T> list = new List<T>();

            return Devices.Where(w => w is T).Select(s => s as T).ToList();
        }

        public List<DatapointEvent> NewRefresh()
        {
            return null;
        }

        public List<DatapointEvent> Refresh()
        {
            if (LegacyXMLAPIClient == null)
                return NewRefresh();

            //this does not need lock, it's internal data structure, and this actually takes time
            var refreshTask = LegacyXMLAPIClient.FetchData();
            refreshTask.Wait();

            bool fullRefresh = refreshTask.Result;

            //this should be quick
            lock (RefreshLock)
            {
                BuildPrevDataPointsByISEID();                              

                if (fullRefresh)
                {
                    BuildDeviceList();
                    BuildRoomList();
                }
                else
                {
                    foreach (var cgi_dev in LegacyXMLAPIClient.StateList.Device)
                    {
                        Device d = null;
                        if (DevicesByISEID.TryGetValue(cgi_dev.Ise_id, out d))
                            d.FillFromStateList(cgi_dev);
                    }
                }
            }

            return GetEvents();
        }

        public void Work()
        {
            var events = Refresh();

            lock (RefreshLock)
            {
                foreach (var a in RegisteredAutomations.Values)
                    a.Work();
            }
        }

        private List<DatapointEvent> GetEvents()
        {
            List<DatapointEvent> list = new List<DatapointEvent>();
                       
            foreach (var d in Devices)
            {
                foreach (var c in d.Channels)
                {
                    foreach (var dpkvp in c.Datapoints)
                    {
                        Datapoint current = dpkvp.Value;

                        Datapoint prev = null;
                        PrevDataPointsByISEID.TryGetValue(current.ISEID, out prev);

                        //new device was just added
                        if (prev == null)
                            list.Add(new DatapointEvent(current, null));
                        else if (current.GetValueString() != prev.GetValueString()|| current.OperationsCounter != prev.OperationsCounter)
                            list.Add(new DatapointEvent(current, prev));                        
                    }
                }
            }

            Events = list;

            return list;
        }             

        private void BuildRoomList()
        {
            Rooms = new List<Room>();
            RoomsByName = new Dictionary<string, Room>();

            foreach (var cgiroom in LegacyXMLAPIClient.RoomList.Room)
            {
                Room r = new Room(cgiroom.Name, cgiroom.Ise_id, this);
                Rooms.Add(r);
                RoomsByName.Add(r.Name, r);
            }
        }

        private void BuildPrevDataPointsByISEID()
        {
            if (Devices == null)
                PrevDataPointsByISEID = new Dictionary<string, Datapoint>();
            else
                PrevDataPointsByISEID = Devices.SelectMany(d => d.Channels).SelectMany(c => c.Datapoints).Select(dp => dp.Value.Clone()).ToDictionary(ks => ks.ISEID);
        }

        private void BuildDeviceList()
        {           
            Devices = new List<Device>();
            DevicesByISEID = new Dictionary<string, Device>();

            foreach (var d in LegacyXMLAPIClient.DeviceList.Device)
            {                
                Device gd = DeviceFactory.CreateInstance(d, LegacyXMLAPIClient, this);
                                
                Devices.Add(gd);
                DevicesByISEID.Add(gd.ISEID, gd);
            }                           
        }
    }
}
