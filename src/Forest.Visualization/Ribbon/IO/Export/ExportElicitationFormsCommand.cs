using System;
using System.Windows;
using System.Windows.Input;
using Forest.Visualization.ViewModels;
using log4net;

namespace Forest.Visualization.Ribbon.IO.Export
{
    public class ExportElicitationFormsCommand : ICommand
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExportElicitationFormsCommand));

        public bool CanExecute(object parameter)
        {
            // TODO: Change this.
            return parameter is EventTreeViewModelOld eventTreeViewModel && eventTreeViewModel.SelectedEstimationHasExperts();
        }

        public void Execute(object parameter)
        {
            if (!(parameter is EventTreeViewModelOld eventTreeViewModel))
            {
                Log.Error("Er is iets misgegaan, waardoor exporteren niet mogelijk is.");
                return;
            }

            var dialog = new ElicitationFormExportDialog
            {
                DataContext = new ExportElicitationFormsViewModel(eventTreeViewModel.GetSelectedEstimationPerTreeEvent())
                {
                    OnExport = eventTreeViewModel.OnExportElicitationForms
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