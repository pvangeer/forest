using System;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;
using StoryTree.Storage.XmlEntities;

namespace StoryTree.Storage.Test.XmlEntities
{
    [TestFixture]
    public class FragilityCurveElementXmlEntityTest
    {
        [Test]
        public void SerializeFragilityCurveElementXmlEntity()
        {
            var entity = new FragilityCurveElementXmlEntity
            {
                Id = 3,
                WaterLevel = 2.0,
                Probability = 0.459
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