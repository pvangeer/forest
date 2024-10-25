using System;
using System.Globalization;
using System.Windows.Data;
using Forest.Gui.Components;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization.Converters
{
    public class GuiStateToViewModelConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is ForestGuiState guiState) || !(values[1] is ContentPresenterViewModel contentPresenterViewModel))
                return parameter;

            switch (guiState)
            {
                case ForestGuiState.Experts:
                    return contentPresenterViewModel.ExpertsViewModel;
                case ForestGuiState.Hydraulics:
                    return contentPresenterViewModel.HydrodynamicsViewModel;
                default:
                    return contentPresenterViewModel;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}