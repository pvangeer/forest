using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Forest.Gui.Components;
using Forest.Gui.ViewModels;

namespace Forest.Gui.Converters
{
    public class SelectedProcessToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is ForestGuiState buttonProcess) || !(value is ForestGuiState selectedProcess))
                return DependencyProperty.UnsetValue;
            return buttonProcess == selectedProcess;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is ForestGuiState buttonProcess) || !(value is bool isSelectedProcess))
                return DependencyProperty.UnsetValue;

            return isSelectedProcess ? buttonProcess : DependencyProperty.UnsetValue;
        }
    }
}