using System.Windows;
using System.Windows.Controls;
using Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.EventTree;
using Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent;

namespace Forest.Visualization.DataTemplates.MainContentPresenter
{
    public class MainContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EventTreeDataTemplate { get; set; }
        public DataTemplate ProbabilityPerTreeEventDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case EventTreeMainContentViewModel _:
                    return EventTreeDataTemplate;
                case ProbabilityPerTreeEventMainContentViewModel _:
                    return ProbabilityPerTreeEventDataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}