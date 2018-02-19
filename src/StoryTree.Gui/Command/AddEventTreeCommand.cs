using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
{
    public class AddEventTreeCommand : ProjectViewModelCommand
    {
        public AddEventTreeCommand(ProjectViewModel projectViewModel) : base(projectViewModel)
        {
        }

        public override void Execute(object parameter)
        {
            ProjectViewModel.AddNewEventTree();
        }
    }
}