using Forest.Data.Tree;
using Forest.Gui;

namespace Forest.Visualization.Commands
{
    public class RemoveTreeEventCommand : EventTreeCommand
    {
        public RemoveTreeEventCommand(ForestGui gui) : base(gui)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return Gui.SelectionManager.Selection is EventTree && Gui.SelectionManager.SelectedTreeEvent != ((EventTree)Gui.SelectionManager.Selection).MainTreeEvent;
        }

        public override void Execute(object parameter)
        {
            var eventTree = (EventTree)Gui.SelectionManager.Selection;
            var parent = ManipulationService.RemoveTreeEvent(eventTree, Gui.SelectionManager.SelectedTreeEvent);
            Gui.SelectionManager.SelectTreeEvent(parent ?? eventTree.MainTreeEvent);
        }
    }
}