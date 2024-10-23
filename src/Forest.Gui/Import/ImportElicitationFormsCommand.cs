using System;
using System.Linq;
using System.Net.Mime;
using System.Windows;
using System.Windows.Input;
using Forest.Gui.ViewModels;
using Forest.Messaging;
using Microsoft.Win32;

namespace Forest.Gui.Import
{
    public class ImportElicitationFormsCommand : ICommand
    {
        private readonly ForestLog log = new ForestLog(typeof(ImportElicitationFormsCommand));

        public bool CanExecute(object parameter)
        {
            return parameter is GuiViewModel guiViewModel && guiViewModel.ProjectHasExperts();
        }

        public void Execute(object parameter)
        {
            if (!(parameter is GuiViewModel guiViewModel))
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
                    guiViewModel.OnImportElicitationForms(dialog.FileNames);
            }
            catch (Exception e)
            {
                log.Error($"Er is iets onverwachts misgegaan tijdens het importeren: {e.Message}");
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}