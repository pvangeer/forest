using System.Windows;
using System.Windows.Controls;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.DataTemplates
{
    public class ContentTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is ContentPresenter contentPresenter)
            {
                switch (item)
                {
                    case TreeEventViewModel _:
                        return contentPresenter.Resources["TreeEventPropertiesTemplate"] as DataTemplate;
                    case EventTreeViewModel _:
                        return contentPresenter.Resources["EventTreePropertiesTemplate"] as DataTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}