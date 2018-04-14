using System;
using System.Windows.Input;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
{
    public class OpenProjectCommand : ICommand
    {
        public OpenProjectCommand(GuiViewModel guiViewModel)
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
            ViewModel.GuiProjectSercices.OpenProject();
        }

        public event EventHandler CanExecuteChanged;
    }
}