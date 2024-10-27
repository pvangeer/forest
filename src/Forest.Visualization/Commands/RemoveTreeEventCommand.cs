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
            return Gui.SelectionManager.SelectedTreeEvent != Gui.ForestAnalysis.EventTree.MainTreeEvent;
        }

        public override void Execute(object parameter)
        {
            var parent = ManipulationService.RemoveTreeEvent(Gui.ForestAnalysis.EventTree, Gui.SelectionManager.SelectedTreeEvent);
            Gui.SelectionManager.SelectTreeEvent(parent ?? Gui.ForestAnalysis.EventTree.MainTreeEvent);
        }
    }
}