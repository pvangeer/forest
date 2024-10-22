using System;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Forest.Gui.Export
{
    public class SelectFileLocationCommand : ICommand
    {
        private readonly ExportElicitationFormsViewModel viewModel;

        public SelectFileLocationCommand(ExportElicitationFormsViewModel exportElicitationFormsViewModel)
        {
            viewModel = exportElicitationFormsViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var dlg = new CommonOpenFileDialog
            {
                Title = "Selecteer locatie",
                IsFolderPicker = true,
                InitialDirectory = viewModel.ExportLocation,
                AddToMostRecentlyUsedList = false,
                AllowNonFileSystemItems = false,
                DefaultDirectory = viewModel.ExportLocation,
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };


            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                viewModel.ExportLocation = dlg.FileName;
                viewModel.OnPropertyChanged(nameof(viewModel.ExportLocation));
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}