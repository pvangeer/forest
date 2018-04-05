using System;
using System.Collections.Generic;
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
            var project = TestDataGenerator.GenerateAsphalProject();

            var storeProject = new StorageSqLite();
            storeProject.StageProject(project);
            storeProject.SaveProjectAs(@"D:\Test\First project.sqlite");

            var project2c = storeProject.LoadProject(@"D:\Test\First project.sqlite");
        }
    }
}
