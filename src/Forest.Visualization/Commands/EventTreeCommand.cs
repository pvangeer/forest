using System;
using System.ComponentModel;
using System.Windows.Input;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization.Commands
{
    public abstract class EventTreeCommand : ICommand
    {
        protected EventTreeCommand(RibbonViewModel viewModel)
        {
            ViewModel = viewModel;
            if (ViewModel != null)
            {
                viewModel.PropertyChanged += ViewModelPropertyChanged;
            }
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(RibbonViewModel.SelectedTreeEvent):
                    FireCanExecuteChanged();
                    break;
            }
        }

        protected RibbonViewModel ViewModel { get; }

        public virtual bool CanExecute(object parameter)
        {
            return ViewModel?.SelectedTreeEvent != null;
        }

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }
}