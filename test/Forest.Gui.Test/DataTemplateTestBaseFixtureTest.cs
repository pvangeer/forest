using System;
using System.Windows;
using NUnit.Framework;

namespace Forest.Gui.Test
{
    [TestFixture]
    public class DataTemplateTestBaseFixtureTest
    {
        [Test]
        [Explicit]
        public void ThisIsATest()
        {
            //Assert.Greater(0,Resources.Count);
            //var resourceFileLocation = $"pack://application:,,,/Forest.Gui;component/{"DataTemplates/General/AssessmentSections.xaml"}";
            var resourceFileLocation = "pack://application:,,,/Forest.Gui;component/DataTemplates/General/TabControlFlatStyle.xaml";
            var dictionary = new ResourceDictionary
            {
                Source = new Uri(resourceFileLocation, UriKind.RelativeOrAbsolute)
            };
        }
    }
}