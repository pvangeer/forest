using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using Forest.Visualization.ViewModels;
using Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Forest.Visualization.Converters
{
    public class FragilityCurveToPlotModelConverter : IValueConverter
    {
        private NotifyCollectionChangedEventHandler conditionCollectionChangedHandler;
        private PropertyChangedEventHandler fragilityCurveElementPropertyChangedHandler;

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ObservableCollection<FragilityCurveElementViewModel> conditions))
                return null;

            return CreatePlotModel(conditions);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected PlotModel CreatePlotModel<T>(ObservableCollection<T> fragilityCurveElementViewModels)
            where T : FragilityCurveElementViewModel
        {
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
                ItemsSource = fragilityCurveElementViewModels,
                Color = OxyColors.CornflowerBlue,
                MarkerSize = 0,
                StrokeThickness = 2,
                DataFieldX = nameof(FragilityCurveElementViewModel.ProbabilityDouble),
                DataFieldY = nameof(FragilityCurveElementViewModel.WaterLevel)
            });

            conditionCollectionChangedHandler =
                (o, e) => ConditionsCollectionChanged(fragilityCurveElementViewModels, plotModel);
            fragilityCurveElementPropertyChangedHandler = (o, e) => FragilityCurveElementPropertyChanged(plotModel);

            fragilityCurveElementViewModels.CollectionChanged += conditionCollectionChangedHandler;
            foreach (var element in fragilityCurveElementViewModels)
                element.PropertyChanged += fragilityCurveElementPropertyChangedHandler;

            return plotModel;
        }

        private void FragilityCurveElementPropertyChanged(PlotModel plotModel)
        {
            plotModel.InvalidatePlot(true);
        }

        private void ConditionsCollectionChanged<T>(ObservableCollection<T> elements, PlotModel plotModel)
            where T : FragilityCurveElementViewModel
        {
            foreach (var element in elements)
            {
                element.PropertyChanged -= fragilityCurveElementPropertyChangedHandler;
                element.PropertyChanged += fragilityCurveElementPropertyChangedHandler;
            }

            plotModel.InvalidatePlot(true);
        }
    }
}