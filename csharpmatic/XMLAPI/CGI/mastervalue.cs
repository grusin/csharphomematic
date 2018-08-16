using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.CGI.MastervalueList
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class mastervalue
    {
        [System.Xml.Serialization.XmlElementAttribute("device")]
        public channel[] channels;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal value { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
