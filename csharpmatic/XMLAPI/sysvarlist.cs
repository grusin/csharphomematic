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
namespace csharpmatic.XMLAPI.SysvarList
{
    [XmlRoot(ElementName = "systemVariable")]
    public class SystemVariable
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "variable")]
        public string Variable { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
        [XmlAttribute(AttributeName = "value_list")]
        public string Value_list { get; set; }
        [XmlAttribute(AttributeName = "ise_id")]
        public string Ise_id { get; set; }
        [XmlAttribute(AttributeName = "min")]
        public string Min { get; set; }
        [XmlAttribute(AttributeName = "max")]
        public string Max { get; set; }
        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "subtype")]
        public string Subtype { get; set; }
        [XmlAttribute(AttributeName = "logged")]
        public string Logged { get; set; }
        [XmlAttribute(AttributeName = "visible")]
        public string Visible { get; set; }
        [XmlAttribute(AttributeName = "timestamp")]
        public string Timestamp { get; set; }
        [XmlAttribute(AttributeName = "value_name_0")]
        public string Value_name_0 { get; set; }
        [XmlAttribute(AttributeName = "value_name_1")]
        public string Value_name_1 { get; set; }
    }

    [XmlRoot(ElementName = "systemVariables")]
    public class SystemVariables
    {
        [XmlElement(ElementName = "systemVariable")]
        public List<SystemVariable> SystemVariable { get; set; }
    }

}
