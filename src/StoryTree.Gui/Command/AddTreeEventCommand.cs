using StoryTree.Data.Services;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
{
    public class AddTreeEventCommand : EventTreeCommand
    {
        public AddTreeEventCommand(ProjectViewModel projectViewModel) : base(projectViewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return ProjectViewModel?.SelectedEventTree != null;
        }

        public override void Execute(object parameter)
        {
            var treeEventType = TreeEventType.Failing;
            if (parameter is TreeEventType treeEventTypeCasted)
            {
                treeEventType = treeEventTypeCasted;
            }
            ProjectViewModel?.SelectedEventTree.AddTreeEvent(ProjectViewModel?.SelectedTreeEvent, treeEventType);
        }
    }
}