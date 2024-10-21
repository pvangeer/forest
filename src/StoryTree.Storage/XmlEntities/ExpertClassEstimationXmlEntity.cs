using System;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    [Serializable]
    public class ExpertClassEstimationXmlEntity : XmlEntityBase
    {
        [XmlAttribute(AttributeName = "order")]
        public long Order { get; set; }

        [XmlAttribute(AttributeName = "expertid")]
        public long ExpertId { get; set; }

        [XmlAttribute(AttributeName = "hydraulicconditionid")]
        public long HydraulicConditionId { get; set; }

        [XmlAttribute(AttributeName = "minimumestimation")]
        public byte MinEstimation { get; set; }

        [XmlAttribute(AttributeName = "averageestimation")]
        public byte AverageEstimation { get; set; }

        [XmlAttribute(AttributeName = "maximumestimation")]
        public byte MaxEstimation { get; set; }
    }
}