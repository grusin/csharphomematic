/* 
Licensed under the Apache License, Version 2.0
    
http://www.apache.org/licenses/LICENSE-2.0
*/

//
//Generated using: https://xmltocsharp.azurewebsites.net/
//

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace csharpmatic.XMLAPI.CGI.DeviceList
{
    [XmlRoot(ElementName = "channel")]
    public class Channel
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "address")]
        public string Address { get; set; }
        [XmlAttribute(AttributeName = "ise_id")]
        public string Ise_id { get; set; }
        [XmlAttribute(AttributeName = "direction")]
        public string Direction { get; set; }
        [XmlAttribute(AttributeName = "parent_device")]
        public string Parent_device { get; set; }
        [XmlAttribute(AttributeName = "index")]
        public string Index { get; set; }
        [XmlAttribute(AttributeName = "group_partner")]
        public string Group_partner { get; set; }
        [XmlAttribute(AttributeName = "aes_available")]
        public string Aes_available { get; set; }
        [XmlAttribute(AttributeName = "transmission_mode")]
        public string Transmission_mode { get; set; }
        [XmlAttribute(AttributeName = "visible")]
        public string Visible { get; set; }
        [XmlAttribute(AttributeName = "ready_config")]
        public string Ready_config { get; set; }
        [XmlAttribute(AttributeName = "operate")]
        public string Operate { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [XmlRoot(ElementName = "device")]
    public class Device
    {
        [XmlElement(ElementName = "channel")]
        public List<Channel> Channel { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "address")]
        public string Address { get; set; }
        [XmlAttribute(AttributeName = "ise_id")]
        public string Ise_id { get; set; }
        [XmlAttribute(AttributeName = "interface")]
        public string Interface { get; set; }
        [XmlAttribute(AttributeName = "device_type")]
        public string Device_type { get; set; }
        [XmlAttribute(AttributeName = "ready_config")]
        public string Ready_config { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [XmlRoot(ElementName = "deviceList")]
    public class DeviceList
    {
        [XmlElement(ElementName = "device")]
        public List<Device> Device { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
