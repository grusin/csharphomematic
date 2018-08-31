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

namespace Samples.RestApi.RoomController
{
    public class RoomController : WebApiController
    {
        [WebApiHandler(HttpVerbs.Get, "/api/rooms")]
        public bool GetRooms(WebServer server, HttpListenerContext context)
        {
            List<Room> list = new List<Room>();

            lock(Program.DeviceManager.RefreshLock)
            {
                foreach(var dr in Program.DeviceManager.Rooms)
                {
                    Room r = new Room(dr);                 
                    list.Add(r);
                }               
            }
          
            context.JsonResponse(list);
           
            return true;
        }

        [WebApiHandler(HttpVerbs.Get, "/api/rooms/{iseid}")]
        public bool GetRoom(WebServer server, HttpListenerContext context, string iseid)
        {            
            lock (Program.DeviceManager.RefreshLock)
            {
                var dr = Program.DeviceManager.Rooms.Where(w => w.ISEID == iseid).FirstOrDefault();
                if(dr != null)
                {
                    Room r = new Room(dr);
                    context.JsonResponse(r);
                    return true;
                }
            }
           
            return false;
        }

        [WebApiHandler(HttpVerbs.Get, "/api/rooms/{iseid}/temp/{newtemp}")]
        public bool SetRoomTemp(WebServer server, HttpListenerContext context, string iseid, decimal newtemp)
        {
            List<Room> list = new List<Room>();
            
            //locks should be placed on all data updates and queries, but never should be put on any long waiting operations
            lock (Program.DeviceManager.RefreshLock)
            {
                var dr = Program.DeviceManager.Rooms.Where(w => w.ISEID == iseid).FirstOrDefault();
                if (dr != null)
                {
                    var dp = dr.TempControlDevices.GroupLeader.Set_Point_Temperature;
                    dp.SetRoomValue(newtemp);
                    Room r = new Room(dr);
                    context.JsonResponse(r);
                    return true;
                }
            }

            return false;
        }

        [WebApiHandler(HttpVerbs.Get, "/api/rooms/{iseid}/boostmode/{newstate}")]
        public bool SetRoom(WebServer server, HttpListenerContext context, string iseid, bool newstate)
        {
            List<Room> list = new List<Room>();

            //locks should be placed on all data updates and queries, but never should be put on any long waiting operations
            lock (Program.DeviceManager.RefreshLock)
            {
                var dr = Program.DeviceManager.Rooms.Where(w => w.ISEID == iseid).FirstOrDefault();
                if (dr != null)
                {
                    var dp = dr.TempControlDevices.GroupLeader.Boost_Mode;
                    dp.SetRoomValue(newstate);

                    Room r = new Room(dr);
                    context.JsonResponse(r);

                    return true;
                }
            }

            return false;
        }

        // You can override the default headers and add custom headers to each API Response.
        public override void SetDefaultHeaders(HttpListenerContext context)
        {
            context.NoCache();
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
