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
                if (eventTree.MainTreeEvent == null)
                {
                    return true;
                }

                var treeEventType = TreeEventType.Failing;
                if (parameter is TreeEventType treeEventTypeCasted)
                    treeEventType = treeEventTypeCasted;

                return (treeEventType == TreeEventType.Failing &&
                        Gui.SelectionManager.SelectedTreeEvent.FailingEvent == null) ||
                       (treeEventType == TreeEventType.Passing && Gui.SelectionManager.SelectedTreeEvent.PassingEvent == null);
            }

            return false;
        }

        public override void Execute(object parameter)
        {
            var treeEventType = TreeEventType.Failing;
            if (parameter is TreeEventType treeEventTypeCasted)
                treeEventType = treeEventTypeCasted;

            var newTreeEvent = ManipulationService.AddTreeEvent(Gui.SelectionManager.Selection as EventTree, Gui.SelectionManager.SelectedTreeEvent, treeEventType);
            Gui.SelectionManager.SelectTreeEvent(newTreeEvent);
        }
    }
}