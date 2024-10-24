using System.ComponentModel;
using System.Windows.Input;
using Forest.Data;
using Forest.Gui.Components;
using Forest.Gui.ViewModels;

namespace Forest.Gui
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var forestGui = new ForestGui
            {
                EventTreeProject = EventTreeProjectFactory.CreateStandardNewProject()
            };
            var guiViewModel = new MainWindowViewModel(forestGui);
            DataContext = guiViewModel;

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