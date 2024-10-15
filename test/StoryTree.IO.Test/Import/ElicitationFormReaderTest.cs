using System.Linq;
using NUnit.Framework;
using StoryTree.IO.Import;

namespace StoryTree.IO.Test.Import
{
    [TestFixture]
    public class ElicitationFormReaderTest
    {
        [Test]
        public void ReaderCanReadForm()
        {
            var formLocation = @"C:\src\storytree\test\StoryTree.IO.Test\test-data\DotFormExample.xlsx";
            var forms = ElicitationFormReader.ReadElicitationForm(formLocation);

            Assert.IsNotNull(forms);
            Assert.AreEqual(1,forms.Count());
            var form = forms.First();
            Assert.AreEqual("Andre van Hoven",form.ExpertName);
            Assert.AreEqual(2018,form.Date.Year);
            Assert.AreEqual(11,form.Date.Month);
            Assert.AreEqual(2,form.Date.Day);

            Assert.AreEqual(2, form.Nodes.Length);

            var firstNode = form.Nodes.First();
            Assert.AreEqual("test", firstNode.NodeName);
            Assert.AreEqual(6,firstNode.Estimates.Length);
            var expectedWaterLevels = new[] {2.3, 2.6, 2.9, 3.2, 3.5, 3.8};
            var expectedFrequencies = new[] { 0.0333, 0.01, 0.00333, 0.001, 0.000333, 0.0001 };
            for (var index = 0; index < firstNode.Estimates.Length; index++)
            {
                var estimate = firstNode.Estimates[index];
                Assert.AreEqual(expectedWaterLevels[index],estimate.WaterLevel);
                Assert.AreEqual(expectedFrequencies[index], estimate.Frequency);
                Assert.AreEqual(2,estimate.LowerEstimate);
                Assert.AreEqual(3,estimate.BestEstimate);
                Assert.AreEqual(4,estimate.UpperEstimate);
                if (index == 0)
                {
                    Assert.AreEqual("Dit is een toelichting", estimate.Comment);
                }
                else
                {
                    Assert.IsEmpty(estimate.Comment);
                }
            }

            var secondNode = form.Nodes.ElementAt(1);
            Assert.AreEqual("test2", secondNode.NodeName);
            Assert.AreEqual(6, secondNode.Estimates.Length);
            for (var index = 0; index < secondNode.Estimates.Length; index++)
            {
                var estimate = secondNode.Estimates[index];
                Assert.AreEqual(expectedWaterLevels[index], estimate.WaterLevel);
                Assert.AreEqual(expectedFrequencies[index], estimate.Frequency);
                Assert.AreEqual(2, estimate.LowerEstimate);
                Assert.AreEqual(3, estimate.BestEstimate);
                Assert.AreEqual(4, estimate.UpperEstimate);
                Assert.IsEmpty(estimate.Comment);
            }
        }
    }
}
