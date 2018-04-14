using System.Windows.Input;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            var guiViewModel = new GuiViewModel(new StoryTreeGui())
            {
                Win32Window = this
            };
            guiViewModel.OnInvalidateVisual += (o,e) =>
            {
                HostControl.InvalidateVisual();
                InvalidateVisual();
            };
            DataContext = guiViewModel;

            InitializeComponent();
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }

    
}
