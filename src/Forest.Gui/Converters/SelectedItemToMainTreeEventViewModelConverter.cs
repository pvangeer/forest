using System;
using System.Globalization;
using System.Windows.Data;
using Forest.Gui.ViewModels;

namespace Forest.Gui.Converters
{
    public class SelectedItemToMainTreeEventViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is EventTreeViewModel eventTreeViewModel))
                return null;

            return eventTreeViewModel.MainTreeEventViewModel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}