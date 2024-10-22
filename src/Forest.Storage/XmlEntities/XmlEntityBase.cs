using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    public class XmlEntityBase
    {
        [XmlAttribute(AttributeName = "id")]
        public long Id { get; set; }
    }
}