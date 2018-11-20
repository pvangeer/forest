using System.Linq;
using NUnit.Framework;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Services;
using StoryTree.Data.Tree;

namespace StoryTree.Data.Test
{
    [TestFixture]
    public class ProjectManipulationServiceTest
    {
        [Test]
        public void AddExpertDoesNotChangesClassEstimatesNoHydraulicConditions()
        {
            var treeEvent = new TreeEvent();
            var project = new Project
            {
                EventTrees = { new EventTree
                {
                    MainTreeEvent = treeEvent
                }}
            };
            var projectManipulationService = new ProjectManipulationService(project);
            Assert.AreEqual(0,treeEvent.ClassesProbabilitySpecification.Count);

            var expert = new Expert();
            projectManipulationService.AddExpert(expert);

            Assert.AreEqual(1, project.Experts.Count);
            Assert.AreEqual(expert, project.Experts.First());
            Assert.AreEqual(0,treeEvent.ClassesProbabilitySpecification.Count);
        }

        [Test]
        public void AddExpertChangesClassEstimatesWithHydraulicConditions()
        {
            var treeEvent = new TreeEvent();
            var project = new Project
            {
                EventTrees = { new EventTree
                {
                    MainTreeEvent = treeEvent
                }},
                HydraulicConditions =
                {
                    new HydraulicCondition(1.0,(Probability)0.01,1,1),
                    new HydraulicCondition(2.0,(Probability)0.001,1,1),
                }
            };
            var projectManipulationService = new ProjectManipulationService(project);

            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            var expert = new Expert();
            projectManipulationService.AddExpert(expert);

            Assert.AreEqual(1, project.Experts.Count);
            Assert.AreEqual(expert, project.Experts.First());
            Assert.AreEqual(2, treeEvent.ClassesProbabilitySpecification.Count);
            var firstSpecification = treeEvent.ClassesProbabilitySpecification.First();
            Assert.AreEqual(expert,firstSpecification.Expert);
            Assert.Contains(firstSpecification.HydraulicCondition, project.HydraulicConditions);

            var secondSpecification = treeEvent.ClassesProbabilitySpecification.Last();
            Assert.AreEqual(expert, secondSpecification.Expert);
            Assert.Contains(secondSpecification.HydraulicCondition, project.HydraulicConditions);

            Assert.AreNotEqual(firstSpecification.HydraulicCondition, secondSpecification.HydraulicCondition);
        }

        [Test]
        public void RemoveExpertChangesClassEstimatesWithHydraulicConditions()
        {
            var expertToRemove = new Expert();
            var otherExpert = new Expert();
            var hydraulicCondition1 = new HydraulicCondition(1.0, (Probability) 0.01, 1, 1);
            var hydraulicCondition2 = new HydraulicCondition(2.0, (Probability) 0.001, 1, 1);
            var treeEvent = new TreeEvent
            {
                ClassesProbabilitySpecification =
                {
                    new ExpertClassEstimation{Expert = expertToRemove, HydraulicCondition = hydraulicCondition1},
                    new ExpertClassEstimation{Expert = expertToRemove, HydraulicCondition = hydraulicCondition2},
                    new ExpertClassEstimation{Expert = otherExpert, HydraulicCondition = hydraulicCondition1},
                    new ExpertClassEstimation{Expert = otherExpert, HydraulicCondition = hydraulicCondition2}
                }
            };
            var project = new Project
            {
                EventTrees =
                {
                    new EventTree
                    {
                        MainTreeEvent = treeEvent
                    }
                },
                Experts =
                {
                    expertToRemove,
                    otherExpert
                },
                HydraulicConditions =
                {
                    hydraulicCondition1,
                    hydraulicCondition2,
                }
            };
            var projectManipulationService = new ProjectManipulationService(project);

            Assert.AreEqual(4, treeEvent.ClassesProbabilitySpecification.Count);

            projectManipulationService.RemoveExpert(expertToRemove);

            Assert.AreEqual(1, project.Experts.Count);
            Assert.AreEqual(otherExpert, project.Experts.First());
            Assert.AreEqual(2, treeEvent.ClassesProbabilitySpecification.Count);

            var firstSpecification = treeEvent.ClassesProbabilitySpecification.First();
            Assert.AreEqual(otherExpert, firstSpecification.Expert);
            Assert.AreEqual(hydraulicCondition1, firstSpecification.HydraulicCondition);
            Assert.Contains(firstSpecification.HydraulicCondition, project.HydraulicConditions);

            var secondSpecification = treeEvent.ClassesProbabilitySpecification.Last();
            Assert.AreEqual(otherExpert, secondSpecification.Expert);
            Assert.AreEqual(hydraulicCondition2, secondSpecification.HydraulicCondition);
            Assert.Contains(secondSpecification.HydraulicCondition, project.HydraulicConditions);

            Assert.AreNotEqual(firstSpecification.HydraulicCondition, secondSpecification.HydraulicCondition);
        }

        [Test]
        public void RemoveExpertDoesNotChangeClassEstimatesNoHydraulicConditions()
        {
            var treeEvent = new TreeEvent();
            var expertToRemove = new Expert();
            var otherExpert = new Expert();
            var project = new Project
            {
                EventTrees =
                {
                    new EventTree
                    {
                        MainTreeEvent = treeEvent
                    }
                },
                Experts =
                {
                    expertToRemove,
                    otherExpert
                }
            };
            var projectManipulationService = new ProjectManipulationService(project);

            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            projectManipulationService.RemoveExpert(expertToRemove);

            Assert.AreEqual(1,project.Experts.Count);
            Assert.AreEqual(otherExpert,project.Experts.First());
            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);
        }

        [Test]
        public void AddHydraulicConditionDoesNotChangesClassEstimatesNoExperts()
        {
            var treeEvent = new TreeEvent();
            var project = new Project
            {
                EventTrees = { new EventTree
                {
                    MainTreeEvent = treeEvent
                }}
            };
            var projectManipulationService = new ProjectManipulationService(project);

            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            var hydraulicCondition = new HydraulicCondition(1.0,(Probability)0.01,1,1);
            projectManipulationService.AddHydraulicCondition(hydraulicCondition);

            Assert.AreEqual(1, project.HydraulicConditions.Count);
            Assert.AreEqual(hydraulicCondition, project.HydraulicConditions.First());
            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);
        }

        [Test]
        public void AddHydraulicConditionChangesClassEstimatesWithExperts()
        {
            var treeEvent = new TreeEvent();
            var project = new Project
            {
                EventTrees = { new EventTree
                {
                    MainTreeEvent = treeEvent
                }},
                Experts =
                {
                    new Expert(),
                    new Expert()
                }
            };
            var projectManipulationService = new ProjectManipulationService(project);

            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            var hydraulicCondition = new HydraulicCondition(1.0,(Probability)0.01,1,1);
            projectManipulationService.AddHydraulicCondition(hydraulicCondition);

            Assert.AreEqual(1, project.HydraulicConditions.Count);
            Assert.AreEqual(hydraulicCondition, project.HydraulicConditions.First());
            Assert.AreEqual(2, treeEvent.ClassesProbabilitySpecification.Count);
            var firstSpecification = treeEvent.ClassesProbabilitySpecification.First();
            Assert.AreEqual(hydraulicCondition, firstSpecification.HydraulicCondition);
            Assert.Contains(firstSpecification.Expert, project.Experts);

            var secondSpecification = treeEvent.ClassesProbabilitySpecification.Last();
            Assert.AreEqual(hydraulicCondition, secondSpecification.HydraulicCondition);
            Assert.Contains(secondSpecification.Expert, project.Experts);

            Assert.AreNotEqual(firstSpecification.Expert, secondSpecification.Expert);
        }

        [Test]
        public void RemoveHydraulicConditionChangesClassEstimatesWithExperts()
        {
            var expert1 = new Expert();
            var expert2 = new Expert();
            var hydraulicConditionToRemove = new HydraulicCondition(1.0, (Probability)0.01, 1, 1);
            var hydraulicCondition2 = new HydraulicCondition(2.0, (Probability)0.001, 1, 1);
            var treeEvent = new TreeEvent
            {
                ClassesProbabilitySpecification =
                {
                    new ExpertClassEstimation{Expert = expert1, HydraulicCondition = hydraulicConditionToRemove},
                    new ExpertClassEstimation{Expert = expert1, HydraulicCondition = hydraulicCondition2},
                    new ExpertClassEstimation{Expert = expert2, HydraulicCondition = hydraulicConditionToRemove},
                    new ExpertClassEstimation{Expert = expert2, HydraulicCondition = hydraulicCondition2}
                }
            };
            var project = new Project
            {
                EventTrees =
                {
                    new EventTree
                    {
                        MainTreeEvent = treeEvent
                    }
                },
                Experts =
                {
                    expert1,
                    expert2
                },
                HydraulicConditions =
                {
                    hydraulicConditionToRemove,
                    hydraulicCondition2,
                }
            };
            var projectManipulationService = new ProjectManipulationService(project);

            Assert.AreEqual(4, treeEvent.ClassesProbabilitySpecification.Count);

            projectManipulationService.RemoveHydraulicCondition(hydraulicConditionToRemove);

            Assert.AreEqual(1, project.HydraulicConditions.Count);
            Assert.AreEqual(hydraulicCondition2, project.HydraulicConditions.First());
            Assert.AreEqual(2, treeEvent.ClassesProbabilitySpecification.Count);

            var firstSpecification = treeEvent.ClassesProbabilitySpecification.First();
            Assert.AreEqual(hydraulicCondition2, firstSpecification.HydraulicCondition);
            Assert.AreEqual(expert1, firstSpecification.Expert);
            Assert.Contains(firstSpecification.Expert, project.Experts);

            var secondSpecification = treeEvent.ClassesProbabilitySpecification.Last();
            Assert.AreEqual(hydraulicCondition2, secondSpecification.HydraulicCondition);
            Assert.AreEqual(expert2, secondSpecification.Expert);
            Assert.Contains(secondSpecification.Expert, project.Experts);

            Assert.AreNotEqual(firstSpecification.Expert, secondSpecification.Expert);
        }

        [Test]
        public void RemoveHydraulicConditionDoesNotChangeClassEstimatesNoExperts()
        {
            var treeEvent = new TreeEvent();
            var hydraulicConditionToRemove = new HydraulicCondition(1.0, (Probability)0.01, 1, 1);
            var hydraulicCondition2 = new HydraulicCondition(2.0, (Probability)0.001, 1, 1);
            var project = new Project
            {
                EventTrees =
                {
                    new EventTree
                    {
                        MainTreeEvent = treeEvent
                    }
                },
                HydraulicConditions =
                {
                    hydraulicConditionToRemove,
                    hydraulicCondition2,
                }
            };
            var projectManipulationService = new ProjectManipulationService(project);

            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);

            projectManipulationService.RemoveHydraulicCondition(hydraulicConditionToRemove);

            Assert.AreEqual(1, project.HydraulicConditions.Count);
            Assert.AreEqual(hydraulicCondition2, project.HydraulicConditions.First());
            Assert.AreEqual(0, treeEvent.ClassesProbabilitySpecification.Count);
        }
    }
}
