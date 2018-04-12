using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using StoryTree.Data;
using StoryTree.Gui.ViewModels;
using StoryTree.Storage;

namespace StoryTree.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private StorageSqLite storageSqLite;

        public string ProjectFilePath { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ProjectViewModel(TestDataGenerator.GenerateAsphalProject());
            storageSqLite = new StorageSqLite();
        }

        private void OnFileNewClicked(object sender, RoutedEventArgs e)
        {
            ProjectFilePath = "";
            DataContext = new ProjectViewModel(new Project());
            storageSqLite.UnstageProject();
        }

        private void OnFileSaveClicked(object sender, RoutedEventArgs e)
        {
            if (ProjectFilePath == null)
            {
                OnFileSaveAsClicked(sender,e);
            }

            StageProjectAndStore();
        }

        private void StageProjectAndStore()
        {
            if (!storageSqLite.HasStagedProject)
            {
                storageSqLite.StageProject(((ProjectViewModel)DataContext).Project);
            }

            storageSqLite.SaveProjectAs(ProjectFilePath);
        }

        private void OnFileSaveAsClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                CheckPathExists = true,
                FileName = ProjectFilePath,
                OverwritePrompt = true,
                Filter = "StoryTree bestand (*.sqlite)|*.sqlite"
            };

            if ((bool)dialog.ShowDialog(this))
            {
                ProjectFilePath = dialog.FileName;
                StageProjectAndStore();
            }
        }

        private void OnFileOpenClicked(object sender, RoutedEventArgs e)
        {
            storageSqLite.UnstageProject();
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "StoryTree bestand (*.sqlite)|*.sqlite",
                FileName = ProjectFilePath,
            };

            if ((bool) dialog.ShowDialog(this))
            {
                DataContext = new ProjectViewModel(storageSqLite.LoadProject(dialog.FileName));
                ProjectFilePath = dialog.FileName;
            }
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
