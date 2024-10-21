using System;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    [Serializable]
    [XmlRoot(ElementName = "project", IsNullable = false)]
    public class ProjectXmlEntity
    {
        public const string VersionInformationElementName = "versioninformation";

        public ProjectXmlEntity()
        {
            VersionInformation = new VersionXmlEntity();
        }

        [XmlElement(ElementName = VersionInformationElementName)]
        public VersionXmlEntity VersionInformation { get; set; }

        [XmlElement(ElementName = "eventtreeproject")]
        public EventTreeProjectXmlEntity EventTreeProject { get; set; }
    }
}