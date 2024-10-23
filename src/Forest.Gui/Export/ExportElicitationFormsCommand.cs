using System;
using System.Net.Mime;
using System.Windows;
using System.Windows.Input;
using Forest.Gui.ViewModels;
using log4net;

namespace Forest.Gui.Export
{
    public class ExportElicitationFormsCommand : ICommand
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExportElicitationFormsCommand));

        public bool CanExecute(object parameter)
        {
            return parameter is GuiViewModel guiViewModel && guiViewModel.ProjectHasExperts();
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
                DataContext = new ExportElicitationFormsViewModel(guiViewModel.GetEventTreeProject())
                {
                    OnExport = guiViewModel.OnExportElicitationForms
                },
                Owner = Application.Current.MainWindow
            };

            dialog.ShowDialog();

            if (dialog.DialogResult != true)
                Log.Info("Exporteren van DOT formulieren is door de gebruiker afgebroken.");
        }

        public event EventHandler CanExecuteChanged;
    }
}