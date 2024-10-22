using NUnit.Framework;

namespace Forest.Gui.Test
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
            var resourceFileLocation = $"pack://application:,,,/Forest.Gui;component/{fileLocation}";
            var dictionary = new ResourceDictionary
            {
                Source = new Uri(resourceFileLocation, UriKind.RelativeOrAbsolute)
            };
            Resources.MergedDictionaries.Add(dictionary);
        }*/
    }
}