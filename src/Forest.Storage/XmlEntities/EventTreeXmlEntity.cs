using System;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    public class EventTreeXmlEntity : XmlEntityBase
    {
        [XmlElement(ElementName = "maintreeevent")]
        public TreeEventXmlEntity MainTreeEvent { get; set; }

        [XmlAttribute(AttributeName = "order")]
        public long Order { get; set; }
    }
}