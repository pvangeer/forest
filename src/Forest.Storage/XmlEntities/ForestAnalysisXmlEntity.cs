using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    [XmlRoot(ElementName = "analysis", IsNullable = false)]
    public class ForestAnalysisXmlEntity : XmlEntityBase
    {
        public ForestAnalysisXmlEntity()
        {
            Experts = new Collection<ExpertXmlEntity>();
            HydraulicConditions = new Collection<HydrodynamicConditionXmlEntity>();
            EventTreeXmlEntities = new Collection<EventTreeXmlEntity>();
        }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "assessmentsection")]
        public string AssessmentSection { get; set; }

        [XmlAttribute(AttributeName = "information")]
        public string ProjectInformation { get; set; }

        [XmlElement(ElementName = "projectleader")]
        public PersonXmlEntity ProjectLeader { get; set; }

        [XmlArray(ElementName = "eventtrees")]
        [XmlArrayItem(ElementName = "eventtree")]
        public Collection<EventTreeXmlEntity> EventTreeXmlEntities { get; set; }

        [XmlArray(ElementName = "experts")]
        [XmlArrayItem(ElementName = "expert")]
        public Collection<ExpertXmlEntity> Experts { get; set; }

        [XmlArray(ElementName = "hydraulicconditions")]
        [XmlArrayItem(ElementName = "hydrauliccondition")]
        public Collection<HydrodynamicConditionXmlEntity> HydraulicConditions { get; set; }

        // TODO: Add probability estimations
    }
}