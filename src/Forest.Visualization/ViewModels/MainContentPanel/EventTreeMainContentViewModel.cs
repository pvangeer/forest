using System.ComponentModel;
using Forest.Data;
using Forest.Data.Services;
using Forest.Data.Tree;
using Forest.Gui;

namespace Forest.Visualization.ViewModels.MainContentPanel
{
    public class EventTreeMainContentViewModel : Entity
    {
        private readonly EventTree eventTree;
        private EventTreeGraph graph;
        private readonly ViewModelFactory viewModelFactory;

        public EventTreeMainContentViewModel(EventTree eventTree, ForestGui gui)
        {
            this.eventTree = eventTree;
            this.eventTree.PropertyChanged += EventTreePropertyChanged;
            this.eventTree.TreeEventsChanged += TreeEventsChanged;
            this.viewModelFactory = new ViewModelFactory(gui);
        }

        public EventTreeGraph Graph => CreateGraph();

        private void EventTreePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EventTree.MainTreeEvent):
                    OnPropertyChanged(nameof(Graph));
                    break;
            }
        }

        private void TreeEventsChanged(object sender, TreeEventsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Graph));
        }

        private EventTreeGraph CreateGraph()
        {
            graph = new EventTreeGraph();

            DrawNode(viewModelFactory.CreateTreeEventViewModel(eventTree.MainTreeEvent));

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
    }
}
