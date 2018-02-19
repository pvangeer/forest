using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
{
    public class RemoveTreeEventCommand : EventTreeCommand
    {
        public RemoveTreeEventCommand(EventTreeViewModel selectedEventTree) : base(selectedEventTree)
        {
        }

        public override void Execute(object parameter)
        {
            SelectedEventTreeViewModel.RemoveTreeEvent();
        }
    }
}