using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using StoryTree.IO.Export;
using StoryTree.IO.Import;

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

            var writer = new ElicitationFormWriter();

            var form = new DotForm
            {
                Date = date,
                GetFileStream = () => new FileStream(eventImageFileName, FileMode.Open),
                EventTreeName = eventName,
                ExpertName = expertName,
                Nodes = new DotNode[]
                {
                    CreateNode("Knoop 1", waterLevels, frequencies),
                    CreateNode("Knoop 2", waterLevels, frequencies),
                }
            };

            writer.WriteForm(fileName, new[] { form });
        }

        private static DotNode CreateNode(string nodeName, double[] waterlevels, double[] frequencies)
        {
            var estimates = new List<DotEstimate>();
            for (int i = 0; i < waterlevels.Length; i++)
            {
                estimates.Add(new DotEstimate
                {
                    WaterLevel = waterlevels[i],
                    Frequency = frequencies[i],
                    BestEstimate =  4,
                    LowerEstimate = 3,
                    UpperEstimate = 5,
                    Comment = "Test comment"
                });
            }
            return new DotNode
            {
                NodeName = nodeName,
                Estimates = estimates.ToArray()
            };
        }
    }
}
