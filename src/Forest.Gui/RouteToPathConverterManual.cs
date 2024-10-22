using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Forest.Gui
{
    public class RouteToPathConverterManual : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 6)
                return null;

            var source = (Control)values[4];
            var target = (Control)values[5];

            var sourceX = (double)values[0];
            var sourceY = (double)values[1];
            var targetX = (double)values[2];
            var targetY = (double)values[3];
            
            var startY = GetHeightFromTreeEventControl(sourceY,source);
            var endY = GetHeightFromTreeEventControl(targetY, target);

            var startPoint = new Point(sourceX + 0.5 * source.ActualWidth, startY);
            var endPoint = new Point(targetX - 0.5 * target.ActualWidth, endY);
            var midPointX = (startPoint.X + endPoint.X) / 2.0;

            return new PathFigureCollection
            {
                new PathFigure(startPoint,
                    new PathSegment[]
                    {
                        new LineSegment(new Point(midPointX, startPoint.Y), true),
                        new LineSegment(new Point(midPointX, endPoint.Y), true),
                        new LineSegment(endPoint, true)
                    }, false)
            };
        }

        private double GetHeightFromTreeEventControl(double controlY, Control control)
        {
            var titleBlock = FindFirstVisualChild<TextBlock>(control, block => block.Name == "DescriptionTextBlock");
            if (titleBlock == null)
            {
                return controlY;
            }

            return controlY + control.ActualHeight / 2.0 - titleBlock.ActualHeight - 1.5;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static T FindFirstVisualChild<T>(DependencyObject parent,Func<T,bool> selectionFunc)
            where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var childObject = VisualTreeHelper.GetChild(parent,i);
                if (childObject is T typedChild && selectionFunc(typedChild))
                {
                    return typedChild;
                }
                var foundControl = FindFirstVisualChild(childObject, selectionFunc);
                if (foundControl != null)
                {
                    return foundControl;
                }
            }

            return null;
        }
    }
}