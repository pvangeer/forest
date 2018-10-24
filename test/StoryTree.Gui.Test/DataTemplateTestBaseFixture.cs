using System;
using System.Collections.Generic;
using System.Windows;
using NUnit.Framework;

namespace StoryTree.Gui.Test
{
    [TestFixture]
    public abstract class DataTemplateTestBaseFixture
    {
        /*protected ResourceDictionary Resources = new ResourceDictionary();

        [SetUp]
        public void SetUpFixture()
        {
            AddToResources("App.xaml");
        }

        protected void AddToResources(string fileLocation)
        {
            var resourceFileLocation = $"pack://application:,,,/StoryTree.Gui;component/{fileLocation}";
            var dictionary = new ResourceDictionary
            {
                Source = new Uri(resourceFileLocation, UriKind.RelativeOrAbsolute)
            };
            Resources.MergedDictionaries.Add(dictionary);
        }*/
    }
}
