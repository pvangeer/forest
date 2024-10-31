using System;
using System.Globalization;
using System.Windows.Data;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.TreeView.Converters
{
    public class LineStyleToDashArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is LineStyle strokeType)) return "1 0";

            switch (strokeType)
            {
                case LineStyle.Dash:
                    return "3 2";
                case LineStyle.DashDot:
                    return "3 2 1 2";
                case LineStyle.DashDotDot:
                    return "3 2 1 2 1 2";
                case LineStyle.LongDash:
                    return "5 2";
                case LineStyle.SmallDash:
                    return "1 2";
                case LineStyle.Solid:
                    return "1 0";
                default:
                    return "1 0";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string str)) throw new ArgumentException("Value should be of type string");

            switch (str)
            {
                case "3 1":
                    return LineStyle.Dash;
                case "3 1 1 1":
                    return LineStyle.DashDot;
                case "3 1 1 1 1 1":
                    return LineStyle.DashDotDot;
                case "5 1":
                    return LineStyle.LongDash;
                case "0.8 1":
                    return LineStyle.SmallDash;
                case "1 0":
                    return LineStyle.Solid;
                default:
                    return LineStyle.Solid;
            }
        }
    }
}