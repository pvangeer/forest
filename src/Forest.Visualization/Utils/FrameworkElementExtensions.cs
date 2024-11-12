using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Forest.Visualization.Utils
{
    public static class FrameworkElementExtensions
    {
        private const double ScreenDpi = 96.0;

        public static void SaveToFile(this FrameworkElement frameworkElement, double scalingFactor = 10.0)
        {
            if (frameworkElement == null) return;

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Afbeelding *.png|*.png"
            };

            if (saveFileDialog.ShowDialog() == false)
                return;

            Render(frameworkElement, saveFileDialog.FileName, scalingFactor);
        }

        private static void Render(FrameworkElement presenter, string path, double scalingFactor)
        {
            var source = PresentationSource.FromVisual(presenter);

            var dpiX = double.NaN;
            var dpiY = double.NaN;
            if (source?.CompositionTarget != null)
            {
                dpiX = ScreenDpi * source.CompositionTarget.TransformToDevice.M11;
                dpiY = ScreenDpi * source.CompositionTarget.TransformToDevice.M22;
            }

            if (Math.Abs(dpiX - dpiY) > 1e-10) return;

            var dpi = dpiX * scalingFactor;
            var scale = dpi / ScreenDpi;
            var renderBitmap =
                new RenderTargetBitmap(
                    (int)(presenter.ActualWidth * scale),
                    (int)(presenter.ActualHeight * scale),
                    dpi,
                    dpi,
                    PixelFormats.Pbgra32);
            renderBitmap.Render(presenter);

            BitmapEncoder encoder = new PngBitmapEncoder();
            using (var outStream = new FileStream(path, FileMode.Create))
            {
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(outStream);
            }
        }
    }
}