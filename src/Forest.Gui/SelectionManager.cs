using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Forest.Data;
using Forest.Data.Tree;

namespace Forest.Gui
{
    public class SelectionManager : Entity
    {
        private readonly ForestGui gui;

        public SelectionManager(ForestGui gui)
        {
            this.gui = gui;
            gui.PropertyChanged += GuiPropertyChanged;
            InitializeSelectionManager();
        }

        public Dictionary<EventTree, TreeEvent> SelectedTreeEvent { get; private set; }

        public object Selection { get; private set; }

        private void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.ForestAnalysis):
                    InitializeSelectionManager();
                    break;
            }
        }

        private void InitializeSelectionManager()
        {
            SelectedTreeEvent = gui.ForestAnalysis.EventTrees.ToDictionary(et => et, et => et.MainTreeEvent);
            Selection = gui.ForestAnalysis.EventTrees.FirstOrDefault();
            gui.ForestAnalysis.EventTrees.CollectionChanged += EventTreesCollectionChanged;
        }

        private void EventTreesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var eventTree in e.NewItems.OfType<EventTree>())
                        SelectedTreeEvent[eventTree] = eventTree.MainTreeEvent;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var eventTree in e.OldItems.OfType<EventTree>())
                    {
                        if (SelectedTreeEvent.ContainsKey(eventTree))
                            SelectedTreeEvent.Remove(eventTree);
                    }

                    break;
            }
        }

        public event EventHandler<EventArgs> SelectedTreeEventChanged;

        public void SelectTreeEvent(EventTree eventTree, TreeEvent treeEvent)
        {
            SelectedTreeEvent[eventTree] = treeEvent;
            SelectedTreeEventChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetSelection(object selection)
        {
            Selection = selection;
            OnPropertyChanged(nameof(Selection));
        }

        public void ClearSelection()
        {
            Selection = null;
            SelectedTreeEvent.Clear();
            SelectedTreeEventChanged?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(Selection));
        }

        public TreeEvent GetSelectedTreeEvent(EventTree eventTree)
        {
            return SelectedTreeEvent.ContainsKey(eventTree) ? SelectedTreeEvent[eventTree] : null;
        }
    }
}