using System;
using System.ComponentModel;
using Forest.Data;
using Forest.Data.Services;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.DataTemplates.MainContentPresenter.EventTree;

namespace Forest.Visualization.ViewModels.MainContentPanel
{
    public class EventTreeMainContentViewModel : Entity
    {
        private readonly EventTree eventTree;
        private readonly ViewModelFactory viewModelFactory;
        private EventTreeGraph graph;
        private readonly ForestGui gui;

        public EventTreeMainContentViewModel(EventTree eventTree, ForestGui gui)
        {
            this.gui = gui;
            gui.PropertyChanged += GuiPropertyChanged;
            this.eventTree = eventTree;
            this.eventTree.PropertyChanged += EventTreePropertyChanged;
            this.eventTree.TreeEventsChanged += TreeEventsChanged;
            this.gui.SelectionManager.SelectedTreeEventChanged += SelectedTreeEventChanged;
            viewModelFactory = new ViewModelFactory(gui);
        }

        public EventTreeGraphLayout EventTreeGraphLayout => new EventTreeGraphLayout { Graph = CreateGraph() };

        public bool IsDetailsPanelVisible => gui.IsShowDetailsPanel;

        public TreeEventViewModel SelectedTreeEventViewModel
        {
            get
            {
                var selectedTreeEvent = gui.SelectionManager.GetSelectedTreeEvent(eventTree);
                return selectedTreeEvent != null ? viewModelFactory.CreateTreeEventViewModel(selectedTreeEvent, eventTree) : null;
            }
        }

        private void EventTreePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EventTree.MainTreeEvent):
                    OnPropertyChanged(nameof(EventTreeGraphLayout));
                    break;
            }
        }

        private void SelectedTreeEventChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(SelectedTreeEventViewModel));
        }

        private void TreeEventsChanged(object sender, TreeEventsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(EventTreeGraphLayout));
        }

        private EventTreeGraph CreateGraph()
        {
            graph = new EventTreeGraph();

            DrawNode(viewModelFactory.CreateTreeEventViewModel(eventTree.MainTreeEvent, eventTree));

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

        private void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.IsShowDetailsPanel):
                    OnPropertyChanged(nameof(IsDetailsPanelVisible));
                    break;
            }
        }
    }
}