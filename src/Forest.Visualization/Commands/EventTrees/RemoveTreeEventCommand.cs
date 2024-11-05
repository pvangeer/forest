using Forest.Data.Tree;
using Forest.Gui;

namespace Forest.Visualization.Commands.EventTrees
{
    public class RemoveTreeEventCommand : EventTreeCommand
    {
        public RemoveTreeEventCommand(ForestGui gui) : base(gui)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return Gui.SelectionManager.Selection is EventTree;
        }

        public override void Execute(object parameter)
        {
            var eventTree = (EventTree)Gui.SelectionManager.Selection;
            var parent = ManipulationService.RemoveTreeEvent(eventTree, Gui.SelectionManager.SelectedTreeEvent[eventTree]);
            Gui.SelectionManager.SelectTreeEvent(eventTree, parent ?? eventTree.MainTreeEvent);
        }
    }
}