using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.CGI.MastervalueList
{

    [System.Xml.Serialization.XmlTypeAttribute("mastervalue")]
    public partial class MastervalueList
    {
        [System.Xml.Serialization.XmlElementAttribute("device")]
        public channel[] channels;

    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class channel
    {

        [System.Xml.Serialization.XmlElementAttribute("mastervalue")]
        public mastervalue[] mastervalue { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ise_id { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("device_type")]
        public string channel_type { get; set; }           
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class mastervalue
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name { get; set; }
               
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value { get; set; }
    }
}
