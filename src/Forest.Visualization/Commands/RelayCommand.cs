using System;
using System.Windows.Input;

namespace Forest.Visualization.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Func<bool> canExecuteEvaluator;
        private readonly Action methodToExecute;

        public RelayCommand(Action methodToExecute, Func<bool> canExecuteEvaluator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        public RelayCommand(Action methodToExecute)
            : this(methodToExecute, null)
        {
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteEvaluator == null)
                return true;

            return canExecuteEvaluator.Invoke();
        }

        public void Execute(object parameter)
        {
            methodToExecute.Invoke();
        }
    }
}