using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using NUnit.Framework;
using StoryTree.Data;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Tree;
using StoryTree.Gui;

namespace StoryTree.Storage.Test
{
    [TestFixture]
    public class DatabaseReadWriteIntegrationTest
    {
        [Test]
        public void WriteAndReadProject()
        {
            var testProjectName = @"D:\Test\FirstProject.sqlite";

            if (File.Exists(testProjectName))
            {
                try
                {
                    File.Delete(testProjectName);
                }
                catch (Exception e)
                {
                    Assert.Fail("Unable to remove previous version of target file.");
                }
            }

            var project = TestDataGenerator.GenerateAsphaltProject();
            var mainTreeEvent = project.EventTree.MainTreeEvent;
            foreach (var waterLevel in project.HydraulicConditions.Select(hc => hc.WaterLevel).Distinct().OrderBy(w => w))
            {
                mainTreeEvent.FixedFragilityCurve.Add(new FragilityCurveElement(waterLevel,(Probability)0.5));
            }
            var storeProject = new StorageSqLite();
            storeProject.StageProject(project);
            storeProject.SaveProjectAs(testProjectName);

            var retrievedProject = storeProject.LoadProject(testProjectName);

            AssertEqualProjects(project, retrievedProject);
        }

        private void AssertEqualProjects(Project project, Project project2C)
        {
            Assert.AreEqual(project.Name,project2C.Name);
            Assert.AreEqual(project.Description, project2C.Description);
            Assert.AreEqual(project.AssessmentSection, project2C.AssessmentSection);
            Assert.AreEqual(project.ProjectInformation, project2C.ProjectInformation);

            AssertEqualProjectLeader(project.ProjectLeader, project2C.ProjectLeader);
            AssertEqualExperts(project.Experts, project2C.Experts);
            AssertEqualHydraulicConditions(project.HydraulicConditions, project2C.HydraulicConditions);
            AssertEqualEventTree(project.EventTree, project2C.EventTree);
        }

        private void AssertEqualEventTree(EventTree projectEventTree, EventTree project2CEventTrees)
        {
                var tree1 = projectEventTree;
                var tree2 = project2CEventTrees;
                Assert.AreEqual(tree1.Name, tree2.Name);
                Assert.AreEqual(tree1.Summary, tree2.Summary);
                Assert.AreEqual(tree1.Details, tree2.Details);
                Assert.AreEqual(tree1.Color, tree2.Color);
                Assert.AreEqual(tree1.NeedsSpecification, tree2.NeedsSpecification);
                AssertEqualTreeEvents(tree1.MainTreeEvent, tree2.MainTreeEvent);
        }

        private void AssertEqualTreeEvents(TreeEvent treeEvent1, TreeEvent treeEvent2)
        {
            Assert.AreEqual(treeEvent1 == null, treeEvent2 == null);
            if (treeEvent1 == null)
            {
                return;
            }
            Assert.AreEqual(treeEvent1.Name, treeEvent2.Name);
            Assert.AreEqual(treeEvent1.ProbabilitySpecificationType, treeEvent2.ProbabilitySpecificationType);
            Assert.AreEqual(treeEvent1.FixedProbability, treeEvent2.FixedProbability);
            Assert.AreEqual(treeEvent1.Summary, treeEvent2.Summary);
            Assert.AreEqual(treeEvent1.Information, treeEvent2.Information);
            Assert.AreEqual(treeEvent1.Discussion, treeEvent2.Discussion);

            AssertEqualTreeEvents(treeEvent1.PassingEvent, treeEvent2.PassingEvent);
            AssertEqualTreeEvents(treeEvent1.FailingEvent, treeEvent2.FailingEvent);

            Assert.AreEqual(treeEvent1.ClassesProbabilitySpecification.Count, treeEvent2.ClassesProbabilitySpecification.Count);
            for (int i = 0; i < treeEvent1.ClassesProbabilitySpecification.Count; i++)
            {
                var spec1 = treeEvent1.ClassesProbabilitySpecification[i];
                var spec2 = treeEvent2.ClassesProbabilitySpecification[i];
                Assert.AreEqual(spec1.HydraulicCondition.WaterLevel, spec2.HydraulicCondition.WaterLevel);
                Assert.AreEqual(spec1.HydraulicCondition.Probability, spec2.HydraulicCondition.Probability);
                Assert.AreEqual(spec1.HydraulicCondition.WaveHeight, spec2.HydraulicCondition.WaveHeight);
                Assert.AreEqual(spec1.HydraulicCondition.WavePeriod, spec2.HydraulicCondition.WavePeriod);
                AssertEqualExperts(spec1.Expert, spec2.Expert);
                Assert.AreEqual(spec1.AverageEstimation, spec2.AverageEstimation);
                Assert.AreEqual(spec1.MinEstimation, spec2.MinEstimation);
                Assert.AreEqual(spec1.MaxEstimation, spec2.MaxEstimation);
            }
            
            Assert.AreEqual(treeEvent1.FixedFragilityCurve.Count, treeEvent2.FixedFragilityCurve.Count);
            for (int i = 0; i < treeEvent1.FixedFragilityCurve.Count; i++)
            {
                var element1 = treeEvent1.FixedFragilityCurve[i];
                var element2 = treeEvent2.FixedFragilityCurve[i];
                Assert.AreEqual(element1.WaterLevel, element2.WaterLevel);
                Assert.AreEqual(element1.Probability, element2.Probability);
            }
        }

        private void AssertEqualHydraulicConditions(ObservableCollection<HydraulicCondition> p1HydraulicConditions, ObservableCollection<HydraulicCondition> p2HydraulicConditions)
        {
            Assert.AreEqual(p1HydraulicConditions.Count, p2HydraulicConditions.Count);
            for (int i = 0; i < p2HydraulicConditions.Count; i++)
            {
                var condition1 = p1HydraulicConditions[i];
                var condition2 = p2HydraulicConditions[i];
                Assert.AreEqual(condition1.WaterLevel, condition2.WaterLevel);
                Assert.AreEqual(condition1.WaveHeight, condition2.WaveHeight);
                Assert.AreEqual(condition1.WavePeriod, condition2.WavePeriod);
                Assert.AreEqual(condition1.Probability, condition2.Probability);
            }
        }

        private void AssertEqualExperts(ObservableCollection<Expert> projectExperts, ObservableCollection<Expert> project2CExperts)
        {
            Assert.AreEqual(projectExperts.Count,project2CExperts.Count);
            for (int i = 0; i < projectExperts.Count; i++)
            {
                var ex1 = projectExperts[i];
                var ex2 = project2CExperts[i];
                AssertEqualExperts(ex1, ex2);
            }
        }

        private static void AssertEqualExperts(Expert ex1, Expert ex2)
        {
            Assert.AreEqual(ex1.Name, ex2.Name);
            Assert.AreEqual(ex1.Telephone, ex2.Telephone);
            Assert.AreEqual(ex1.Email, ex2.Email);
            Assert.AreEqual(ex1.Organisation, ex2.Organisation);
            Assert.AreEqual(ex1.Expertise, ex2.Expertise);
        }

        private void AssertEqualProjectLeader(Person projectProjectLeader, Person project2CProjectLeader)
        {
            Assert.AreEqual(projectProjectLeader == null,project2CProjectLeader == null);
            if (project2CProjectLeader != null)
            {
                Assert.AreEqual(projectProjectLeader.Name, project2CProjectLeader.Name);
                Assert.AreEqual(projectProjectLeader.Telephone, project2CProjectLeader.Telephone);
                Assert.AreEqual(projectProjectLeader.Email, project2CProjectLeader.Email);
            }
        }
    }
}
