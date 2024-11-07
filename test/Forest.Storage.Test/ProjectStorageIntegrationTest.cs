using System.Xml;
using System.Xml.Serialization;
using Forest.Data;
using Forest.Storage.Create;
using Forest.Storage.XmlEntities;
using NUnit.Framework;

namespace Forest.Storage.Test
{
    [TestFixture]
    public class ProjectStorageIntegrationTest
    {
        [Test]
        public void ProjectXmlSerializes()
        {
            var project = new ProjectXmlEntity
            {
                VersionInformation = new VersionXmlEntity
                {
                    Creator = "test",
                    Created = "some date"
                },
                ForestAnalysis = ForestAnalysisFactory.CreateStandardNewAnalysis().Create(new PersistenceRegistry())
            };

            var serializer = new XmlSerializer(typeof(ProjectXmlEntity));

            var filePath = @"C:\Test\test.xml";
            using (var writer = XmlWriter.Create(filePath))
            {
                serializer.Serialize(writer, project);
            }

            var readSerializer = new XmlSerializer(typeof(ProjectXmlEntity));
            ProjectXmlEntity projectXmlEntity;
            using (var reader = XmlReader.Create(filePath))
            {
                projectXmlEntity = (ProjectXmlEntity)readSerializer.Deserialize(reader);
            }

            Assert.IsNotNull(projectXmlEntity);
        }
    }
}