using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using StoryTree.Data;
using StoryTree.Data.Properties;
using StoryTree.Data.Services;

namespace StoryTree.Gui.ViewModels
{
    public partial class EventTreeViewModel : INotifyPropertyChanged
    {
        private TreeEventViewModel selectedTreeEvent;
        private TreeEventViewModel mainTreeEventViewModel;
        private bool isSelected;
        private EventTreeGraph graph;
        private readonly ProjectManipulationService projectManipulationService;

        private EventTree EventTree { get; }

        public EventTreeViewModel()
        {
            var project = new Project();
            projectManipulationService = new ProjectManipulationService(project);
            EventTree = project.EventTree;
            EventTree.PropertyChanged += EventTreePropertyChanged;
        }

        public EventTreeViewModel([NotNull]EventTree eventTree, ProjectManipulationService projectManipulationService)
        {
            EventTree = eventTree;
            SelectedTreeEvent = MainTreeEventViewModel;
            this.projectManipulationService = projectManipulationService;
            eventTree.PropertyChanged += EventTreePropertyChanged;
        }

        public EventTreeGraph Graph => CreateGraph();

        private EventTreeGraph CreateGraph()
        {
            graph = new EventTreeGraph();

            DrawNode(MainTreeEventViewModel);

            return graph;
        }

        private void DrawNode(TreeEventViewModel treeEventViewModel, GraphVertex parent = null)
        {
            if (treeEventViewModel == null)
            {
                return;
            }

            var vertex = new GraphVertex(treeEventViewModel);
            graph.AddVertex(vertex);
            if (parent != null)
            {
                graph.AddEdge(new TreeEventConnector(parent, vertex));
            }

            if (treeEventViewModel.FailingEvent != null)
            {
                DrawNode(treeEventViewModel.FailingEvent, vertex);
            }
            else
            {
                var lastVertex = new GraphVertex(false);
                graph.AddVertex(lastVertex);
                graph.AddEdge(new TreeEventConnector(vertex, lastVertex));
            }

            if (treeEventViewModel.PassingEvent != null)
            {
                DrawNode(treeEventViewModel.PassingEvent, vertex);
            }
            else
            {
                var lastVertex = new GraphVertex(true);
                graph.AddVertex(lastVertex);
                graph.AddEdge(new TreeEventConnector(vertex, lastVertex));
            }
        }

        private void EventTreePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EventTree.MainTreeEvent):
                    mainTreeEventViewModel = null;
                    OnPropertyChanged(nameof(MainTreeEventViewModel));
                    OnPropertyChanged(nameof(Graph));
                    break;
            }
        }

        public TreeEventViewModel SelectedTreeEvent
        {
            get => selectedTreeEvent;
            set
            {
                selectedTreeEvent = value;
                MainTreeEventViewModel?.FireSelectedStateChangeRecursive();
                OnPropertyChanged(nameof(SelectedTreeEvent));
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public TreeEventViewModel MainTreeEventViewModel
        {
            get
            {
                if (mainTreeEventViewModel == null && EventTree.MainTreeEvent == null)
                {
                    return null;
                }

                return mainTreeEventViewModel ??
                       (mainTreeEventViewModel = new TreeEventViewModel(EventTree.MainTreeEvent, this, projectManipulationService));
            }
        }

        public IEnumerable<TreeEventViewModel> AllTreeEvents => GetAllEventsRecursive(MainTreeEventViewModel);

        public EstimationSpecificationViewModelFactory EstimationSpecificationViewModelFactory { get; set; }

        public bool IsViewModelFor(EventTree eventTree)
        {
            return Equals(EventTree, eventTree);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddTreeEvent(TreeEventViewModel treeEventViewModel, TreeEventType treeEventType)
        {
            projectManipulationService.AddTreeEvent(EventTree,treeEventViewModel?.TreeEvent,treeEventType);
            SelectedTreeEvent = treeEventViewModel == null ? MainTreeEventViewModel : treeEventType == TreeEventType.Failing ? treeEventViewModel.FailingEvent : treeEventViewModel.PassingEvent;
            OnPropertyChanged(nameof(AllTreeEvents));
            OnPropertyChanged(nameof(Graph));
        }

        public void RemoveTreeEvent(TreeEventViewModel treeEventViewModel, TreeEventType eventType)
        {
            var parent = projectManipulationService.RemoveTreeEvent(EventTree, treeEventViewModel.TreeEvent);
            SelectedTreeEvent = parent == null ? MainTreeEventViewModel : FindLastEventViewModel(MainTreeEventViewModel, eventType);
            OnPropertyChanged(nameof(AllTreeEvents));
            OnPropertyChanged(nameof(Graph));
        }

        private static IEnumerable<TreeEventViewModel> GetAllEventsRecursive(TreeEventViewModel treeEventViewModel)
        {
            var list = new[]{treeEventViewModel};

            if (treeEventViewModel == null)
            {
                return list;
            }

            if (treeEventViewModel.FailingEvent != null)
            {
                list = list.Concat(GetAllEventsRecursive(treeEventViewModel.FailingEvent)).ToArray();
            }

            if (treeEventViewModel.PassingEvent != null)
            {
                list = list.Concat(GetAllEventsRecursive(treeEventViewModel.PassingEvent)).ToArray();
            }

            return list;
        }

        private static TreeEventViewModel FindLastEventViewModel(TreeEventViewModel mainTreeEventViewModel, TreeEventType type)
        {
            switch (type)
            {
                case TreeEventType.Failing:
                    if (mainTreeEventViewModel.FailingEvent == null)
                    {
                        return mainTreeEventViewModel;
                    }

                    return FindLastEventViewModel(mainTreeEventViewModel.FailingEvent, type);
                case TreeEventType.Passing:
                    if (mainTreeEventViewModel.PassingEvent == null)
                    {
                        return mainTreeEventViewModel;
                    }

                    return FindLastEventViewModel(mainTreeEventViewModel.PassingEvent, type);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}