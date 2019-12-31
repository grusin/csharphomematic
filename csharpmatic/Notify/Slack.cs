using csharpmatic.Generic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Notify 
{
    public class Slack : INotify
    {
        public Uri WebHookURL { get; set; }
        public string Name { get; private set; }

        public Slack(Uri webhook)
        {
            WebHookURL = webhook;
            Name = "Slack";
        }

        public Slack(string webhook)
        {
            WebHookURL = new Uri(webhook);
            Name = "Slack";
        }

        public async Task SendTextMessageAsync(string message)
        {
            string json = getBody(message);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                var result = await wc.UploadStringTaskAsync(WebHookURL, json);                
            }
        }

        private string getBody(string message)
        {
            var j = new JObject();
            j.Add("text", message);

            return j.ToString();            
        }
        
        public void Dispose()
        {

        }

        public static Slack TryFromCCU(DeviceManager dm)
        {
            JsonRPCAPIClient.Client c = new JsonRPCAPIClient.Client(dm.HttpServerUri.Host);

            var url = c.GetSystemVariable("link").Value<string>();

            if (String.IsNullOrWhiteSpace(url))
                return null;

            Slack s = new Slack(url);

            return s;
        }
    }
}
