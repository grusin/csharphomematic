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
            WebClient wc = new WebClient();

            //string req = @"{ ""version"": ""1.1"", ""method"": ""Event.poll"", ""params"": { ""_session_id_"": ""U24JE4vSTI""} }";
            string req = @"{ ""version"": ""1.1"", ""method"": ""Session.login"", ""params"": { ""username"": ""Admin"", ""password"":"""" } }";

        Console.WriteLine($"req: {req}");
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            string ret = wc.UploadString(new Uri(@"http://192.168.1.200/api/homematic.cgi"), "POST", req);
            //wc.DownloadString()
        }
    }
}
