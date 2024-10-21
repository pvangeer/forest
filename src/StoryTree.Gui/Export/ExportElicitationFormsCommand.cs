using System;
using System.Windows.Input;
using log4net;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Export
{
    public class ExportElicitationFormsCommand : ICommand
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExportElicitationFormsCommand));

        public bool CanExecute(object parameter)
        {
            return parameter is GuiViewModel guiViewModel && guiViewModel.ProjectViewModel.EventTreeProject.Experts.Count != 0;
        }

        public void Execute(object parameter)
        {
            if (!(parameter is GuiViewModel guiViewModel))
            {
                Log.Error("Er is iets misgegaan, waardoor exporteren niet mogelijk is.");
                return;
            }

            var dialog = new ElicitationFormExportDialog
            {
                DataContext = new ExportElicitationFormsViewModel(guiViewModel.ProjectViewModel.EventTreeProject)
                {
                    OnExport = guiViewModel.OnExportElicitationForms
                },
                Owner = guiViewModel.Win32Window
            };

            dialog.ShowDialog();

            if (dialog.DialogResult != true)
                Log.Info("Exporteren van DOT formulieren is door de gebruiker afgebroken.");
        }

        public event EventHandler CanExecuteChanged;
    }
}