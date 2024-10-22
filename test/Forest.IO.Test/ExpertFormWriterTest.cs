using System;
using System.Collections.Generic;
using System.IO;
using Forest.IO.Export;
using Forest.IO.Import;
using NUnit.Framework;

namespace Forest.IO.Test
{
    [TestFixture]
    public class ExpertFormWriterTest
    {
        [Test]
        public void FormWriterWrites()
        {
            var fileName = @"D:\Test\ThisIsATest.xlsx";
            var eventImageFileName = @"C:\src\storytree\src\Forest.IO\giraffe-png-images-8.png";
            var expertName = "Test Expert";
            var date = DateTime.Now;
            double[] waterLevels = { 1.0, 2.0, 3.0, 4.0 };
            double[] frequencies = { 0.0001, 0.001, 0.01, 0.1 };

            var writer = new ElicitationFormWriter();

            var form = new DotForm
            {
                Date = date,
                GetFileStream = () => new FileStream(eventImageFileName, FileMode.Open),
                ExpertName = expertName,
                Nodes = new[]
                {
                    CreateNode("Knoop 1", waterLevels, frequencies),
                    CreateNode("Knoop 2", waterLevels, frequencies)
                }
            };

            writer.WriteForm(fileName, form);
        }

        private static DotNode CreateNode(string nodeName, double[] waterlevels, double[] frequencies)
        {
            var estimates = new List<DotEstimate>();
            for (var i = 0; i < waterlevels.Length; i++)
                estimates.Add(new DotEstimate
                {
                    WaterLevel = waterlevels[i],
                    Frequency = frequencies[i],
                    BestEstimate = 4,
                    LowerEstimate = 3,
                    UpperEstimate = 5,
                    Comment = "Test comment"
                });
            return new DotNode
            {
                NodeName = nodeName,
                Estimates = estimates.ToArray()
            };
        }
    }
}