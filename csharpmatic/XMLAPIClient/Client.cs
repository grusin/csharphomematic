using csharpmatic.Generic;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace csharpmatic.XMLAPIClient
{
    public class Client
    {
        public Uri HttpServerUri { get; private set; }

        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Client(Uri httpServerUri)
        {
            HttpServerUri = httpServerUri;
        }

        public Client(string httpServerUri)
        {
            HttpServerUri = new Uri(httpServerUri);
        }

        public List<DatapointEvent> Update(DeviceManager dm)
        {
            UpdateDeviceList(dm).Wait();
            return null;
        }

        private async Task<XmlDocument> GetXMLDocumentAsync(Uri getUrl)
        {
            using (var wc = new WebClient())
            {
                var str = await wc.DownloadStringTaskAsync(getUrl.ToString());

                var xml = new XmlDocument();
                xml.LoadXml(str);

                return xml;
            }
        }

        private async Task<XmlDocument> SafeGetXMLDocumentAsync(Uri getUrl, int attempts = 3, int retryWaitMs = 1000)
        {
            Exception e = null;

            for (int i = 0; i < attempts; ++i)
            {
                try
                {
                    var ret = await GetXMLDocumentAsync(getUrl);
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

        private async Task<XmlDocument> GetXMLDeviceListAsync()
        {
            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/devicelist.cgi");
            return await SafeGetXMLDocumentAsync(uri);
        }

        public async Task UpdateDeviceList(DeviceManager dm=null)
        {
            var xml = await GetXMLDeviceListAsync();
            var dl = xml["deviceList"];

            foreach (XmlElement dev in xml["deviceList"])
            {
                var devAttr = dev.Attributes;

                //channel
                foreach (XmlElement chan in dev.ChildNodes)
                {
                    var chanAttr = chan.Attributes;
                }            
            }
        }
    }
}
