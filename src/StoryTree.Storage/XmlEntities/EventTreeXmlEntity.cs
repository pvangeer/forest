using System;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    [Serializable]
    public class EventTreeXmlEntity : XmlEntityBase
    {
        [XmlElement(ElementName = "maintreeevent")]
        public TreeEventXmlEntity MainTreeEvent { get; set; }
    }
}