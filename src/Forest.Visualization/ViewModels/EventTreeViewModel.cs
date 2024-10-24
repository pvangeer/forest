using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Properties;
using Forest.Data.Services;
using Forest.Data.Tree;
using Forest.Gui.Components;

namespace Forest.Visualization.ViewModels
{
    public class EventTreeViewModel : INotifyPropertyChanged
    {
        private readonly AnalysisManipulationService analysisManipulationService;
        private readonly SelectionManager selectionManager;
        private EventTreeGraph graph;
        private bool isSelected;
        private TreeEventViewModel mainTreeEventViewModel;
        private readonly ObservableCollection<ProbabilityEstimation> estimations;

        public EventTreeViewModel()
        {
            var project = ForestAnalysisFactory.CreateStandardNewProject();
            analysisManipulationService = new AnalysisManipulationService(project);
            EventTree = project.EventTree;
            EventTree.PropertyChanged += EventTreePropertyChanged;
            EventTree.TreeEventsChanged += TreeEventsChanged;
        }

        public EventTreeViewModel([NotNull] EventTree eventTree, AnalysisManipulationService analysisManipulationService,
            SelectionManager selectionManager, ObservableCollection<ProbabilityEstimation> estimations)
        {
            EventTree = eventTree;
            this.selectionManager = selectionManager;
            this.estimations = estimations;
            this.analysisManipulationService = analysisManipulationService;
            eventTree.PropertyChanged += EventTreePropertyChanged;
            EventTree.TreeEventsChanged += TreeEventsChanged;

            SelectedTreeEvent = MainTreeEventViewModel;
        }

        private EventTree EventTree { get; }

        public EventTreeGraph Graph => CreateGraph();

        public TreeEventViewModel SelectedTreeEvent
        {
            get => FindTreeEventViewModel(selectionManager.SelectedTreeEvent);
            set
            {
                if (value != null)
                    selectionManager.SelectTreeEvent(value.TreeEvent);
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }

        public TreeEventViewModel MainTreeEventViewModel
        {
            get
            {
                if (mainTreeEventViewModel == null && EventTree.MainTreeEvent == null)
                    return null;

                return mainTreeEventViewModel ??
                       (mainTreeEventViewModel = new TreeEventViewModel(EventTree.MainTreeEvent, this, analysisManipulationService, estimations));
            }
        }

        public IEnumerable<TreeEventViewModel> AllTreeEvents => GetAllEventsRecursive(MainTreeEventViewModel);

        public EstimationSpecificationViewModelFactory EstimationSpecificationViewModelFactory { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private TreeEventViewModel FindTreeEventViewModel(TreeEvent treeEvent)
        {
            return treeEvent == null ? null : AllTreeEvents?.FirstOrDefault(e => e.TreeEvent == treeEvent);
        }

        private EventTreeGraph CreateGraph()
        {
            graph = new EventTreeGraph();

            DrawNode(MainTreeEventViewModel);

            return graph;
        }

        private void DrawNode(TreeEventViewModel treeEventViewModel, GraphVertex parent = null)
        {
            if (treeEventViewModel == null)
                return;

            var vertex = new GraphVertex(treeEventViewModel);
            graph.AddVertex(vertex);
            if (parent != null)
                graph.AddEdge(new TreeEventConnector(parent, vertex));

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

        private void TreeEventsChanged(object sender, TreeEventsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(AllTreeEvents));
            OnPropertyChanged(nameof(Graph));
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static IEnumerable<TreeEventViewModel> GetAllEventsRecursive(TreeEventViewModel treeEventViewModel)
        {
            var list = new[] { treeEventViewModel };

            if (treeEventViewModel == null)
                return list;

            if (treeEventViewModel.FailingEvent != null)
                list = list.Concat(GetAllEventsRecursive(treeEventViewModel.FailingEvent)).ToArray();

            if (treeEventViewModel.PassingEvent != null)
                list = list.Concat(GetAllEventsRecursive(treeEventViewModel.PassingEvent)).ToArray();

            return list;
        }

        public bool IsViewModelFor(EventTree argEventTree)
        {
            return argEventTree == EventTree;
        }
    }
}