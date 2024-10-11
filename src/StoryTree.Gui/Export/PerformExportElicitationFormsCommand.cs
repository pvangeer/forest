using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StoryTree.Gui.Export
{
    public class PerformExportElicitationFormsCommand : ICommand
    {
        private readonly ExportElicitationFormsViewModel viewModel;

        public PerformExportElicitationFormsCommand(ExportElicitationFormsViewModel exportElicitationFormsViewModel)
        {
            this.viewModel = exportElicitationFormsViewModel;
            exportElicitationFormsViewModel.CanExportChanged += CanExportChanged;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel != null &&
                   viewModel.Experts.Any(e => e.IsChecked);
        }

        public void Execute(object parameter)
        {
            try
            {
                viewModel.OnExportHandler();
            }
            finally
            {
                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
        }

        private void CanExportChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this,null);
        }

        public event EventHandler CanExecuteChanged;
    }
}