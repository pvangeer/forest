using Forest.Data.Tree;
using Forest.Gui;

namespace Forest.Visualization.Commands
{
    public class AddTreeEventCommand : EventTreeCommand
    {
        public AddTreeEventCommand(ForestGui gui) : base(gui)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var treeEventType = TreeEventType.Failing;
            if (parameter is TreeEventType treeEventTypeCasted)
                treeEventType = treeEventTypeCasted;

            var newTreeEvent = ManipulationService.AddTreeEvent(Gui.ForestAnalysis.EventTree, Gui.SelectionManager.SelectedTreeEvent, treeEventType);
            Gui.SelectionManager.SelectTreeEvent(newTreeEvent);
        }
    }
}