using System;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    [Serializable]
    public class ProbabilityXmlEntity : XmlEntityBase
    {
        [XmlAttribute(AttributeName = "value")]
        public double Value { get; set; }
    }
}
