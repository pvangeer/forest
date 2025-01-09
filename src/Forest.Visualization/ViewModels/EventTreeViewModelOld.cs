using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Estimations.PerTreeEvent.Experts;
using Forest.Data.Properties;
using Forest.Data.Services;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.EventTreeEditing;

namespace Forest.Visualization.ViewModels
{
    public class EventTreeViewModelOld : Entity
    {
        private readonly AnalysisManipulationService analysisManipulationService;
        private readonly ObservableCollection<ProbabilityEstimation> estimations;
        private readonly SelectionManager selectionManager;
        private EventTreeGraph graph;
        private bool isSelected;
        private TreeEventViewModelOld mainTreeEventViewModel;

        public EventTreeViewModelOld(EventTree eventTree)
        {
            var project = ForestAnalysisFactory.CreateStandardNewAnalysis();
            analysisManipulationService = new AnalysisManipulationService(project);
            EventTree = eventTree;
            EventTree.PropertyChanged += EventTreePropertyChanged;
            EventTree.TreeEventsChanged += TreeEventsChanged;
        }

        public EventTreeViewModelOld([NotNull] EventTree eventTree,
            AnalysisManipulationService analysisManipulationService,
            SelectionManager selectionManager,
            ObservableCollection<ProbabilityEstimation> estimations)
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

        public TreeEventViewModelOld SelectedTreeEvent
        {
            get => FindTreeEventViewModel(selectionManager.SelectedTreeEvent[EventTree]);
            set
            {
                if (value != null)
                    selectionManager.SelectTreeEvent(EventTree, value.TreeEvent);
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

        public TreeEventViewModelOld MainTreeEventViewModel
        {
            get
            {
                if (mainTreeEventViewModel == null && EventTree.MainTreeEvent == null)
                    return null;

                return mainTreeEventViewModel ??
                       (mainTreeEventViewModel =
                           new TreeEventViewModelOld(EventTree.MainTreeEvent, this, analysisManipulationService, estimations));
            }
        }

        public IEnumerable<TreeEventViewModelOld> AllTreeEvents => GetAllEventsRecursive(MainTreeEventViewModel);

        public bool SelectedEstimationHasExperts()
        {
            var selectedEstimation = selectionManager.Selection as ProbabilityEstimationPerTreeEvent;
            return selectedEstimation != null && selectedEstimation.Experts.Any();
        }

        public ProbabilityEstimationPerTreeEvent GetSelectedEstimationPerTreeEvent()
        {
            return selectionManager.Selection as ProbabilityEstimationPerTreeEvent;
        }

        private TreeEventViewModelOld FindTreeEventViewModel(TreeEvent treeEvent)
        {
            return treeEvent == null ? null : AllTreeEvents?.FirstOrDefault(e => e.TreeEvent == treeEvent);
        }

        private EventTreeGraph CreateGraph()
        {
            graph = new EventTreeGraph();

            DrawNode(MainTreeEventViewModel);

            return graph;
        }

        private void DrawNode(TreeEventViewModelOld treeEventViewModel, GraphVertex parent = null)
        {
            if (treeEventViewModel == null)
                return;

            /*var vertex = new GraphVertex(treeEventViewModel);
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
            }*/
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

        private static IEnumerable<TreeEventViewModelOld> GetAllEventsRecursive(TreeEventViewModelOld treeEventViewModel)
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