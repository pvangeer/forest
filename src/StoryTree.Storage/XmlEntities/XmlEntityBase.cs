using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    public class XmlEntityBase
    {
        [XmlAttribute(AttributeName = "id")]
        public long Id { get; set; }
    }
}