using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.CGI.Mastervalue
{





    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class mastervalue
    {

        private mastervalueDevice deviceField;

        /// <remarks/>
        public mastervalueDevice device
        {
            get
            {
                return this.deviceField;
            }
            set
            {
                this.deviceField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class mastervalueDevice
    {

        private mastervalueDeviceMastervalue[] mastervalueField;

        private string nameField;

        private ushort ise_idField;

        private string device_typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("mastervalue")]
        public mastervalueDeviceMastervalue[] mastervalue
        {
            get
            {
                return this.mastervalueField;
            }
            set
            {
                this.mastervalueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort ise_id
        {
            get
            {
                return this.ise_idField;
            }
            set
            {
                this.ise_idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string device_type
        {
            get
            {
                return this.device_typeField;
            }
            set
            {
                this.device_typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class mastervalueDeviceMastervalue
    {

        private string nameField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}
