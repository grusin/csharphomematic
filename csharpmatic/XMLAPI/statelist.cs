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

namespace csharpmatic.XMLAPI.StateList
{
    [XmlRoot(ElementName = "datapoint")]
    public class Datapoint
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "ise_id")]
        public string Ise_id { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
        [XmlAttribute(AttributeName = "valuetype")]
        public string Valuetype { get; set; }
        [XmlAttribute(AttributeName = "valueunit")]
        public string Valueunit { get; set; }
        [XmlAttribute(AttributeName = "timestamp")]
        public string Timestamp { get; set; }
        [XmlAttribute(AttributeName = "operations")]
        public string Operations { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [XmlRoot(ElementName = "channel")]
    public class Channel
    {
        [XmlElement(ElementName = "datapoint")]
        public List<Datapoint> Datapoint { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "ise_id")]
        public string Ise_id { get; set; }
        [XmlAttribute(AttributeName = "index")]
        public string Index { get; set; }
        [XmlAttribute(AttributeName = "visible")]
        public string Visible { get; set; }
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
        [XmlAttribute(AttributeName = "ise_id")]
        public string Ise_id { get; set; }
        [XmlAttribute(AttributeName = "unreach")]
        public string Unreach { get; set; }
        [XmlAttribute(AttributeName = "config_pending")]
        public string Config_pending { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [XmlRoot(ElementName = "stateList")]    public class StateList
    {
        [XmlElement(ElementName = "device")]
        public List<Device> Device { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}

