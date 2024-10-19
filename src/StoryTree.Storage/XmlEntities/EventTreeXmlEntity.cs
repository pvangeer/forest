using System;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    [Serializable]
    public class EventTreeXmlEntity : XmlEntityBase
    {
        [XmlAttribute(AttributeName = "maintreeeventid")]
        public long MainTreeEventId { get; set; }
    }
}
