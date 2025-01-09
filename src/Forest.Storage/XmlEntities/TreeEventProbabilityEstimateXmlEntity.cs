using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    public class TreeEventProbabilityEstimateXmlEntity : XmlEntityBase
    {
        public TreeEventProbabilityEstimateXmlEntity()
        {
            FragilityCurve = new ObservableCollection<FragilityCurveElementXmlEntity>();
        }

        [XmlAttribute(AttributeName = "treeeventid")]
        public long TreeEventId { get; }

        [XmlAttribute(AttributeName = "specificationtype")]
        public string ProbabilitySpecificationType { get; set; }

        [XmlAttribute(AttributeName = "fixedprobability")]
        public double FixedProbability { get; set; }

        [XmlArray(ElementName = "fragilitycurve")]
        [XmlArrayItem(ElementName = "fragilitycurveelement")]
        public ObservableCollection<FragilityCurveElementXmlEntity> FragilityCurve { get; set; }

        [XmlAttribute(AttributeName = "order")]
        public long Order { get; set; }
    }
}