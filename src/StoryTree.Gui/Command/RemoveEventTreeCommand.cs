using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.Command
{
    public class RemoveEventTreeCommand : ProjectViewModelCommand
    {
        public RemoveEventTreeCommand(ProjectViewModel projectViewModel) : base(projectViewModel)
        {
        }

        public override void Execute(object parameter)
        {
            ProjectViewModel.RemoveSelectedEventTree();
        }
    }
}