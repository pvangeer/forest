using System;
using System.Windows.Input;

namespace Forest.Visualization.Commands
{
    public class CanAlwaysExecuteActionCommand : ICommand
    {
        public Action<object> ExecuteAction;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ExecuteAction?.Invoke(parameter);
        }

    }
}