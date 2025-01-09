using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Forest.Calculators;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Visualization.ViewModels;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Forest.Visualization.Converters
{
    public class CriticalPathToPlotModelConverter : CriticalPathConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (ExtractInput(values, out var hydraulics, out var estimations, out var pathElements, out var criticalPath))
                return values;

            var plotModel = new PlotModel();
            plotModel.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Bottom
            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left
            });

            for (var i = 0; i < pathElements.Length; i++)
            {
                var curve = new FragilityCurveViewModel(
                    ClassEstimationFragilityCurveCalculator.CalculateCombinedFragilityCurve(hydraulics,
                        pathElements.Take(i + 1).ToArray()));

                plotModel.Series.Add(new LineSeries
                {
                    ItemsSource = curve.CurveElements,
                    MarkerSize = 0,
                    StrokeThickness = 2,
                    DataFieldX = nameof(FragilityCurveElementViewModel.ProbabilityDouble),
                    DataFieldY = nameof(FragilityCurveElementViewModel.WaterLevel),
                    Title = criticalPath[i].Name
                });
            }

            return plotModel;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PolygonData
    {
        public PolygonData(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
        }

        public double X1 { get; }

        public double X2 { get; }

        public double Y1 { get; }

        public double Y2 { get; }
    }
}