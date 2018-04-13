using System.Windows;
using System.Windows.Controls;
using Fluent;

namespace StoryTree.Gui
{
    public class BusyIconTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(container is ContentPresenter contentPresenter) ||
                !(contentPresenter.TemplatedParent is StatusBarItem statusBarItem) ||
                !(item is StorageState busy))
            {
                return base.SelectTemplate(item, container);
            }

            return busy == StorageState.Busy
                ? statusBarItem.Resources["BusyIconTemplate"] as DataTemplate
                : statusBarItem.Resources["IdleIconTemplate"] as DataTemplate;
        }
    }
}