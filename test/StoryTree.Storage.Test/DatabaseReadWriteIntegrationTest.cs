using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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

            var project = TestDataGenerator.GenerateAsphalProject();

            var storeProject = new StorageSqLite();
            storeProject.StageProject(project);
            storeProject.SaveProjectAs(testProjectName);

            var project2c = storeProject.LoadProject(testProjectName);
        }
    }
}
