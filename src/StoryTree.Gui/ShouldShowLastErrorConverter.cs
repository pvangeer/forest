using System;
using System.Globalization;
using System.Windows.Data;
using StoryTree.Gui.Messaging;

namespace StoryTree.Gui
{
    public class ShouldShowLastErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is StoryTreeMessage message && message.Severity == MessageSeverity.Error;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}