using System.Windows;
using Forest.Visualization.Utils;

namespace Forest.Visualization.Behaviors
{
    public static class SaveToImageBehavior
    {
        public static readonly DependencyProperty SaveToImageProperty =
            DependencyProperty.RegisterAttached("SaveToImage", typeof(bool), typeof(SaveToImageBehavior),
                new UIPropertyMetadata(false, OnSaveToImage));

        public static void SetSaveToImage(DependencyObject obj, bool value)
        {
            obj.SetValue(SaveToImageProperty, value);
        }

        public static bool GetSaveToImage(DependencyObject obj)
        {
            return (bool)obj.GetValue(SaveToImageProperty);
        }

        private static void OnSaveToImage(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                if (obj is FrameworkElement frameworkElement)
                    frameworkElement.SaveToFile();
        }
    }
}