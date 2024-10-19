using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
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

        [XmlAttribute(AttributeName = "failingeventid")]
        public long FailingEventId { get; set; }

        [XmlAttribute(AttributeName = "passingeventid")]
        public long PassingEvent { get; set; }

        [XmlAttribute(AttributeName = "summary")]
        public string Summary { get; set; }

        [XmlAttribute(AttributeName = "information")]
        public string Information { get; set; }

        [XmlAttribute(AttributeName = "discussion")]
        public string Discussion { get; set; }

        [XmlAttribute(AttributeName = "probabilityspecificationtype")] 
        public byte ProbabilitySpecificationType { get; set; }

        [XmlAttribute(AttributeName = "fixedprobability")]
        public double FixedProbability { get; set; }

        [XmlArray(ElementName = "classprobabilityspecifications")]
        [XmlArrayItem(ElementName = "specification")]
        public Collection<ExpertClassEstimationXmlEntity> ClassesProbabilitySpecifications { get; }

        [XmlArray(ElementName = "fragilitycurve")]
        [XmlArrayItem(ElementName = "fragilitycurveelement")]
        public Collection<FragilityCurveElementXmlEntity> FixedFragilityCurveElements { get; }

        
    }
}
