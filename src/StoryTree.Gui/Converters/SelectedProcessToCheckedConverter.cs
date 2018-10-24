using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Converters
{
    public class SelectedProcessToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is StoryTreeProcess buttonProcess) || !(value is StoryTreeProcess selectedProcess))
            {
                return DependencyProperty.UnsetValue;
            }
            return buttonProcess == selectedProcess;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is StoryTreeProcess buttonProcess) || !(value is bool isSelectedProcess))
            {
                return DependencyProperty.UnsetValue;
            }

            return isSelectedProcess ? buttonProcess : DependencyProperty.UnsetValue;
        }
    }
}