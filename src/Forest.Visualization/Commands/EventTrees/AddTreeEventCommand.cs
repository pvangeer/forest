using Forest.Data.Tree;
using Forest.Gui;

namespace Forest.Visualization.Commands.EventTrees
{
    public class AddTreeEventCommand : EventTreeCommand
    {
        public AddTreeEventCommand(ForestGui gui) : base(gui)
        {
        }

        public override bool CanExecute(object parameter)
        {
            if (Gui.SelectionManager.Selection is EventTree eventTree)
            { 
                var treeEventType = TreeEventType.MainEvent;
                if (parameter is TreeEventType treeEventTypeCasted)
                    treeEventType = treeEventTypeCasted;

                if (eventTree.MainTreeEvent == null && treeEventType != TreeEventType.Passing)
                    return true;

                if (Gui.SelectionManager.SelectedTreeEvent[eventTree] == null)
                    return false;

                return (treeEventType == TreeEventType.Failing &&
                        Gui.SelectionManager.SelectedTreeEvent[eventTree].FailingEvent == null) ||
                       (treeEventType == TreeEventType.Passing && Gui.SelectionManager.SelectedTreeEvent[eventTree].PassingEvent == null);
            }

            return false;
        }

        public override void Execute(object parameter)
        {
            var treeEventType = TreeEventType.Failing;
            if (parameter is TreeEventType treeEventTypeCasted)
                treeEventType = treeEventTypeCasted;

            var eventTree = Gui.SelectionManager.Selection as EventTree;
            if (eventTree == null)
                return;

            var newTreeEvent = ManipulationService.AddTreeEvent(eventTree,
                Gui.SelectionManager.SelectedTreeEvent[eventTree],
                treeEventType);
            Gui.SelectionManager.SelectTreeEvent(eventTree, newTreeEvent);
        }
    }
}