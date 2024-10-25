using System.Linq;
using Forest.Data.Estimations;
using Forest.Data.Estimations.PerTreeEvet;
using Forest.Data.Experts;
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
            var secondTreeEvent = new TreeEvent("2");
            var treeEvent = new TreeEvent("") { FailingEvent = secondTreeEvent };
            var evenTree = new EventTree { MainTreeEvent = treeEvent };
            var firstEstimation = new TreeEventProbabilityEstimation(treeEvent);
            var secondEstimation = new TreeEventProbabilityEstimation(secondTreeEvent);
            var estimation = new ProbabilityEstimationPerTreeEvent
            {
                EventTree = evenTree,
                Estimations =
                {
                    firstEstimation,
                    secondEstimation
                }
            };

            Assert.AreEqual(0, firstEstimation.ClassProbabilitySpecification.Count);
            Assert.AreEqual(0, secondEstimation.ClassProbabilitySpecification.Count);

            var expert = new Expert();
            estimation.AddExpert(expert);

            Assert.AreEqual(1, estimation.Experts.Count);
            Assert.AreEqual(expert, estimation.Experts.First());

            Assert.AreEqual(0, firstEstimation.ClassProbabilitySpecification.Count);
            Assert.AreEqual(0, secondEstimation.ClassProbabilitySpecification.Count);
        }

        [Test]
        public void AddExpertChangesClassEstimatesWithHydraulicConditions()
        {
            var secondTreeEvent = new TreeEvent("");
            var treeEvent = new TreeEvent("") { FailingEvent = secondTreeEvent };
            var eventTree = new EventTree { MainTreeEvent = treeEvent };
            var estimationFirstTreeEvent = new TreeEventProbabilityEstimation(treeEvent);
            var estimationSecondTreeEvent = new TreeEventProbabilityEstimation(secondTreeEvent);
            var estimation = new ProbabilityEstimationPerTreeEvent
            {
                EventTree = eventTree,
                HydrodynamicConditions =
                {
                    new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1),
                    new HydrodynamicCondition(2.0, (Probability)0.001, 1, 1)
                },
                Estimations =
                {
                    estimationFirstTreeEvent,
                    estimationSecondTreeEvent
                }
            };

            Assert.AreEqual(0, estimationFirstTreeEvent.ClassProbabilitySpecification.Count);
            Assert.AreEqual(0, estimationSecondTreeEvent.ClassProbabilitySpecification.Count);

            var expert = new Expert();
            estimation.AddExpert(expert);

            Assert.AreEqual(1, estimation.Experts.Count);
            Assert.AreEqual(expert, estimation.Experts.First());

            Assert.AreEqual(2, estimationFirstTreeEvent.ClassProbabilitySpecification.Count);
            var firstSpecification = estimationFirstTreeEvent.ClassProbabilitySpecification.First();
            Assert.AreEqual(expert, firstSpecification.Expert);
            Assert.Contains(firstSpecification.HydrodynamicCondition, estimation.HydrodynamicConditions);
            var secondSpecification = estimationFirstTreeEvent.ClassProbabilitySpecification.Last();
            Assert.AreEqual(expert, secondSpecification.Expert);
            Assert.Contains(secondSpecification.HydrodynamicCondition, estimation.HydrodynamicConditions);
            Assert.AreNotEqual(firstSpecification.HydrodynamicCondition, secondSpecification.HydrodynamicCondition);

            Assert.AreEqual(2, estimationSecondTreeEvent.ClassProbabilitySpecification.Count);
            var thirdSpecification = estimationFirstTreeEvent.ClassProbabilitySpecification.First();
            Assert.AreEqual(expert, thirdSpecification.Expert);
            Assert.Contains(thirdSpecification.HydrodynamicCondition, estimation.HydrodynamicConditions);
            var fourthSpecification = estimationFirstTreeEvent.ClassProbabilitySpecification.Last();
            Assert.AreEqual(expert, fourthSpecification.Expert);
            Assert.Contains(fourthSpecification.HydrodynamicCondition, estimation.HydrodynamicConditions);
            Assert.AreNotEqual(thirdSpecification.HydrodynamicCondition, fourthSpecification.HydrodynamicCondition);
        }

        [Test]
        public void RemoveExpertChangesClassEstimatesWithHydraulicConditions()
        {
            var expertToRemove = new Expert();
            var otherExpert = new Expert();
            var hydraulicCondition1 = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            var hydraulicCondition2 = new HydrodynamicCondition(2.0, (Probability)0.001, 1, 1);
            var treeEvent = new TreeEvent("");
            var eventTree = new EventTree
            {
                MainTreeEvent = treeEvent
            };
            var treeEventProbabilityEstimation = new TreeEventProbabilityEstimation(treeEvent)
            {
                ProbabilitySpecificationType = ProbabilitySpecificationType.Classes,
                ClassProbabilitySpecification =
                {
                    new ExpertClassEstimation { Expert = expertToRemove, HydrodynamicCondition = hydraulicCondition1 },
                    new ExpertClassEstimation { Expert = expertToRemove, HydrodynamicCondition = hydraulicCondition2 },
                    new ExpertClassEstimation { Expert = otherExpert, HydrodynamicCondition = hydraulicCondition1 },
                    new ExpertClassEstimation { Expert = otherExpert, HydrodynamicCondition = hydraulicCondition2 }
                }
            };

            var probabilityEstimationPerTreeEvent = new ProbabilityEstimationPerTreeEvent
            {
                EventTree = eventTree,
                Experts =
                {
                    expertToRemove,
                    otherExpert
                },
                HydrodynamicConditions =
                {
                    hydraulicCondition1,
                    hydraulicCondition2
                },
                Estimations =
                {
                    treeEventProbabilityEstimation
                }
            };

            Assert.AreEqual(4, treeEventProbabilityEstimation.ClassProbabilitySpecification.Count);

            probabilityEstimationPerTreeEvent.RemoveExpert(expertToRemove);

            Assert.AreEqual(1, probabilityEstimationPerTreeEvent.Experts.Count);
            Assert.AreEqual(otherExpert, probabilityEstimationPerTreeEvent.Experts.First());
            Assert.AreEqual(2, treeEventProbabilityEstimation.ClassProbabilitySpecification.Count);

            var firstSpecification = treeEventProbabilityEstimation.ClassProbabilitySpecification.First();
            Assert.AreEqual(otherExpert, firstSpecification.Expert);
            Assert.AreEqual(hydraulicCondition1, firstSpecification.HydrodynamicCondition);
            Assert.Contains(firstSpecification.HydrodynamicCondition, probabilityEstimationPerTreeEvent.HydrodynamicConditions);

            var secondSpecification = treeEventProbabilityEstimation.ClassProbabilitySpecification.Last();
            Assert.AreEqual(otherExpert, secondSpecification.Expert);
            Assert.AreEqual(hydraulicCondition2, secondSpecification.HydrodynamicCondition);
            Assert.Contains(secondSpecification.HydrodynamicCondition, probabilityEstimationPerTreeEvent.HydrodynamicConditions);

            Assert.AreNotEqual(firstSpecification.HydrodynamicCondition, secondSpecification.HydrodynamicCondition);
        }

        [Test]
        public void RemoveExpertDoesNotChangeClassEstimatesNoHydraulicConditions()
        {
            var treeEvent = new TreeEvent("");
            var expertToRemove = new Expert();
            var otherExpert = new Expert();
            var eventTree = new EventTree { MainTreeEvent = treeEvent };
            var estimation = new ProbabilityEstimationPerTreeEvent
            {
                EventTree = eventTree,
                Experts =
                {
                    expertToRemove,
                    otherExpert
                }
            };
            estimation.RemoveExpert(expertToRemove);

            Assert.AreEqual(1, estimation.Experts.Count);
            Assert.AreEqual(otherExpert, estimation.Experts.First());
        }

        [Test]
        public void AddHydraulicConditionDoesNotChangesClassEstimatesNoExperts()
        {
            var treeEvent = new TreeEvent("");
            var eventTree = new EventTree { MainTreeEvent = treeEvent };
            var estimation = new ProbabilityEstimationPerTreeEvent { EventTree = eventTree };

            var hydraulicCondition = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            estimation.AddHydraulicCondition(hydraulicCondition);

            Assert.AreEqual(1, estimation.HydrodynamicConditions.Count);
            Assert.AreEqual(hydraulicCondition, estimation.HydrodynamicConditions.First());
        }

        [Test]
        public void AddHydraulicConditionChangesClassEstimatesWithExperts()
        {
            var treeEvent = new TreeEvent("");
            var eventTree = new EventTree
            {
                MainTreeEvent = treeEvent
            };
            var firstEstimation = new TreeEventProbabilityEstimation(treeEvent);
            var probabilityEstimation = new ProbabilityEstimationPerTreeEvent
            {
                EventTree = eventTree,
                Experts =
                {
                    new Expert(),
                    new Expert()
                },
                Estimations =
                {
                    firstEstimation
                }
            };

            Assert.AreEqual(0, firstEstimation.ClassProbabilitySpecification.Count);

            var hydraulicCondition = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            probabilityEstimation.AddHydraulicCondition(hydraulicCondition);

            // TODO: Should be added to the probability estimation, not the project.
            Assert.AreEqual(1, probabilityEstimation.HydrodynamicConditions.Count);

            Assert.AreEqual(hydraulicCondition, probabilityEstimation.HydrodynamicConditions.First());
            Assert.AreEqual(2, firstEstimation.ClassProbabilitySpecification.Count);
            var firstSpecification = firstEstimation.ClassProbabilitySpecification.First();
            Assert.AreEqual(hydraulicCondition, firstSpecification.HydrodynamicCondition);
            Assert.Contains(firstSpecification.Expert, probabilityEstimation.Experts);

            var secondSpecification = firstEstimation.ClassProbabilitySpecification.Last();
            Assert.AreEqual(hydraulicCondition, secondSpecification.HydrodynamicCondition);
            Assert.Contains(secondSpecification.Expert, probabilityEstimation.Experts);

            Assert.AreNotEqual(firstSpecification.Expert, secondSpecification.Expert);
        }

        [Test]
        public void RemoveHydraulicConditionChangesClassEstimatesWithExperts()
        {
            var expert1 = new Expert();
            var expert2 = new Expert();
            var hydraulicConditionToRemove = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            var hydraulicCondition2 = new HydrodynamicCondition(2.0, (Probability)0.001, 1, 1);
            var treeEvent = new TreeEvent("");
            var eventTree = new EventTree { MainTreeEvent = treeEvent };
            var treeEventProbabilityEstimation = new TreeEventProbabilityEstimation(treeEvent)
            {
                ProbabilitySpecificationType = ProbabilitySpecificationType.Classes,
                ClassProbabilitySpecification =
                {
                    new ExpertClassEstimation { Expert = expert1, HydrodynamicCondition = hydraulicConditionToRemove },
                    new ExpertClassEstimation { Expert = expert1, HydrodynamicCondition = hydraulicCondition2 },
                    new ExpertClassEstimation { Expert = expert2, HydrodynamicCondition = hydraulicConditionToRemove },
                    new ExpertClassEstimation { Expert = expert2, HydrodynamicCondition = hydraulicCondition2 }
                }
            };
            var estimation = new ProbabilityEstimationPerTreeEvent
            {
                EventTree = eventTree,
                Experts =
                {
                    expert1,
                    expert2
                },
                HydrodynamicConditions =
                {
                    hydraulicConditionToRemove,
                    hydraulicCondition2
                },
                Estimations =
                {
                    treeEventProbabilityEstimation
                }
            };

            Assert.AreEqual(4, treeEventProbabilityEstimation.ClassProbabilitySpecification.Count);

            estimation.RemoveHydraulicCondition(hydraulicConditionToRemove);

            // TODO: Move hydrodynamic conditions to estimation.
            Assert.AreEqual(1, estimation.HydrodynamicConditions.Count);
            Assert.AreEqual(hydraulicCondition2, estimation.HydrodynamicConditions.First());
            Assert.AreEqual(2, treeEventProbabilityEstimation.ClassProbabilitySpecification.Count);

            var firstSpecification = treeEventProbabilityEstimation.ClassProbabilitySpecification.First();
            Assert.AreEqual(hydraulicCondition2, firstSpecification.HydrodynamicCondition);
            Assert.AreEqual(expert1, firstSpecification.Expert);
            Assert.Contains(firstSpecification.Expert, estimation.Experts);

            var secondSpecification = treeEventProbabilityEstimation.ClassProbabilitySpecification.Last();
            Assert.AreEqual(hydraulicCondition2, secondSpecification.HydrodynamicCondition);
            Assert.AreEqual(expert2, secondSpecification.Expert);
            Assert.Contains(secondSpecification.Expert, estimation.Experts);

            Assert.AreNotEqual(firstSpecification.Expert, secondSpecification.Expert);
        }

        [Test]
        public void RemoveHydraulicConditionDoesNotChangeClassEstimatesNoExperts()
        {
            var treeEvent = new TreeEvent("");
            var eventTree = new EventTree { MainTreeEvent = treeEvent };
            var hydraulicConditionToRemove = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            var hydraulicCondition2 = new HydrodynamicCondition(2.0, (Probability)0.001, 1, 1);
            var estimation = new ProbabilityEstimationPerTreeEvent
            {
                EventTree = eventTree,
                HydrodynamicConditions =
                {
                    hydraulicConditionToRemove,
                    hydraulicCondition2
                }
            };

            estimation.RemoveHydraulicCondition(hydraulicConditionToRemove);

            Assert.AreEqual(1, estimation.HydrodynamicConditions.Count);
            Assert.AreEqual(hydraulicCondition2, estimation.HydrodynamicConditions.First());
        }
    }
}