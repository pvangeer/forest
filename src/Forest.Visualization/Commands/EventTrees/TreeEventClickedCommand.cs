using System;
using System.Windows.Input;
using Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.EventTreeEditing;

namespace Forest.Visualization.Commands.EventTrees
{
    public class TreeEventClickedCommand : ICommand
    {
        private readonly TreeEventViewModel treeEventViewModel;

        public TreeEventClickedCommand(TreeEventViewModel treeEventViewModel)
        {
            this.treeEventViewModel = treeEventViewModel;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            treeEventViewModel.Select();
        }

        public event EventHandler CanExecuteChanged;
    }
}