using System;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    public class TreeEventXmlEntity : XmlEntityBase
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string TreeEventType { get; set; }

        [XmlElement(ElementName = "failingevent")]
        public TreeEventXmlEntity FailingEvent { get; set; }

        [XmlElement(ElementName = "passingevent")]
        public TreeEventXmlEntity PassingEvent { get; set; }

        [XmlAttribute(AttributeName = "summary")]
        public string Summary { get; set; }

        [XmlAttribute(AttributeName = "information")]
        public string Information { get; set; }
    }
}