using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace StoryTree.Gui.Converters
{
    public class EventTreesToCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ICollection collection))
            {
                return value;
            }

            return collection.Count.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}