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
            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            var expert = new Expert();
            projectManipulationService.AddExpert(expert);

            Assert.AreEqual(1, project.Experts.Count);
            Assert.AreEqual(expert, project.Experts.First());
            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);
        }

        [Test]
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
        }

        [Test]
        public void RemoveExpertChangesClassEstimatesWithHydraulicConditions()
        {
            var expertToRemove = new Expert();
            var otherExpert = new Expert();
            var hydraulicCondition1 = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            var hydraulicCondition2 = new HydrodynamicCondition(2.0, (Probability)0.001, 1, 1);
            var treeEvent = new TreeEvent("")
            {
                ClassesProbabilitySpecification =
                {
                    new ExpertClassEstimation { Expert = expertToRemove, HydrodynamicCondition = hydraulicCondition1 },
                    new ExpertClassEstimation { Expert = expertToRemove, HydrodynamicCondition = hydraulicCondition2 },
                    new ExpertClassEstimation { Expert = otherExpert, HydrodynamicCondition = hydraulicCondition1 },
                    new ExpertClassEstimation { Expert = otherExpert, HydrodynamicCondition = hydraulicCondition2 }
                }
            };
            var project = new ForestAnalysis
            {
                EventTree =
                    { MainTreeEvent = treeEvent },
                Experts =
                {
                    expertToRemove,
                    otherExpert
                },
                HydrodynamicConditions =
                {
                    hydraulicCondition1,
                    hydraulicCondition2
                }
            };
            var projectManipulationService = new AnalysisManipulationService(project);

            Assert.AreEqual(4, treeEvent.ClassesProbabilitySpecification.Count);

            projectManipulationService.RemoveExpert(expertToRemove);

            Assert.AreEqual(1, project.Experts.Count);
            Assert.AreEqual(otherExpert, project.Experts.First());
            Assert.AreEqual(2, treeEvent.ClassesProbabilitySpecification.Count);

            var firstSpecification = treeEvent.ClassesProbabilitySpecification.First();
            Assert.AreEqual(otherExpert, firstSpecification.Expert);
            Assert.AreEqual(hydraulicCondition1, firstSpecification.HydrodynamicCondition);
            Assert.Contains(firstSpecification.HydrodynamicCondition, project.HydrodynamicConditions);

            var secondSpecification = treeEvent.ClassesProbabilitySpecification.Last();
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
            var project = new ForestAnalysis
            {
                EventTree =
                    { MainTreeEvent = treeEvent },
                Experts =
                {
                    expertToRemove,
                    otherExpert
                }
            };
            var projectManipulationService = new AnalysisManipulationService(project);

            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            projectManipulationService.RemoveExpert(expertToRemove);

            Assert.AreEqual(1, project.Experts.Count);
            Assert.AreEqual(otherExpert, project.Experts.First());
            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);
        }

        [Test]
        public void AddHydraulicConditionDoesNotChangesClassEstimatesNoExperts()
        {
            var treeEvent = new TreeEvent("");
            var project = new ForestAnalysis
            {
                EventTree = { MainTreeEvent = treeEvent }
            };
            var projectManipulationService = new AnalysisManipulationService(project);

            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            var hydraulicCondition = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            projectManipulationService.AddHydraulicCondition(hydraulicCondition);

            Assert.AreEqual(1, project.HydrodynamicConditions.Count);
            Assert.AreEqual(hydraulicCondition, project.HydrodynamicConditions.First());
            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);
        }

        [Test]
        public void AddHydraulicConditionChangesClassEstimatesWithExperts()
        {
            var treeEvent = new TreeEvent("");
            var project = new ForestAnalysis
            {
                EventTree = { MainTreeEvent = treeEvent },
                Experts =
                {
                    new Expert(),
                    new Expert()
                }
            };
            var projectManipulationService = new AnalysisManipulationService(project);

            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            var hydraulicCondition = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            projectManipulationService.AddHydraulicCondition(hydraulicCondition);

            Assert.AreEqual(1, project.HydrodynamicConditions.Count);
            Assert.AreEqual(hydraulicCondition, project.HydrodynamicConditions.First());
            Assert.AreEqual(2, treeEvent.ClassesProbabilitySpecification.Count);
            var firstSpecification = treeEvent.ClassesProbabilitySpecification.First();
            Assert.AreEqual(hydraulicCondition, firstSpecification.HydrodynamicCondition);
            Assert.Contains(firstSpecification.Expert, project.Experts);

            var secondSpecification = treeEvent.ClassesProbabilitySpecification.Last();
            Assert.AreEqual(hydraulicCondition, secondSpecification.HydrodynamicCondition);
            Assert.Contains(secondSpecification.Expert, project.Experts);

            Assert.AreNotEqual(firstSpecification.Expert, secondSpecification.Expert);
        }

        [Test]
        public void RemoveHydraulicConditionChangesClassEstimatesWithExperts()
        {
            var expert1 = new Expert();
            var expert2 = new Expert();
            var hydraulicConditionToRemove = new HydrodynamicCondition(1.0, (Probability)0.01, 1, 1);
            var hydraulicCondition2 = new HydrodynamicCondition(2.0, (Probability)0.001, 1, 1);
            var treeEvent = new TreeEvent("")
            {
                ClassesProbabilitySpecification =
                {
                    new ExpertClassEstimation { Expert = expert1, HydrodynamicCondition = hydraulicConditionToRemove },
                    new ExpertClassEstimation { Expert = expert1, HydrodynamicCondition = hydraulicCondition2 },
                    new ExpertClassEstimation { Expert = expert2, HydrodynamicCondition = hydraulicConditionToRemove },
                    new ExpertClassEstimation { Expert = expert2, HydrodynamicCondition = hydraulicCondition2 }
                }
            };
            var project = new ForestAnalysis
            {
                EventTree =
                    { MainTreeEvent = treeEvent },
                Experts =
                {
                    expert1,
                    expert2
                },
                HydrodynamicConditions =
                {
                    hydraulicConditionToRemove,
                    hydraulicCondition2
                }
            };
            var projectManipulationService = new AnalysisManipulationService(project);

            Assert.AreEqual(4, treeEvent.ClassesProbabilitySpecification.Count);

            projectManipulationService.RemoveHydraulicCondition(hydraulicConditionToRemove);

            Assert.AreEqual(1, project.HydrodynamicConditions.Count);
            Assert.AreEqual(hydraulicCondition2, project.HydrodynamicConditions.First());
            Assert.AreEqual(2, treeEvent.ClassesProbabilitySpecification.Count);

            var firstSpecification = treeEvent.ClassesProbabilitySpecification.First();
            Assert.AreEqual(hydraulicCondition2, firstSpecification.HydrodynamicCondition);
            Assert.AreEqual(expert1, firstSpecification.Expert);
            Assert.Contains(firstSpecification.Expert, project.Experts);

            var secondSpecification = treeEvent.ClassesProbabilitySpecification.Last();
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

            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            projectManipulationService.RemoveHydraulicCondition(hydraulicConditionToRemove);

            Assert.AreEqual(1, project.HydrodynamicConditions.Count);
            Assert.AreEqual(hydraulicCondition2, project.HydrodynamicConditions.First());
            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);
        }
    }
}