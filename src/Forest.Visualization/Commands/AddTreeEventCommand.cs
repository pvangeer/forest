using Forest.Data.Tree;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization.Commands
{
    public class AddTreeEventCommand : EventTreeCommand
    {
        public AddTreeEventCommand(RibbonViewModel viewModel) : base(viewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return ViewModel != null;
        }

        public override void Execute(object parameter)
        {
            var treeEventType = TreeEventType.Failing;
            if (parameter is TreeEventType treeEventTypeCasted)
                treeEventType = treeEventTypeCasted;
            ViewModel?.AddTreeEvent(treeEventType);
        }
    }
}