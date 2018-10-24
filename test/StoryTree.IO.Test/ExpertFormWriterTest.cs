using System;
using NUnit.Framework;

namespace StoryTree.IO.Test
{
    [TestFixture]
    public class ExpertFormWriterTest
    {
        [Test]
        public void FormWriterWrites()
        {
            var fileName = @"D:\Test\ThisIsATest.xlsx";
            var eventImageFileName = @"C:\src\storytree\src\StoryTree.IO\giraffe-png-images-8.png";
            string eventName = "TestEvent";
            string expertName = "Test Expert";
            DateTime date = DateTime.Now;
            double[] waterLevels = {1.0, 2.0, 3.0, 4.0};
            double[] frequencies = {0.0001, 0.001, 0.01, 0.1};
            string[] eventNodes = {"Knoop 1", "Knoop 2"};

            var writer = new ExpertFormWriter();
            writer.WriteForm(fileName, eventName, eventImageFileName, expertName, date, waterLevels, frequencies, eventNodes);
        }
    }
}
