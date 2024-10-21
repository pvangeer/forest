using System;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    [Serializable]
    public class PersonXmlEntity : XmlEntityBase
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "email")]
        public string Email { get; set; }

        [XmlAttribute(AttributeName = "telephone")]
        public string Telephone { get; set; }
    }
}