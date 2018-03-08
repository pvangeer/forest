using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Fluent;

namespace StoryTree.Gui
{
    public class TabItemToTemplateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is RibbonTabItem tabItem))
            {
                return null;
            }

            switch (tabItem.Name)
            {
                case nameof(MainWindow.ExpertsTabItem):
                    return ExpertsTemplate;
                case nameof(MainWindow.GeneralDataTabItem):
                    return GeneralDataTemplate;
                case nameof(MainWindow.HydraulicsTabItem):
                    return HydraulicsTemplate;
                case nameof(MainWindow.EventsTabItem):
                    return EventsTemplate;
                case nameof(MainWindow.TreeEventsTabItem):
                    return TreeEventsTemplate;
                case nameof(MainWindow.EventsTreeTabItem):
                    return EventsTreeTemplate;
                case nameof(MainWindow.ResultsTabItem):
                    return ResultsTemplate;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public DataTemplate ExpertsTemplate { get; set; }

        public DataTemplate GeneralDataTemplate { get; set; }

        public DataTemplate HydraulicsTemplate { get; set; }

        public DataTemplate EventsTemplate { get; set; }

        public DataTemplate TreeEventsTemplate { get; set; }

        public DataTemplate EventsTreeTemplate { get; set; }

        public DataTemplate ResultsTemplate { get; set; }
    }
}