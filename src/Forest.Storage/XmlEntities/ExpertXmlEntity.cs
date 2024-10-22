using System;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    public class ExpertXmlEntity : PersonXmlEntity
    {
        [XmlAttribute(AttributeName = "expertise")]
        public string Expertise { get; set; }

        [XmlAttribute(AttributeName = "organization")]
        public string Organization { get; set; }

        [XmlAttribute(AttributeName = "order")]
        public long Order { get; set; }
    }
}