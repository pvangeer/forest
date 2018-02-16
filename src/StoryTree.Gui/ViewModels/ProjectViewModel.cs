using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
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
            EventTrees = new ObservableCollection<EventTreeViewModel>(project.EventTrees.Select(te => new EventTreeViewModel(te)));
        }

        private Project Project { get; }

        public ObservableCollection<EventTreeViewModel> EventTrees { get; }

        public ProjectViewModel Self => this;

        public EventTreeViewModel SelectedEventTree => EventTrees?.FirstOrDefault();

        public Brush Color => new SolidColorBrush(Colors.BlanchedAlmond);

        public string ProjectName => Project.Name;
    }
}