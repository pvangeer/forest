using System.Linq;
using Forest.Data.Estimations;
using Forest.Data.Experts;
using Forest.Data.Hydrodynamics;
using Forest.Data.Services;
using Forest.Data.Tree;
using NUnit.Framework;

namespace Forest.Data.Test
{
    [TestFixture]
    public class ProjectManipulationServiceTest
    {
        [Test]
        public void AddExpertDoesNotChangesClassEstimatesNoHydraulicConditions()
        {
            var treeEvent = new TreeEvent("");
            var project = new ForestAnalysis
            {
                EventTree = { MainTreeEvent = treeEvent }
            };
            var projectManipulationService = new AnalysisManipulationService(project);
            //TODO: Rewrite test
            //Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            var expert = new Expert();
            projectManipulationService.AddExpert(expert);

            Assert.AreEqual(1, project.Experts.Count);
            Assert.AreEqual(expert, project.Experts.First());
            //TODO: Rewrite test
            //Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);
        }

        /*[Test]
        public void AddExpertChangesClassEstimatesWithHydraulicConditions()
        {
            var treeEvent = new TreeEvent("");
            var project = new ForestAnalysis
            {
                EventTree = { MainTreeEvent = treeEvent },
                HydrodynamicConditions =
                {
                    new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1),
                    new HydrodynamicCondition(2.0, (Probability)0.001, 1, 1)
                }
            };
            var projectManipulationService = new AnalysisManipulationService(project);

            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            var expert = new Expert();
            projectManipulationService.AddExpert(expert);

            Assert.AreEqual(1, project.Experts.Count);
            Assert.AreEqual(expert, project.Experts.First());
            Assert.AreEqual(2, treeEvent.ClassesProbabilitySpecification.Count);
            var firstSpecification = treeEvent.ClassesProbabilitySpecification.First();
            Assert.AreEqual(expert, firstSpecification.Expert);
            Assert.Contains(firstSpecification.HydrodynamicCondition, project.HydrodynamicConditions);

            var secondSpecification = treeEvent.ClassesProbabilitySpecification.Last();
            Assert.AreEqual(expert, secondSpecification.Expert);
            Assert.Contains(secondSpecification.HydrodynamicCondition, project.HydrodynamicConditions);

            Assert.AreNotEqual(firstSpecification.HydrodynamicCondition, secondSpecification.HydrodynamicCondition);
        }*/

        [Test]
        public void RemoveExpertChangesClassEstimatesWithHydraulicConditions()
        {
            var expertToRemove = new Expert();
            var otherExpert = new Expert();
            var hydraulicCondition1 = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            var hydraulicCondition2 = new HydrodynamicCondition(2.0, (Probability)0.001, 1, 1);
            var treeEvent = new TreeEvent("");
            var eventTree = new EventTree()
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
                Estimations =
                {
                    treeEventProbabilityEstimation
                }
            };
            var project = new ForestAnalysis
            {
                EventTree = eventTree,
                HydrodynamicConditions =
                {
                    hydraulicCondition1,
                    hydraulicCondition2
                },
                ProbabilityEstimations =
                {
                    probabilityEstimationPerTreeEvent
                }
            };

            var projectManipulationService = new AnalysisManipulationService(project);

            Assert.AreEqual(4, treeEventProbabilityEstimation.ClassProbabilitySpecification.Count);

            projectManipulationService.RemoveExpert(expertToRemove);

            Assert.AreEqual(1, probabilityEstimationPerTreeEvent.Experts.Count);
            Assert.AreEqual(otherExpert, probabilityEstimationPerTreeEvent.Experts.First());
            Assert.AreEqual(2, treeEventProbabilityEstimation.ClassProbabilitySpecification.Count);

            var firstSpecification = treeEventProbabilityEstimation.ClassProbabilitySpecification.First();
            Assert.AreEqual(otherExpert, firstSpecification.Expert);
            Assert.AreEqual(hydraulicCondition1, firstSpecification.HydrodynamicCondition);
            Assert.Contains(firstSpecification.HydrodynamicCondition, project.HydrodynamicConditions);

            var secondSpecification = treeEventProbabilityEstimation.ClassProbabilitySpecification.Last();
            Assert.AreEqual(otherExpert, secondSpecification.Expert);
            Assert.AreEqual(hydraulicCondition2, secondSpecification.HydrodynamicCondition);
            Assert.Contains(secondSpecification.HydrodynamicCondition, project.HydrodynamicConditions);

            Assert.AreNotEqual(firstSpecification.HydrodynamicCondition, secondSpecification.HydrodynamicCondition);
        }

        [Test]
        public void RemoveExpertDoesNotChangeClassEstimatesNoHydraulicConditions()
        {
            var treeEvent = new TreeEvent("");
            var expertToRemove = new Expert();
            var otherExpert = new Expert();
            var eventTree = new EventTree { MainTreeEvent = treeEvent };
            var project = new ForestAnalysis
            {
                EventTree = eventTree,
                Experts =
                {
                    expertToRemove,
                    otherExpert
                }
            };
            var projectManipulationService = new AnalysisManipulationService(project);

            projectManipulationService.RemoveExpert(expertToRemove);

            Assert.AreEqual(1, project.Experts.Count);
            Assert.AreEqual(otherExpert, project.Experts.First());
        }

        [Test]
        public void AddHydraulicConditionDoesNotChangesClassEstimatesNoExperts()
        {
            var treeEvent = new TreeEvent("");
            var eventTree = new EventTree { MainTreeEvent = treeEvent };
            var project = new ForestAnalysis
            {
                EventTree = eventTree
            };
            var projectManipulationService = new AnalysisManipulationService(project);

            var hydraulicCondition = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            projectManipulationService.AddHydraulicCondition(hydraulicCondition);

            Assert.AreEqual(1, project.HydrodynamicConditions.Count);
            Assert.AreEqual(hydraulicCondition, project.HydrodynamicConditions.First());
        }

        [Test]
        public void AddHydraulicConditionChangesClassEstimatesWithExperts()
        {
            var treeEvent = new TreeEvent("");
            var eventTree = new EventTree()
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
            var project = new ForestAnalysis
            {
                EventTree = { MainTreeEvent = treeEvent },
                ProbabilityEstimations =
                {
                    probabilityEstimation
                }
            };
            var projectManipulationService = new AnalysisManipulationService(project);

            Assert.AreEqual(0, firstEstimation.ClassProbabilitySpecification.Count);

            var hydraulicCondition = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            projectManipulationService.AddHydraulicCondition(hydraulicCondition);

            // TODO: Should be added to the probability estimation, not the project.
            Assert.AreEqual(1, project.HydrodynamicConditions.Count);

            Assert.AreEqual(hydraulicCondition, project.HydrodynamicConditions.First());
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
                Estimations =
                {
                    treeEventProbabilityEstimation
                }
            };
            var project = new ForestAnalysis
            {
                EventTree = eventTree,
                HydrodynamicConditions =
                {
                    hydraulicConditionToRemove,
                    hydraulicCondition2
                },
                ProbabilityEstimations = { estimation }
            };
            var projectManipulationService = new AnalysisManipulationService(project);

            Assert.AreEqual(4, treeEventProbabilityEstimation.ClassProbabilitySpecification.Count);

            projectManipulationService.RemoveHydraulicCondition(hydraulicConditionToRemove);

            // TODO: Move hydrodynamic conditions to estimation.
            Assert.AreEqual(1, project.HydrodynamicConditions.Count);
            Assert.AreEqual(hydraulicCondition2, project.HydrodynamicConditions.First());
            Assert.AreEqual(2, treeEventProbabilityEstimation.ClassProbabilitySpecification.Count);

            var firstSpecification = treeEventProbabilityEstimation.ClassProbabilitySpecification.First();
            Assert.AreEqual(hydraulicCondition2, firstSpecification.HydrodynamicCondition);
            Assert.AreEqual(expert1, firstSpecification.Expert);
            Assert.Contains(firstSpecification.Expert, project.Experts);

            var secondSpecification = treeEventProbabilityEstimation.ClassProbabilitySpecification.Last();
            Assert.AreEqual(hydraulicCondition2, secondSpecification.HydrodynamicCondition);
            Assert.AreEqual(expert2, secondSpecification.Expert);
            Assert.Contains(secondSpecification.Expert, project.Experts);

            Assert.AreNotEqual(firstSpecification.Expert, secondSpecification.Expert);
        }

        [Test]
        public void RemoveHydraulicConditionDoesNotChangeClassEstimatesNoExperts()
        {
            var treeEvent = new TreeEvent("");
            var hydraulicConditionToRemove = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            var hydraulicCondition2 = new HydrodynamicCondition(2.0, (Probability)0.001, 1, 1);
            var project = new ForestAnalysis
            {
                EventTree =
                    { MainTreeEvent = treeEvent },
                HydrodynamicConditions =
                {
                    hydraulicConditionToRemove,
                    hydraulicCondition2
                }
            };
            var projectManipulationService = new AnalysisManipulationService(project);

            projectManipulationService.RemoveHydraulicCondition(hydraulicConditionToRemove);

            Assert.AreEqual(1, project.HydrodynamicConditions.Count);
            Assert.AreEqual(hydraulicCondition2, project.HydrodynamicConditions.First());
        }
    }
}