using StoryTree.Data.Services;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
{
    public class RemoveTreeEventCommand : EventTreeCommand
    {
        public RemoveTreeEventCommand(ProjectViewModel projectViewModel) : base(projectViewModel)
        {
        }

        public override void Execute(object parameter)
        {
            ProjectViewModel.SelectedEventTreeFiltered.RemoveTreeEvent(ProjectViewModel.SelectedTreeEvent, TreeEventType.Failing);
        }
    }
}