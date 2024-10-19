using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    [Serializable]
    [XmlRoot(ElementName = "project", IsNullable = false)]
    public class ProjectXmlEntity : XmlEntityBase
    {
        public const string VersionInformationElementName = "versioninformation";

        public ProjectXmlEntity()
        {
            VersionInformation = new VersionXmlEntity();
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

        [XmlAttribute(AttributeName = "projectleaderid")]
        public long ProjectLeaderId { get; set; }

        [XmlAttribute(AttributeName = "eventtreeid")]
        public long EventTreeId { get; set; }

        [XmlArray(ElementName = "experts")]
        [XmlArrayItem(ElementName = "expert")]
        public Collection<ExpertXmlEntity> Experts { get; set; }

        [XmlArray(ElementName = "hydraulicconditions")]
        [XmlArrayItem(ElementName = "hydrauliccondition")]
        public Collection<HydraulicConditionXmlEntity> HydraulicConditions { get; set; }

        [XmlElement(ElementName = VersionInformationElementName)]
        public VersionXmlEntity VersionInformation { get; set; }
    }
}
