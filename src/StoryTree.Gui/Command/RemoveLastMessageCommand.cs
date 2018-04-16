using System;
using System.Windows.Input;

namespace StoryTree.Gui.ViewModels
{
    public class RemoveLastMessageCommand : ICommand
    {
        public RemoveLastMessageCommand(GuiViewModel guiViewModel)
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
            ViewModel.LastErrorMessage = null;
            ViewModel.OnPropertyChanged(nameof(GuiViewModel.LastErrorMessage));
        }

        public event EventHandler CanExecuteChanged;
    }
}