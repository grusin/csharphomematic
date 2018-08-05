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

namespace csharpmatic.XMLAPI.CGI.StateChange
{
    [XmlRoot(ElementName = "changed")]
    public class Changed
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "new_value")]
        public string New_value { get; set; }
    }

    [XmlRoot(ElementName = "result")]
    public class Result
    {
        [XmlElement(ElementName = "changed")]
        public Changed Changed { get; set; }
    }

}