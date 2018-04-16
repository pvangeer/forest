using System;
using System.Windows.Input;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
{
    public class ShowMessageListCommand : ICommand
    {
        public ShowMessageListCommand(GuiViewModel guiViewModel)
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
            ViewModel.ShowMessages = ViewModel.Messages.Count != 0;
            ViewModel.OnPropertyChanged(nameof(GuiViewModel.ShowMessages));
        }

        public event EventHandler CanExecuteChanged;
    }
}