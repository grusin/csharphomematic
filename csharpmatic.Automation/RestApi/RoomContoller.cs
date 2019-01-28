using csharpmatic.Generic;
using csharpmatic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;
using Unosquare.Net;

namespace csharpmatic.Automation.RestApi
{
    public class RoomController : WebApiController
    {
        public static DeviceManager DeviceManager { get; set; }

        public RoomController(IHttpContext context) : base(context)
        {
           
        }
        
        [WebApiHandler(HttpVerbs.Get, "/api/rooms")]
        public bool GetRooms()
        {
            List<Room> list = new List<Room>();

            lock (DeviceManager.RefreshLock)
            {
                foreach (var dr in DeviceManager.Rooms)
                {
                    Room r = new Room(dr);
                    list.Add(r);
                }
            }

            return this.JsonResponse(list);
        }

        [WebApiHandler(HttpVerbs.Get, "/api/rooms/{iseid}")]
        public bool GetRoom(string iseid)
        {
            lock (DeviceManager.RefreshLock)
            {
                var dr = DeviceManager.Rooms.Where(w => w.ISEID == iseid).FirstOrDefault();
                if (dr != null)
                {
                    Room r = new Room(dr);
                    return this.JsonResponse(r);
                }
            }

            return false;
        }

        [WebApiHandler(HttpVerbs.Get, "/api/rooms/{iseid}/temp/{newtemp}")]
        public bool SetRoomTemp(string iseid, decimal newtemp)
        {
            List<Room> list = new List<Room>();

            //locks should be placed on all data updates and queries, but never should be put on any long waiting operations
            lock (DeviceManager.RefreshLock)
            {
                var dr = DeviceManager.Rooms.Where(w => w.ISEID == iseid).FirstOrDefault();
                if (dr != null)
                {
                    var dp = dr.TempControlDevices.GroupLeader.Set_Point_Temperature;
                    dp.SetRoomValue(newtemp);
                    Room r = new Room(dr);
                    return this.JsonResponse(r);
                }
            }

            return false;
        }

        [WebApiHandler(HttpVerbs.Get, "/api/rooms/{iseid}/boostmode/{newstate}")]
        public bool SetRoom(string iseid, bool newstate)
        {
            List<Room> list = new List<Room>();

            //locks should be placed on all data updates and queries, but never should be put on any long waiting operations
            lock (DeviceManager.RefreshLock)
            {
                var dr = DeviceManager.Rooms.Where(w => w.ISEID == iseid).FirstOrDefault();
                if (dr != null)
                {
                    var dp = dr.TempControlDevices.GroupLeader.Boost_Mode;
                    dp.SetRoomValue(newstate);

                    Room r = new Room(dr);
                    return this.JsonResponse(r);
                }
            }

            return false;
        }

        // You can override the default headers and add custom headers to each API Response.
        public override void SetDefaultHeaders()
        {
            this.NoCache();
            this.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
