using System;
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
            return parameter is MainWindowViewModel guiViewModel && guiViewModel.SelectedEstimationHasExperts();
        }

        public void Execute(object parameter)
        {
            // TODO: Parameter should not be mainWindowViewModel
            if (!(parameter is MainWindowViewModel mainWindowViewModel))
            {
                Log.Error("Er is iets misgegaan, waardoor exporteren niet mogelijk is.");
                return;
            }

            var dialog = new ElicitationFormExportDialog
            {
                DataContext = new ExportElicitationFormsViewModel(mainWindowViewModel.GetSelectedEstimationPerTreeEvent())
                {
                    OnExport = mainWindowViewModel.OnExportElicitationForms
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