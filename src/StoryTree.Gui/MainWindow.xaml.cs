using System;
using System.ComponentModel;
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
        private readonly StorageSqLite storageSqLite;

        public MainWindow()
        {
            DataContext = new GuiViewModel(new StoryTreeGui());
            InitializeComponent();
            storageSqLite = new StorageSqLite();
        }

        public GuiViewModel ViewModel => (GuiViewModel) DataContext;

        private void OnFileNewClicked(object sender, RoutedEventArgs e)
        {
            ViewModel.ProjectFilePath = "";
            storageSqLite.UnstageProject();
            ViewModel.Gui.Project = new Project();
            ViewModel.Gui.OnPropertyChanged(nameof(StoryTreeGui.Project));
        }

        private void OnFileSaveClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.ProjectFilePath))
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
            worker.RunWorkerCompleted += BackgroundWorkerAsyncFinished;
            worker.WorkerSupportsCancellation = false;

            worker.RunWorkerAsync(new BackgroundWorkerArguments(storageSqLite, ViewModel));
        }

        private void BackgroundWorkerAsyncFinished(object sender, RunWorkerCompletedEventArgs e)
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
                    arguments.StorageSqLite.StageProject(arguments.Gui.Project);
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
                FileName = ViewModel.ProjectFilePath,
                OverwritePrompt = true,
                Filter = "StoryTree bestand (*.sqlite)|*.sqlite"
            };

            if ((bool)dialog.ShowDialog(this))
            {
                ViewModel.ProjectFilePath = dialog.FileName;
                StageProjectAndStore();
                return;
            }

            ChangeState(StorageState.Idle);
        }

        private void ChangeState(StorageState state)
        {
            if (ViewModel == null)
            {
                return;
            }

            ViewModel.BusyIndicator = state;
            ViewModel.OnPropertyChanged(nameof(ProjectViewModel.BusyIndicator));
            BusyStatusBarItem.InvalidateVisual();
        }

        private void OnFileOpenClicked(object sender, RoutedEventArgs e)
        {
            ChangeState(StorageState.Busy);
            storageSqLite.UnstageProject();
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "StoryTree bestand (*.sqlite)|*.sqlite",
                FileName = ViewModel.ProjectFilePath,
            };

            if ((bool) dialog.ShowDialog(this))
            {
                var worker = new BackgroundWorker();
                worker.DoWork += OpenProjectAsync;
                worker.RunWorkerCompleted += BackgroundWorkerAsyncFinished;
                worker.WorkerSupportsCancellation = false;

                ViewModel.ProjectFilePath = dialog.FileName;
                worker.RunWorkerAsync(new BackgroundWorkerArguments(storageSqLite, ViewModel));
            }
            ChangeState(StorageState.Idle);
        }

        private void OpenProjectAsync(object sender, DoWorkEventArgs e)
        {
            if (!(e.Argument is BackgroundWorkerArguments arguments))
            {
                return;
            }

            try
            {
                arguments.Gui.Project = storageSqLite.LoadProject(arguments.ProjectFilePath);
                arguments.Gui.OnPropertyChanged(nameof(StoryTreeGui.Project));
            }
            catch (Exception exception)
            {
                e.Result = exception;
            }
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }

    internal class BackgroundWorkerArguments
    {
        public BackgroundWorkerArguments(StorageSqLite storageSqLite, GuiViewModel guiViewModel)
        {
            StorageSqLite = storageSqLite;
            Gui = guiViewModel.Gui;
            ProjectFilePath = guiViewModel.ProjectFilePath;
        }

        public string ProjectFilePath { get; }

        public StorageSqLite StorageSqLite { get; }

        public StoryTreeGui Gui { get; }
    }
}
