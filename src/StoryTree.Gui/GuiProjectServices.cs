using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Win32;
using StoryTree.Data;
using StoryTree.Gui.ViewModels;
using StoryTree.Messaging;
using StoryTree.Storage;

namespace StoryTree.Gui
{
    public class GuiProjectServices
    {
        private readonly GuiViewModel guiViewModel;
        private readonly StorageSqLite storageSqLite;

        public GuiProjectServices(GuiViewModel guiViewModel)
        {
            this.guiViewModel = guiViewModel;
            storageSqLite = new StorageSqLite();
        }

        public Window Win32Window { get; set; }

        public void NewProject()
        {
            guiViewModel.ProjectFilePath = "";
            storageSqLite.UnstageProject();
            guiViewModel.Gui.Project = new Project();
            guiViewModel.Gui.OnPropertyChanged(nameof(StoryTreeGui.Project));
        }

        public void OpenProject()
        {
            storageSqLite.UnstageProject();
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "StoryTree bestand (*.sqlite)|*.sqlite",
                FileName = guiViewModel.ProjectFilePath,
            };

            if ((bool)dialog.ShowDialog(Win32Window))
            {
                guiViewModel.Messages.Add(new LogMessage
                {
                    Severity = MessageSeverity.Information,
                    Message = $"Bezig met openen van proejct uit bestand '{dialog.FileName}'"
                });

                ChangeState(StorageState.Busy);

                var worker = new BackgroundWorker();
                worker.DoWork += OpenProjectAsync;
                worker.RunWorkerCompleted += BackgroundWorkerAsyncFinished;
                worker.WorkerSupportsCancellation = false;

                guiViewModel.ProjectFilePath = dialog.FileName;
                worker.RunWorkerAsync(new BackgroundWorkerArguments(storageSqLite, guiViewModel));

                guiViewModel.Messages.Add(new LogMessage
                {
                    Severity = MessageSeverity.Error,
                    Message = "Test met een hele lange test. Hopelijk gaat hij over meerdere regels worden verdeeld. Misschien moeten we er nog een titel aan toevoegen???"
                });
            }
        }

        public void SaveProject()
        {
            if (string.IsNullOrWhiteSpace(guiViewModel.ProjectFilePath))
            {
                SaveProjectAs();
            }

            StageProjectAndStore();
        }

        public void SaveProjectAs()
        {
            var dialog = new SaveFileDialog
            {
                CheckPathExists = true,
                FileName = guiViewModel.ProjectFilePath,
                OverwritePrompt = true,
                Filter = "StoryTree bestand (*.sqlite)|*.sqlite"
            };

            if ((bool)dialog.ShowDialog(Win32Window))
            {
                guiViewModel.ProjectFilePath = dialog.FileName;
                StageProjectAndStore();
            }
        }

        private void StageProjectAndStore()
        {
            ChangeState(StorageState.Busy);
            var worker = new BackgroundWorker();
            worker.DoWork += StageAndeStoreProjectAsync;
            worker.RunWorkerCompleted += BackgroundWorkerAsyncFinished;
            worker.WorkerSupportsCancellation = false;

            worker.RunWorkerAsync(new BackgroundWorkerArguments(storageSqLite, guiViewModel));
        }

        private void BackgroundWorkerAsyncFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is Exception)
            {
                // Handle exception
            }

            ChangeState(StorageState.Idle);
        }

        private static void OpenProjectAsync(object sender, DoWorkEventArgs e)
        {
            if (!(e.Argument is BackgroundWorkerArguments arguments))
            {
                return;
            }

            try
            {
                arguments.Gui.Project = arguments.StorageSqLite.LoadProject(arguments.ProjectFilePath);
                arguments.Gui.OnPropertyChanged(nameof(StoryTreeGui.Project));
            }
            catch (Exception exception)
            {
                e.Result = exception;
            }
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

        private void ChangeState(StorageState state)
        {
            guiViewModel.BusyIndicator = state;
            guiViewModel.OnPropertyChanged(nameof(GuiViewModel.BusyIndicator));
            guiViewModel.InvokeInvalidateVisual();
        }

        private class BackgroundWorkerArguments
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
}
