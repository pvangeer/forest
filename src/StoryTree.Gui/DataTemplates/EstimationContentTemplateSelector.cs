using System;
using System.Windows;
using System.Windows.Controls;
using StoryTree.Data.Estimations;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.DataTemplates
{
    public class EstimationContentTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(container is ContentPresenter contentPresenter))
            {
                return base.SelectTemplate(item, container);
            }

            if (!(contentPresenter.Content is ProbabilitySpecificationViewModelBase probabilitySpecification))
            {
                // TODO: Return default template
                return null;
            }

            switch (probabilitySpecification.Type)
            {
                case ProbabilitySpecificationType.Classes:
                    return contentPresenter.Resources["ClassEstimationTemplate"] as DataTemplate;
                case ProbabilitySpecificationType.FixedValue:
                    return contentPresenter.Resources["FixedValueEstimationTemplate"] as DataTemplate;
                case ProbabilitySpecificationType.FixedFreqeuncy:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}