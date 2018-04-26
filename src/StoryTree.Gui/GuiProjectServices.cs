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
        private readonly StoryTreeLog log = new StoryTreeLog(typeof(GuiProjectServices));
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
            HandleUnsavedChanges(guiViewModel.Gui, CreateNewProject);
        }

        private void CreateNewProject()
        {
            storageSqLite.UnstageProject();
            guiViewModel.ProjectFilePath = "";

            guiViewModel.Gui.Project = new Project();
            guiViewModel.Gui.OnPropertyChanged(nameof(StoryTreeGui.Project));
        }

        public void OpenProject()
        {
            storageSqLite.UnstageProject();

            HandleUnsavedChanges(guiViewModel.Gui, OpenNewProjectCore);
        }

        private void OpenNewProjectCore()
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "StoryTree bestand (*.sqlite)|*.sqlite",
                FileName = guiViewModel.ProjectFilePath,
            };

            if ((bool) dialog.ShowDialog(Win32Window))
            {
                ChangeState(StorageState.Busy);

                var worker = new BackgroundWorker();
                worker.DoWork += OpenProjectAsync;
                worker.RunWorkerCompleted += (o, e) => BackgroundWorkerAsyncFinished(o, e,
                    () =>
                    {
                        guiViewModel.ProjectFilePath = dialog.FileName;
                        log.Info($"Klaar met openen van project uit bestand '{guiViewModel.ProjectFilePath}'.");
                    });
                worker.WorkerSupportsCancellation = false;

                worker.RunWorkerAsync(dialog.FileName);
            }
        }

        public void SaveProject(Action followingAction = null)
        {
            if (string.IsNullOrWhiteSpace(guiViewModel.ProjectFilePath))
            {
                SaveProjectAs(followingAction);
                return;
            }

            StageProjectAndStore(followingAction);
        }

        public void SaveProjectAs(Action followingAction = null)
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
                StageProjectAndStore(followingAction);
            }
        }

        private void StageProjectAndStore(Action followingAction = null)
        {
            ChangeState(StorageState.Busy);
            var worker = new BackgroundWorker();
            worker.DoWork += StageAndStoreProjectAsync;
            worker.RunWorkerCompleted += (o, e) => BackgroundWorkerAsyncFinished(o, e,
                () =>
                {
                    log.Info($"Project is opgeslagen in bestand '{guiViewModel.ProjectFilePath}'.");
                    followingAction?.Invoke();
                });
            worker.WorkerSupportsCancellation = false;

            worker.RunWorkerAsync();
        }

        private void StageAndStoreProjectAsync(object sender, DoWorkEventArgs e)
        {
            try
            {
                StageAndStoreProjectCore();
            }
            catch (Exception exception)
            {
                e.Result = exception;
            }
        }

        private void BackgroundWorkerAsyncFinished(object sender, RunWorkerCompletedEventArgs e, Action workFinishedAction)
        {
            if (e.Result is Exception exception)
            {
                log.Error(exception.Message);
            }
            else
            {
                workFinishedAction();
            }

            ChangeState(StorageState.Idle);
        }

        private void OpenProjectAsync(object sender, DoWorkEventArgs e)
        {
            var fileName = e.Argument as string;
            try
            {
                guiViewModel.Gui.Project = storageSqLite.LoadProject(fileName);
                guiViewModel.Gui.OnPropertyChanged(nameof(StoryTreeGui.Project));
            }
            catch (Exception exception)
            {
                e.Result = exception;
            }
        }

        // TODO: Also use this method when exiting the gui 
        private void HandleUnsavedChanges(StoryTreeGui gui, Action followingAction)
        {
            storageSqLite.StageProject(gui.Project);
            if (storageSqLite.HasStagedProjectChanges(gui.ProjectFilePath))
            {
                if (gui.ShouldSaveOpenChanges != null && gui.ShouldSaveOpenChanges())
                {
                    SaveProject(followingAction);
                }
                else
                {
                    followingAction();
                }
            }
            else
            {
                followingAction();
            }
        }

        private void StageAndStoreProjectCore()
        {
            if (!storageSqLite.HasStagedProject)
            {
                storageSqLite.StageProject(guiViewModel.Gui.Project);
            }

            storageSqLite.SaveProjectAs(guiViewModel.ProjectFilePath);
        }

        private void ChangeState(StorageState state)
        {
            guiViewModel.BusyIndicator = state;
            guiViewModel.OnPropertyChanged(nameof(GuiViewModel.BusyIndicator));
            guiViewModel.InvokeInvalidateVisual();
        }
    }
}
