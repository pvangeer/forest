using System.Windows;

namespace Forest.Gui.Export
{
    /// <summary>
    ///     Interaction logic for ElicitationFormExportDialog.xaml
    /// </summary>
    public partial class ElicitationFormExportDialog : Window
    {
        public ElicitationFormExportDialog()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}