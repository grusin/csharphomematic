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
    public class RoomController : WebApiController
    {
        public static DeviceManager DeviceManager { get; set; }

        public RoomController()
        {
           
        }
        
        [Route(HttpVerbs.Get, "/rooms")]
        public async Task<List<Room>> GetRooms()
        {
            List<Room> list = new List<Room>();

            lock (DeviceManager.RefreshLock)
            {
                foreach (var dr in DeviceManager.Rooms)
                {
                    Room r = new Room(dr);
                    list.Add(r);
                }

                return list;
            }           
        }

        [Route(HttpVerbs.Get, "/rooms/{iseid}")]
        public async Task<Room> GetRoom(string iseid)
        {
            lock (DeviceManager.RefreshLock)
            {
                var dr = DeviceManager.Rooms.Where(w => w.ISEID == iseid).FirstOrDefault();
                if (dr != null)
                {
                    Room r = new Room(dr);
                    return r;
                }                  
            }

            return null;
        }

        [Route(HttpVerbs.Get, "/rooms/{iseid}/temp/{newtemp}")]
        public async Task<Room> SetRoomTemp(string iseid, decimal newtemp)
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
                    return r;
                }
            }

            return null;
        }

        [Route(HttpVerbs.Get, "/rooms/{iseid}/boostmode/{newstate}")]
        public async Task<Room> SetRoom(string iseid, bool newstate)
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
                    return r;
                }
            }

            return null;
        }

        // You can override the default headers and add custom headers to each API Response.
        public void SetDefaultHeaders()
        {            
            this.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
