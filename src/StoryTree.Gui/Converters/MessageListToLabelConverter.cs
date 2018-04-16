using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace StoryTree.Gui
{
    public class MessageListToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ICollection collection))
            {
                return value;
            }

            return $"{collection.Count} Berichten";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}