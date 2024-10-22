using System;
using System.Windows.Input;
using Forest.Gui.ViewModels;

namespace Forest.Gui.Command
{
    public class SaveProjectCommand : ICommand
    {
        public SaveProjectCommand(GuiViewModel guiViewModel)
        {
            ViewModel = guiViewModel;
        }

        public GuiViewModel ViewModel { get; }

        public bool CanExecute(object parameter)
        {
            return ViewModel.CanSaveProject();
        }

        public void Execute(object parameter)
        {
            ViewModel.SaveProject();
        }

        public event EventHandler CanExecuteChanged;
    }
}