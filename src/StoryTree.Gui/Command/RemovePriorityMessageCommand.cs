using System;
using System.Windows.Input;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
{
    public class RemovePriorityMessageCommand : ICommand
    {
        public RemovePriorityMessageCommand(GuiViewModel guiViewModel)
        {
            ViewModel = guiViewModel;
        }

        private GuiViewModel ViewModel { get; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.PriorityMessage = null;
            ViewModel.OnPropertyChanged(nameof(GuiViewModel.PriorityMessage));
        }

        public event EventHandler CanExecuteChanged;
    }
}