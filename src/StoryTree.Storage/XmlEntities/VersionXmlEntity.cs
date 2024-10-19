using System;
using System.Xml.Serialization;

namespace StoryTree.Storage.XmlEntities
{
    [Serializable]
    public class VersionXmlEntity
    {
        public const string FileVersionElementName = "fileversion";
        public const string LastChangedElementName = "lastchanged";

        public VersionXmlEntity()
        {
            FileVersion = CurrentVersion;
            LastChanged = VersionInfo.CurrentDateTime;
            LastAuthor = VersionInfo.CurrentUser;
        }

        public static string CurrentVersion => $"{VersionInfo.Year}.{VersionInfo.MajorVersion}";

        [XmlElement(ElementName = FileVersionElementName)]
        public string FileVersion { get; set; }

        [XmlElement(ElementName = "creator")]
        public string Creator { get; set; }

        [XmlElement(ElementName = "created")]
        public string Created { get; set; }

        [XmlElement(ElementName = "lastauthor")]
        public string LastAuthor { get; set; }

        [XmlElement(ElementName = LastChangedElementName)]
        public string LastChanged { get; set; }
    }
}