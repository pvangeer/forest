﻿using NUnit.Framework;
using StoryTree.Data;
using StoryTree.Storage.Create;
using StoryTree.Storage.XmlEntities;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace StoryTree.Storage.Test.Create
{
    [TestFixture]
    public class PersonCreateExtensionTest
    {
        [Test]
        public void ExtensionCreates()
        {
            var person = new Person
            {
                Name = "Piet",
                Email = "Klaas@Piet.com",
                Telephone = "01455-4592"
            };

            PersonXmlEntity xmlEntity = person.Create(new PersistenceRegistry());
            Assert.IsNotNull(xmlEntity);
            var xmlSerializer = new XmlSerializer(typeof(PersonXmlEntity));
            var xmlText = "";
            int hash;
            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, xmlEntity);
                xmlText = textWriter.ToString();
                hash = textWriter.ToString().GetHashCode();
            }

            Assert.IsNotEmpty(xmlText);
            Assert.AreNotEqual(0,hash);
        }

        [Test]
        public void CanSerializeProject()
        {
            var xmlEntity = new ProjectXmlEntity
            {
                EventTreeProject = new EventTreeProject().Create(new PersistenceRegistry()),
                VersionInformation =
                {
                    Creator = "Me",
                    Created = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    FileVersion = "24.1",
                    LastAuthor = "Me",
                    LastChanged = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                }
            };

            Stream stream = new MemoryStream();

            // Write the project to a stream
            var xmlSerializer = new XmlSerializer(typeof(ProjectXmlEntity));
            var xmlText = "";
            var outputFileName = "Testfile.xml";
            using (var textWriter = XmlWriter.Create(outputFileName))
            {
                xmlSerializer.Serialize(textWriter, xmlEntity);
                xmlText = stream.ToString();
            }

            Assert.IsNotEmpty(xmlText);

            stream.Position = 0;

            // Read the project again.
            ProjectXmlEntity readEntity;
            using (var reader = XmlReader.Create(outputFileName))
            {
                readEntity = (ProjectXmlEntity)xmlSerializer.Deserialize(reader);
            }

            Assert.IsNotNull(readEntity);
        }
    }
}