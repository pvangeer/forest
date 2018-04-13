using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
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
            if (string.IsNullOrWhiteSpace(ProjectFilePath))
            {
                OnFileSaveAsClicked(sender,e);
            }

            ChangeState(StorageState.Busy);
            
            StageProjectAndStore();
        }

        private void StageProjectAndStore()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += StageAndeStoreProjectAsync;
            worker.RunWorkerCompleted += StageAndStoreProjectAsyncFinished;
            worker.WorkerSupportsCancellation = false;

            worker.RunWorkerAsync(new BackgroundWorkerArguments(storageSqLite, ((ProjectViewModel)DataContext).Project, ProjectFilePath));
        }

        private void StageAndStoreProjectAsyncFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is Exception)
            {
                // Handle exception
            }

            ChangeState(StorageState.Idle);
        }

        private static void StageAndeStoreProjectAsync(object sender, DoWorkEventArgs e)
        {
            if (!(e.Argument is BackgroundWorkerArguments arguments))
            {
                return;
            }

            try
            {
                if (!arguments.StorageSqLite.HasStagedProject)
                {
                    arguments.StorageSqLite.StageProject(arguments.Project);
                }

                arguments.StorageSqLite.SaveProjectAs(arguments.ProjectFilePath);
            }
            catch (Exception exception)
            {
                e.Result = exception;
            }
        }

        private void OnFileSaveAsClicked(object sender, RoutedEventArgs e)
        {
            ChangeState(StorageState.Busy);
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
                return;
            }

            ChangeState(StorageState.Idle);
        }

        private void ChangeState(StorageState state)
        {
            if (DataContext == null)
            {
                return;
            }

            var projectViewModel = (ProjectViewModel) DataContext;
            projectViewModel.BusyIndicator = state;
            projectViewModel.OnPropertyChanged(nameof(projectViewModel.BusyIndicator));
            BusyStatusBarItem.InvalidateVisual();
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

    internal class BackgroundWorkerArguments
    {
        public BackgroundWorkerArguments(StorageSqLite storageSqLite, Project project, string projectFilePath)
        {
            StorageSqLite = storageSqLite;
            Project = project;
            ProjectFilePath = projectFilePath;
        }

        public string ProjectFilePath { get; }

        public StorageSqLite StorageSqLite { get; }

        public Project Project { get; }
    }
}
