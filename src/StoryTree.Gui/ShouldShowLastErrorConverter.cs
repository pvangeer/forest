using System;
using System.Globalization;
using System.Windows.Data;
using StoryTree.Messaging;

namespace StoryTree.Gui
{
    public class ShouldShowLastErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is LogMessage message && message.Severity == MessageSeverity.Error;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}