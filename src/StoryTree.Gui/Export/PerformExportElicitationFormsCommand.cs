using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StoryTree.Gui.Export
{
    public class PerformExportElicitationFormsCommand : ICommand
    {
        private readonly ExportExpertElicitationFormsViewModel viewModel;

        public PerformExportElicitationFormsCommand(ExportExpertElicitationFormsViewModel exportExpertElicitationFormsViewModel)
        {
            this.viewModel = exportExpertElicitationFormsViewModel;
            exportExpertElicitationFormsViewModel.CanExportChanged += CanExportChanged;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel != null &&
                   viewModel.Experts.Any(e => e.IsChecked) &&
                   viewModel.EventTrees.Any(e => e.IsChecked);
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