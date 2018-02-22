using System;
using System.Windows.Input;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
{
    public abstract class EventTreeCommand : ICommand
    {
        protected ProjectViewModel ProjectViewModel { get; }

        protected EventTreeCommand(ProjectViewModel projectViewModel)
        {
            ProjectViewModel = projectViewModel;
        }

        public virtual bool CanExecute(object parameter)
        {
            return ProjectViewModel != null && ProjectViewModel.SelectedTreeEvent != null;
        }

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }
}