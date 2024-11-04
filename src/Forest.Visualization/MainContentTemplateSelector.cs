using System.Text;
using System.Windows;
using System.Windows.Controls;
using Forest.Data.Tree;
using Forest.Visualization.ViewModels.MainContentPanel;

namespace Forest.Visualization
{
    public class MainContentTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case EventTreeMainContentViewModel eventTreeViewModel:
                    return EventTreeDataTemplate;
            }
            return base.SelectTemplate(item, container);
        }

        public DataTemplate EventTreeDataTemplate { get; set; }
    }
}