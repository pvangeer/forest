using System;
using System.ComponentModel;
using System.Windows.Input;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.TreeView.Commands
{
    public class ToggleIsExpandedCommand : ICommand
    {
        private readonly IExpandable expandableViewModel;

        public ToggleIsExpandedCommand(IExpandable expandableViewModel)
        {
            this.expandableViewModel = expandableViewModel;
            if (this.expandableViewModel != null) this.expandableViewModel.PropertyChanged += ExpandablePropertyChanged;
        }

        public bool CanExecute(object parameter)
        {
            return expandableViewModel.IsExpandable;
        }

        public void Execute(object parameter)
        {
            if (expandableViewModel != null && expandableViewModel.IsExpandable)
                expandableViewModel.IsExpanded = !expandableViewModel.IsExpanded;
        }

        public event EventHandler CanExecuteChanged;

        private void ExpandablePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IExpandable.IsExpandable):
                    CanExecuteChanged?.Invoke(this, null);
                    break;
            }
        }
    }
}