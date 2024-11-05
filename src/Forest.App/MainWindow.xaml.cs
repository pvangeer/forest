using System.ComponentModel;
using System.Windows.Input;
using Forest.App.ViewModels;

namespace Forest.App
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
                e.Cancel = !viewModel.ForcedClosingMainWindow();
        }
    }
}