using System;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    [Serializable]
    public class FragilityCurveElementXmlEntity : XmlEntityBase
    {
        [XmlAttribute(AttributeName = "waterlevel")]
        public double WaterLevel { get; set; }

        [XmlAttribute(AttributeName = "probabilityid")]
        public long ProbabilityId { get; set; }
    }
}
