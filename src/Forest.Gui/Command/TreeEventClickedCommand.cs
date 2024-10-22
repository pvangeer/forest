using System;
using System.Windows.Input;
using Forest.Gui.ViewModels;

namespace Forest.Gui.Command
{
    public class TreeEventClickedCommand : ICommand
    {
        public TreeEventClickedCommand(TreeEventViewModel treeEventViewModel)
        {
            TreeEventViewModel = treeEventViewModel;
        }

        public TreeEventViewModel TreeEventViewModel { get; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            TreeEventViewModel.Select();
        }

        public event EventHandler CanExecuteChanged;
    }
}