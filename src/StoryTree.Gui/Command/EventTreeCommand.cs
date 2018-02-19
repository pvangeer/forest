using System;
using System.Windows.Input;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
{
    public abstract class EventTreeCommand : ICommand
    {
        public EventTreeCommand(EventTreeViewModel selectedEventTreeViewModel)
        {
            SelectedEventTreeViewModel = selectedEventTreeViewModel;
        }

        private EventTreeViewModel selectedEventTreeViewModel;
        public EventTreeViewModel SelectedEventTreeViewModel
        {
            protected get => selectedEventTreeViewModel;
            set
            {
                selectedEventTreeViewModel = value;
                CanExecuteChanged?.Invoke(this, null);
            }
        }

        public virtual bool CanExecute(object parameter)
        {
            return SelectedEventTreeViewModel != null;
        }

        public abstract void Execute(object parameter);
        
        public event EventHandler CanExecuteChanged;
    }
}