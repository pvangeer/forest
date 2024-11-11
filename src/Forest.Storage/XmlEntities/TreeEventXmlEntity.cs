using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    public class TreeEventXmlEntity : XmlEntityBase
    {
        public TreeEventXmlEntity()
        {
            ClassesProbabilitySpecifications = new Collection<ExpertClassEstimationXmlEntity>();
            FixedFragilityCurveElements = new Collection<FragilityCurveElementXmlEntity>();
        }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string TreeEventType { get; set; }

        [XmlElement(ElementName = "failingevent")]
        public TreeEventXmlEntity FailingEvent { get; set; }

        [XmlElement(ElementName = "passingevent")]
        public TreeEventXmlEntity PassingEvent { get; set; }

        [XmlAttribute(AttributeName = "summary")]
        public string Summary { get; set; }

        [XmlAttribute(AttributeName = "information")]
        public string Information { get; set; }

        [XmlAttribute(AttributeName = "discussion")]
        public string Discussion { get; set; }

        [XmlArray(ElementName = "classprobabilityspecifications")]
        [XmlArrayItem(ElementName = "specification")]
        public Collection<ExpertClassEstimationXmlEntity> ClassesProbabilitySpecifications { get; }

        [XmlArray(ElementName = "fragilitycurve")]
        [XmlArrayItem(ElementName = "fragilitycurveelement")]
        public Collection<FragilityCurveElementXmlEntity> FixedFragilityCurveElements { get; }
    }
}