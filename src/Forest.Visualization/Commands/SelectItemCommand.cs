using System;
using System.Windows.Input;
using Forest.Gui;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.Commands
{
    public class SelectItemCommand : ICommand
    {
        private readonly ISelectable selectable;
        private readonly SelectionManager selectionManager;

        public SelectItemCommand(SelectionManager selectionManager, ISelectable selectable)
        {
            this.selectionManager = selectionManager;
            this.selectable = selectable;
        }

        public bool CanExecute(object parameter)
        {
            return selectable.CanSelect;
        }

        public void Execute(object o)
        {
            selectionManager.SetSelection(selectable.GetSelectableObject());
        }

        public event EventHandler CanExecuteChanged;
    }
}