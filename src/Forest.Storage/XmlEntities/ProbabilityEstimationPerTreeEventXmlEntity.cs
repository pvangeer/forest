using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    public class ProbabilityEstimationPerTreeEventXmlEntity : XmlEntityBase
    {
        public ProbabilityEstimationPerTreeEventXmlEntity()
        {
            HydrodynamicConditions = new Collection<HydrodynamicConditionXmlEntity>();
            Estimations = new Collection<TreeEventProbabilityEstimateXmlEntity>();
        }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "eventtreeid")]
        public long EventTreeId { get; set; }

        [XmlArray(ElementName = "hydraulicconditions")]
        [XmlArrayItem(ElementName = "hydrauliccondition")]
        public Collection<HydrodynamicConditionXmlEntity> HydrodynamicConditions { get; set; }

        [XmlArray(ElementName = "estimates")]
        [XmlArrayItem(ElementName = "estimate")]
        public Collection<TreeEventProbabilityEstimateXmlEntity> Estimations { get; set; }

        [XmlAttribute(AttributeName = "order")]
        public long Order { get; set; }
    }
}