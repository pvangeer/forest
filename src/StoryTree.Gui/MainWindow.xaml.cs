using System.Windows;
using System.Windows.Input;
using StoryTree.Data;
using StoryTree.Data.Estimations;
using StoryTree.Data.Estimations.Classes;
using StoryTree.Data.Tree;
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
            InitializeComponent();
            DataContext = new ProjectViewModel(TestDataGenerator.GenerateAsphalProject());
        }

        private void OnFileNewClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnFileSaveClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnFileSaveAsClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnFileOpenClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnFileExitClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
