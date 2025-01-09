using System.Linq;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Estimations.PerTreeEvent.Experts;
using Forest.Data.Hydrodynamics;
using Forest.Data.Probabilities;
using Forest.Data.Tree;
using NUnit.Framework;

namespace Forest.Data.Test.Estimations
{
    [TestFixture]
    public class ProbabilityEstimationPerTreeEventExtensionsTest
    {
        [Test]
        public void AddExpertDoesNotChangesClassEstimatesNoHydraulicConditions()
        {
            var secondTreeEvent = new TreeEvent("2", TreeEventType.Failing);
            var treeEvent = new TreeEvent("", TreeEventType.MainEvent) { FailingEvent = secondTreeEvent };
            var evenTree = new EventTree { MainTreeEvent = treeEvent };
            var firstEstimation = new TreeEventProbabilityEstimate(treeEvent);
            var secondEstimation = new TreeEventProbabilityEstimate(secondTreeEvent);
            var estimation = new ProbabilityEstimationPerTreeEvent
            {
                EventTree = evenTree,
                Estimates =
                {
                    firstEstimation,
                    secondEstimation
                }
            };

            var expert = new Expert();
            estimation.AddExpert(expert);

            Assert.AreEqual(1, estimation.Experts.Count);
            Assert.AreEqual(expert, estimation.Experts.First());
        }
    }
}