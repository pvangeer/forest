using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    public class ProbabilityEstimationPerTreeEventXmlEntity
    {
        public ProbabilityEstimationPerTreeEventXmlEntity()
        {
            Experts = new Collection<ExpertXmlEntity>();
            HydrodynamicConditions = new Collection<HydrodynamicConditionXmlEntity>();
        }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "eventtreeid")]
        public long EventTreeId { get; set; }

        [XmlArray(ElementName = "experts")]
        [XmlArrayItem(ElementName = "expert")]
        public Collection<ExpertXmlEntity> Experts { get; set; }

        [XmlArray(ElementName = "hydraulicconditions")]
        [XmlArrayItem(ElementName = "hydrauliccondition")]
        public Collection<HydrodynamicConditionXmlEntity> HydrodynamicConditions { get; set; }

        [XmlAttribute(AttributeName = "order")]
        public long Order { get; set; }

        [XmlArray(ElementName = "estimates")]
        [XmlArrayItem(ElementName = "estimate")]
        public Collection<TreeEventProbabilityEstimateXmlEntity> Estimations { get; set; }
    }
}