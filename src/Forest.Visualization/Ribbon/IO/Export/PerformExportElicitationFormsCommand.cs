using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Forest.Visualization.Ribbon.IO.Export
{
    public class PerformExportElicitationFormsCommand : ICommand
    {
        private readonly ExportElicitationFormsViewModel viewModel;

        public PerformExportElicitationFormsCommand(ExportElicitationFormsViewModel exportElicitationFormsViewModel)
        {
            viewModel = exportElicitationFormsViewModel;
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

        public event EventHandler CanExecuteChanged;

        private void CanExportChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }
}