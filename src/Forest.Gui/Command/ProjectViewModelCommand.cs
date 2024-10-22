using System;
using System.Windows.Input;
using Forest.Gui.ViewModels;

namespace Forest.Gui.Command
{
    public abstract class ProjectViewModelCommand : ICommand
    {
        protected ProjectViewModelCommand(ProjectViewModel projectViewModel)
        {
            ProjectViewModel = projectViewModel;
        }

        protected ProjectViewModel ProjectViewModel { get; }

        public virtual bool CanExecute(object parameter)
        {
            return ProjectViewModel != null;
        }

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;
    }
}