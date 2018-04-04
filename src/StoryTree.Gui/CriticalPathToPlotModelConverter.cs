using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using StoryTree.Calculators;
using StoryTree.Data;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui
{
    public class CriticalPathToPlotModelConverter : CriticalPathConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (ExtractInput(values, out var hydraulics, out var curves, out var criticalPath))
            {
                return values;
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

            for (int i = 0; i < curves.Length; i++)
            {
                FragilityCurveViewModel curve = new FragilityCurveViewModel(
                    ClassEstimationFragilityCurveCalculator.CalculateCombinedFragilityCurve(hydraulics,
                        curves.Take(i+1).ToArray()));
                
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
}