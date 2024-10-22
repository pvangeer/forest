using System.ComponentModel;
using System.Windows.Input;
using Forest.Data;
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

            var storyTreeGui = new StoryTreeGui
            {
                EventTreeProject = new EventTreeProject()
            };
            var guiViewModel = new GuiViewModel(storyTreeGui)
            {
                Win32Window = this
            };
            guiViewModel.OnInvalidateVisual += (o, e) =>
            {
                HostControl.InvalidateVisual();
                InvalidateVisual();
            };
            DataContext = guiViewModel;
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            if (DataContext is GuiViewModel viewModel)
                e.Cancel = !viewModel.ForcedClosingMainWindow();
        }
    }
}