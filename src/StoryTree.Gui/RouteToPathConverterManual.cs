using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace StoryTree.Gui
{
    public class RouteToPathConverterManual : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 9)
                return null;

            var sourceX = (double)values[0];
            var sourceY = (double)values[1];
            var sourceWidth = (double)values[2];
            var targetX = (double)values[4];
            var targetY = (double)values[5];
            var targetWidth = (double)values[6];
            var startPoint = new Point(sourceX + 0.5 * sourceWidth, sourceY);
            var endPoint = new Point(targetX - 0.5 * targetWidth, targetY);
            var midPointX = (startPoint.X + endPoint.X) / 2.0;

            return new PathFigureCollection()
            {
                new PathFigure(startPoint,
                    new PathSegment[]
                    {
                        new LineSegment(new Point(midPointX, startPoint.Y), true),
                        new LineSegment(new Point(midPointX, endPoint.Y), true),
                        new LineSegment(endPoint, true),
                    }, false)
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}