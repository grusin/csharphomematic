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

namespace csharpmatic.XMLAPI.CGI.FunctionList
{
    [XmlRoot(ElementName = "channel")]
    public class Channel
    {
        [XmlAttribute(AttributeName = "address")]
        public string Address { get; set; }
        [XmlAttribute(AttributeName = "ise_id")]
        public string Ise_id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [XmlRoot(ElementName = "function")]
    public class Function
    {
        [XmlElement(ElementName = "channel")]
        public List<Channel> Channel { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "ise_id")]
        public string Ise_id { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [XmlRoot(ElementName = "functionList")]
    public class FunctionList
    {
        [XmlElement(ElementName = "function")]
        public List<Function> Function { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
