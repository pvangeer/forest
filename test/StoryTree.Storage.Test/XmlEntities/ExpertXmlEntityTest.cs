using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using NUnit.Framework;
using StoryTree.Storage.XmlEntities;

namespace StoryTree.Storage.Test.XmlEntities
{
    [TestFixture]
    public class ExpertXmlEntityTest
    {
        [Test]
        public void SerializeExpertXmlEntity()
        {
            var expertise = "Nothing";
            var organization = "Nowhere";
            var entity = new ExpertXmlEntity
            {
                Id = 3,
                Expertise = expertise,
                Organization = organization,
                Name = "No one",
                Email = "mail@you.com",
                Telephone = "0536-324653"
            };

            Stream stream = new MemoryStream();

            var serializer = new XmlSerializer(typeof(ExpertXmlEntity));
            serializer.Serialize(stream, entity);
            serializer.Serialize(Console.Out, entity);

            // TODO: Add correct asserts (by deserializing?) This is not a test.
            Assert.AreNotEqual(0, stream.Length);

        }
    }
}
