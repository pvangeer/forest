using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Forest.Messaging;
using Forest.Visualization.ViewModels;
using Microsoft.Win32;

namespace Forest.Visualization.Ribbon.IO.Import
{
    public class ImportElicitationFormsCommand : ICommand
    {
        private readonly ForestLog log = new ForestLog(typeof(ImportElicitationFormsCommand));

        public bool CanExecute(object parameter)
        {
            return parameter is EventTreeViewModelOld eventTreeViewModel && eventTreeViewModel.SelectedEstimationHasExperts();
        }

        public void Execute(object parameter)
        {
            if (!(parameter is EventTreeViewModelOld eventTreeViewModel))
            {
                log.Error("Er is iets misgegaan, waardoor exporteren niet mogelijk is.");
                return;
            }

            try
            {
                var dialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Multiselect = true,
                    Filter = "Excel bestand (*.xlsx)|*.xlsx|Alle bestanden (*.*)|*.*"
                };

                if (dialog.ShowDialog(Application.Current.MainWindow) == true && dialog.FileNames.Any())
                    eventTreeViewModel.OnImportElicitationForms(dialog.FileNames);
            }
            catch (Exception e)
            {
                log.Error($"Er is iets onverwachts misgegaan tijdens het importeren: {e.Message}");
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}