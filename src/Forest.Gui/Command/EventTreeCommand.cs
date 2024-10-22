using System;
using System.Windows.Input;
using Forest.Gui.ViewModels;

namespace Forest.Gui.Command
{
    public abstract class EventTreeCommand : ICommand
    {
        protected EventTreeCommand(ProjectViewModel projectViewModel)
        {
            ProjectViewModel = projectViewModel;
        }

        protected ProjectViewModel ProjectViewModel { get; }

        public virtual bool CanExecute(object parameter)
        {
            return ProjectViewModel?.SelectedTreeEvent != null;
        }

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }
}