using System.Collections.ObjectModel;
using System.Linq;
using StoryTree.Data;

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
            EventTrees = new ObservableCollection<EventTreeViewModel>(project.EventTrees.Select(te => new EventTreeViewModel(te)));
        }

        private Project Project { get; }

        public ObservableCollection<EventTreeViewModel> EventTrees { get; }

        public ProjectViewModel Self => this;

        public string ProjectName => Project.Name;
    }
}