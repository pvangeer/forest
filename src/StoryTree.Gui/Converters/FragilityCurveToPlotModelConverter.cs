using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Converters
{
    public class FragilityCurveToPlotModelConverter : IValueConverter
    {
        private NotifyCollectionChangedEventHandler conditionCollectionChangedHandler;
        private PropertyChangedEventHandler hydraulicConditionPropertyChangedHandler;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ObservableCollection<FragilityCurveElementViewModel> conditions))
            {
                return null;
            }

            var plotModel = new PlotModel();
            plotModel.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Bottom
            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left
            });

            plotModel.Series.Add(new LineSeries
            {
                ItemsSource = conditions,
                Color = OxyColors.CornflowerBlue,
                MarkerSize = 0,
                StrokeThickness = 2,
                DataFieldX = nameof(FragilityCurveElementViewModel.ProbabilityDouble),
                DataFieldY = nameof(FragilityCurveElementViewModel.WaterLevel)
            });

            conditionCollectionChangedHandler = (o,e) => ConditionsCollectionChanged(conditions, plotModel);
            hydraulicConditionPropertyChangedHandler = (o, e) => HydraulicConditionPropertyChanged(plotModel);

            conditions.CollectionChanged += conditionCollectionChangedHandler;
            foreach (var condition in conditions)
            {
                condition.PropertyChanged += hydraulicConditionPropertyChangedHandler;
            }

            return plotModel;
        }

        private void HydraulicConditionPropertyChanged(PlotModel plotModel)
        {
            plotModel.InvalidatePlot(true);
        }

        private void ConditionsCollectionChanged(ObservableCollection<FragilityCurveElementViewModel> conditions, PlotModel plotModel)
        {
            foreach (var hydraulicConditionViewModel in conditions)
            {
                hydraulicConditionViewModel.PropertyChanged -= hydraulicConditionPropertyChangedHandler;
                hydraulicConditionViewModel.PropertyChanged += hydraulicConditionPropertyChangedHandler;
            }
            plotModel.InvalidatePlot(true);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}