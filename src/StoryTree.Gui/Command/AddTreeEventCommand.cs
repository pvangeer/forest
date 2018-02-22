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
            return ProjectViewModel?.SelectedEventTree != null && (ProjectViewModel?.SelectedEventTree.MainTreeEventViewModel == null || ProjectViewModel?.SelectedEventTree.SelectedTreeEvent != null);
        }

        public override void Execute(object parameter)
        {
            ProjectViewModel?.SelectedEventTree.AddTreeEvent(ProjectViewModel?.SelectedTreeEvent);
        }
    }
}