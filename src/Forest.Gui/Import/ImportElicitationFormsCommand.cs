using System;
using System.Linq;
using System.Windows.Input;
using Forest.Gui.ViewModels;
using Forest.Messaging;
using Microsoft.Win32;

namespace Forest.Gui.Import
{
    public class ImportElicitationFormsCommand : ICommand
    {
        private readonly StoryTreeLog log = new StoryTreeLog(typeof(ImportElicitationFormsCommand));

        public bool CanExecute(object parameter)
        {
            return parameter is GuiViewModel guiViewModel && guiViewModel.ProjectViewModel.EventTreeProject.Experts.Count != 0;
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

                if (dialog.ShowDialog(guiViewModel.Win32Window) == true && dialog.FileNames.Any())
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