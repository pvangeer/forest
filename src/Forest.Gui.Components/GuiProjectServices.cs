using System;
using System.ComponentModel;
using Forest.Data;
using Forest.Messaging;
using Forest.Storage;
using Forest.Storage.Migration;
using log4net;
using static System.Net.Mime.MediaTypeNames;

namespace Forest.Gui.Components
{
    public class GuiProjectServices
    {
        private readonly ForestGui gui;
        private readonly ForestLog log = new ForestLog(typeof(GuiProjectServices));
        private readonly StorageXml storageXml;

        public GuiProjectServices(ForestGui gui)
        {
            this.gui = gui;
            storageXml = new StorageXml();
        }

        public Func<string,FileNameQuestionResult> OpenProjectFileNameFunc { get; set; }
        
        public Func<string,FileNameQuestionResult> SaveProjectFileNameFunc { get; set; }
        
        public void NewProject()
        {
            HandleUnsavedChanges(CreateNewProject);
        }

        private void CreateNewProject()
        {
            storageXml.UnStageEventTreeProject();
            gui.ProjectFilePath = "";

            gui.EventTreeProject = EventTreeProjectFactory.CreateStandardNewProject();
            gui.OnPropertyChanged(nameof(ForestGui.EventTreeProject));
        }

        public void OpenProject()
        {
            storageXml.UnStageEventTreeProject();
            storageXml.UnStageVersionInformation();

            HandleUnsavedChanges(() =>
            {
                var result = OpenProjectFileNameFunc(gui.ProjectFilePath);
                if (result.Proceed)
                {
                    OpenProjectCore(result.FileName);
                }

            });
        }

        private void OpenProjectCore(string fileName)
        {
            var needsMigration = false;
            try
            {
                needsMigration = XmlStorageMigrationService.NeedsMigration(fileName);
            }
            catch (XmlStorageException e)
            {
                var message = e.Message;
                if (e.InnerException != null)
                    message = $"Er is een fout opgetreden bij het lezen van dit bestand: {e.InnerException}";
                log.Error(message);
                return;
            }

            if (needsMigration && gui.ShouldMigrateProject != null &&
                gui.ShouldMigrateProject())
            {
                if (!MigrateProject(fileName, out var newFileName))
                    return;

                fileName = newFileName;
            }

            ChangeState(StorageState.Busy);

            var worker = new BackgroundWorker();
            worker.DoWork += OpenProjectAsync;
            worker.RunWorkerCompleted += (o, e) => BackgroundWorkerAsyncFinished(o, e,
                () =>
                {
                    gui.ProjectFilePath = fileName;
                    log.Info($"Klaar met openen van project uit bestand '{gui.ProjectFilePath}'.");
                });
            worker.WorkerSupportsCancellation = false;

            worker.RunWorkerAsync(fileName);
        }

        private bool MigrateProject(string fileName, out string newFileName)
        {
            if (SaveProjectFileNameFunc == null)
            {
                throw new XmlMigrationException("SaveProjectFileNameFunc was not specified.");
            }
            var result = SaveProjectFileNameFunc(fileName.Replace(".xml",
                $"-migrated-{VersionInfo.Year}.{VersionInfo.MajorVersion}.{VersionInfo.MinorVersion}.xml"));

            newFileName = null;

            if (result.Proceed)
            {
                try
                {
                    XmlStorageMigrationService.MigrateFile(fileName, result.FileName);
                }
                catch (XmlMigrationException e)
                {
                    log.Error(e.Message);
                }
                catch (Exception e)
                {
                    log.Error($"Er is een fout opgetreden bij het migreren van dit bestand: {e.Message}");
                    return false;
                }

                newFileName = result.FileName;
                log.Info($"Migratie van bestand '{fileName}' is voltooid. Het resultaat is opgeslagen in het bestand '{newFileName}'");
                return true;
            }

            log.Info("Migratie gestopt door de gebruiker. Bestand wordt niet geopend.");
            return false;
        }

        public void SaveProject()
        {
            storageXml.UnStageEventTreeProject();
            SaveProject(null);
        }

        public void SaveProjectAs()
        {
            storageXml.UnStageEventTreeProject();
            SaveProjectAs(null);
        }

        private void SaveProject(Action followingAction)
        {
            if (string.IsNullOrWhiteSpace(gui.ProjectFilePath))
            {
                SaveProjectAs(followingAction);
                return;
            }

            StageProjectAndStore(followingAction);
        }

        private void SaveProjectAs(Action followingAction)
        {
            var result = SaveProjectFileNameFunc(gui.ProjectFilePath);
            
            if (result.Proceed)
            {
                gui.ProjectFilePath = result.FileName;
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
                    log.Info($"EventTreeProject is opgeslagen in bestand '{gui.ProjectFilePath}'.");
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
                log.Error(exception.Message);
            else
                workFinishedAction();

            ChangeState(StorageState.Idle);
        }

        private void OpenProjectAsync(object sender, DoWorkEventArgs e)
        {
            var fileName = e.Argument as string;
            try
            {
                var readProjectData = storageXml.LoadProject(fileName);
                gui.EventTreeProject = readProjectData.EventTreeProject;
                gui.VersionInfo = new VersionInfo
                {
                    AuthorCreated = readProjectData.Author,
                    DateCreated = readProjectData.Created
                };
                gui.ProjectFilePath = fileName;

                gui.OnPropertyChanged(nameof(ForestGui.EventTreeProject));
                gui.OnPropertyChanged(nameof(ForestGui.ProjectFilePath));
            }
            catch (Exception exception)
            {
                e.Result = exception;
            }
        }

        public bool HandleUnsavedChanges(Action followingAction)
        {
            storageXml.StageEventTreeProject(gui.EventTreeProject);
            storageXml.StageVersionInformation(gui.VersionInfo);
            if (storageXml.HasStagedProjectChanges())
            {
                if (gui.ShouldSaveOpenChanges != null)
                    switch (gui.ShouldSaveOpenChanges())
                    {
                        case ShouldProceedState.Yes:
                            SaveProject(followingAction);
                            break;
                        case ShouldProceedState.No:
                            followingAction();
                            break;
                        case ShouldProceedState.Cancel:
                            return false;
                    }
                else
                    followingAction();
            }
            else
            {
                followingAction();
            }

            return true;
        }

        private void StageAndStoreProjectCore()
        {
            if (!storageXml.HasStagedEventTreeProject)
                storageXml.StageEventTreeProject(gui.EventTreeProject);

            storageXml.SaveProjectAs(gui.ProjectFilePath);
        }

        private void ChangeState(StorageState state)
        {
            gui.BusyIndicator = state;
            gui.OnPropertyChanged(nameof(ForestGui.BusyIndicator));
        }
    }
}