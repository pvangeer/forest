using System;
using System.Windows.Input;

namespace StoryTree.Gui.ViewModels
{
    public class ToggleShowMessagesCommand : ICommand
    {
        public ToggleShowMessagesCommand(GuiViewModel guiViewModel)
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
            ViewModel.ShowMessages = ViewModel.Messages.Count != 0 && !ViewModel.ShowMessages;
            ViewModel.OnPropertyChanged(nameof(GuiViewModel.ShowMessages));
        }

        public event EventHandler CanExecuteChanged;
    }
}