using System;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    [Serializable]
    public class ExpertXmlEntity : PersonXmlEntity
    {
        [XmlAttribute(AttributeName = "expertise")]
        public string Expertise { get; set; }

        [XmlAttribute(AttributeName = "organization")]
        public string Organization { get; set; }
    }
}
