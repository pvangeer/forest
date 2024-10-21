using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    [Serializable]
    [XmlRoot(ElementName = "eventtreeproject", IsNullable = false)]
    public class EventTreeProjectXmlEntity : XmlEntityBase
    {
        public const string VersionInformationElementName = "versioninformation";

        public EventTreeProjectXmlEntity()
        {
            Experts = new Collection<ExpertXmlEntity>();
            HydraulicConditions = new Collection<HydraulicConditionXmlEntity>();
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

        [XmlElement(ElementName = "eventtree")]
        public EventTreeXmlEntity EventTree { get; set; }

        [XmlArray(ElementName = "experts")]
        [XmlArrayItem(ElementName = "expert")]
        public Collection<ExpertXmlEntity> Experts { get; set; }

        [XmlArray(ElementName = "hydraulicconditions")]
        [XmlArrayItem(ElementName = "hydrauliccondition")]
        public Collection<HydraulicConditionXmlEntity> HydraulicConditions { get; set; }
    }
}
