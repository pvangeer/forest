using StoryTree.Data;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class ProjectViewModel
    {
        public ProjectViewModel()
        {

        }

        public ProjectViewModel(Project project)
        {
            Project = project;
        }

        private Project Project { get; }

        public TreeEventViewModel MainTreeEventViewModel => Project == null ? null : new TreeEventViewModel(Project.MainTreeEvent);
    }
}