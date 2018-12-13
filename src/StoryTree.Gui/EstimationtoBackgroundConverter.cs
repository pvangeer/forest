using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui
{
    public class EstimationToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ExpertClassEstimationViewModel viewModel))
            {
                return new SolidColorBrush(Colors.Transparent);
            }

            if (viewModel.MinEstimation > viewModel.AverageEstimation ||
                viewModel.MaxEstimation < viewModel.AverageEstimation ||
                viewModel.MinEstimation > viewModel.MaxEstimation)
            {
                return new SolidColorBrush(Colors.IndianRed);
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}