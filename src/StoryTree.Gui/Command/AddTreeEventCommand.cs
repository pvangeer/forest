using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
{
    public class AddTreeEventCommand : EventTreeCommand
    {
        public AddTreeEventCommand(EventTreeViewModel selectedEventTreeViewModel) : base(selectedEventTreeViewModel)
        {
        }

        public override void Execute(object parameter)
        {
            SelectedEventTreeViewModel.AddTreeEvent();
        }
    }
}