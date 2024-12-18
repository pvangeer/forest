using System;
using System.Globalization;
using System.Windows.Data;
using Forest.Visualization.ViewModels.StatusBar;

namespace Forest.Visualization.Converters.Taskbar
{
    public class MessageListToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is MessageListViewModel viewModel))
                return value;

            return $"{viewModel.MessageList.Count} Berichten";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}