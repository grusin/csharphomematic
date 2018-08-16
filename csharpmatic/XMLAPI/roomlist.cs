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

namespace csharpmatic.XMLAPI.RoomList
{
    [XmlRoot(ElementName = "room")]
    public class Room
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "ise_id")]
        public string Ise_id { get; set; }
        [XmlElement(ElementName = "channel")]
        public List<Channel> Channel { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [XmlRoot(ElementName = "channel")]
    public class Channel
    {
        [XmlAttribute(AttributeName = "ise_id")]
        public string Ise_id { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [XmlRoot(ElementName = "roomList")]
    public class RoomList
    {
        [XmlElement(ElementName = "room")]
        public List<Room> Room { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
