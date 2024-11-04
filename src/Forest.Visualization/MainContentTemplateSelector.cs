using System.Windows;
using System.Windows.Controls;
using Forest.Visualization.ViewModels.MainContentPanel;

namespace Forest.Visualization
{
    public class MainContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EventTreeDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case EventTreeMainContentViewModel eventTreeViewModel:
                    return EventTreeDataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}