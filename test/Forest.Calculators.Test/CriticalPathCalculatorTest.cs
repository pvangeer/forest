using System.Linq;
using Forest.Data.Tree;
using NUnit.Framework;

namespace Forest.Calculators.Test
{
    [TestFixture]
    public class CriticalPathCalculatorTest
    {
        [Test]
        public void CalculateCriticalPathWorks()
        {
            var targetNode = new TreeEvent("Scooooreee!!", TreeEventType.Failing);
            var node1 = new TreeEvent("", TreeEventType.Failing) { FailingEvent = targetNode, PassingEvent = new TreeEvent("", TreeEventType.Passing) };
            var node2 = new TreeEvent("", TreeEventType.Failing) { FailingEvent = node1 };
            var mainNode = new TreeEvent("", TreeEventType.MainEvent) { FailingEvent = node2 };

            var path = CriticalPathCalculator.GetCriticalPath(mainNode, targetNode).ToArray();

            Assert.AreEqual(4, path.Length);
            Assert.AreEqual(mainNode, path[0]);
            Assert.AreEqual(node2, path[1]);
            Assert.AreEqual(node1, path[2]);
            Assert.AreEqual(targetNode, path[3]);
        }
    }
}