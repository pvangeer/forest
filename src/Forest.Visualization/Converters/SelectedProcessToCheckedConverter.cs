using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Forest.Gui;

namespace Forest.Visualization.Converters
{
    public class SelectedProcessToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is ForestGuiState buttonGuiState) || !(value is ForestGuiState selectedState))
                return DependencyProperty.UnsetValue;
            return buttonGuiState == selectedState;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is ForestGuiState buttonGuiState) || !(value is bool isSelectedProcess))
                return DependencyProperty.UnsetValue;

            return isSelectedProcess ? buttonGuiState : DependencyProperty.UnsetValue;
        }
    }
}