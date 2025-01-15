using System;
using System.Windows;
using System.Windows.Controls;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Visualization.ViewModels;
using Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent;

namespace Forest.Visualization.DataTemplates.MainContentPresenter.ProbabilityEstimationPerTreeEvent
{
    public class EstimationContentTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(container is ContentPresenter contentPresenter))
                return base.SelectTemplate(item, container);

            if (!(contentPresenter.Content is ProbabilitySpecificationViewModelBase probabilitySpecification))
                // TODO: Return default template
                return null;

            switch (probabilitySpecification.Type)
            {
                case ProbabilitySpecificationType.FixedValue:
                    return contentPresenter.Resources["FixedValueEstimationTemplate"] as DataTemplate;
                case ProbabilitySpecificationType.FixedFrequency:
                    return contentPresenter.Resources["FragilityCurveEstimationTemplate"] as DataTemplate;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}