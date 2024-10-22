using System;
using System.Windows.Input;
using Forest.Gui.ViewModels;

namespace Forest.Gui.Command
{
    public class SaveProjectAsCommand : ICommand
    {
        public SaveProjectAsCommand(GuiViewModel guiViewModel)
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
            ViewModel.SaveProjectAs();
        }

        public event EventHandler CanExecuteChanged;
    }
}