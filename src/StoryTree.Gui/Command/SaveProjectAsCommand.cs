using System;
using System.Windows.Input;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
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
            return ViewModel.Gui.Project != null;
        }

        public void Execute(object parameter)
        {
            ViewModel.GuiProjectSercices.SaveProjectAs();
        }

        public event EventHandler CanExecuteChanged;
    }
}