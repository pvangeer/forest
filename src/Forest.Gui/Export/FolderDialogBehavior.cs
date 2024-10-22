using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Forest.Gui.Export
{
    public class FolderDialogBehavior : Behavior<Button>
    {
        private void OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog
            {
                Title = "Selecteer Bestandslocatie",
                IsFolderPicker = true,
                InitialDirectory = GetFolderName(this),
                AddToMostRecentlyUsedList = false,
                AllowNonFileSystemItems = false,
                DefaultDirectory = GetFolderName(this),
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                SetValue(FolderName, dlg.FileName);
        }

        #region Attached Behavior wiring

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += OnClick;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Click -= OnClick;
            base.OnDetaching();
        }

        #endregion

        #region FolderName Dependency Property

        public static readonly DependencyProperty FolderName =
            DependencyProperty.RegisterAttached("FolderName",
                typeof(string), typeof(FolderDialogBehavior));

        public static string GetFolderName(DependencyObject obj)
        {
            return (string)obj.GetValue(FolderName);
        }

        public static void SetFolderName(DependencyObject obj, string value)
        {
            obj.SetValue(FolderName, value);
        }

        #endregion
    }
}