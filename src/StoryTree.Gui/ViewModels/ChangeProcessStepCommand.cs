using System;
using System.Windows.Input;

namespace StoryTree.Gui.ViewModels
{
    public class ChangeProcessStepCommand : ICommand
    {
        public ChangeProcessStepCommand(GuiViewModel guiViewModel)
        {
            ViewModel = guiViewModel;
        }

        public GuiViewModel ViewModel { get; }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!(parameter is StoryTreeProcess process))
            {
                return;
            }

            ViewModel.SelectedProcess = process;
        }

        public event EventHandler CanExecuteChanged;
    }
}