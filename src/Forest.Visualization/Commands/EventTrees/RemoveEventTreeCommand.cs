using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Forest.Data.Services;
using Forest.Data.Tree;
using Forest.Gui;

namespace Forest.Visualization.Commands.EventTrees
{
    public class RemoveEventTreeCommand : ICommand
    {
        private readonly ForestGui gui;
        private EventTree eventTree;

        public RemoveEventTreeCommand(ForestGui gui, EventTree eventTree)
        {
            this.eventTree = eventTree;
            if (eventTree == null)
            {
                this.eventTree = gui?.SelectionManager.Selection as EventTree;
                if (gui != null)
                {
                    gui.SelectionManager.PropertyChanged += SelectionManagerPropertyChanged;
                }
            }
            this.gui = gui;
            if (gui != null)
            {
                this.gui.ForestAnalysis.EventTrees.CollectionChanged += EventTreeCollectionChanged;
            }
        }

        private void SelectionManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectionManager.Selection):
                    this.eventTree = gui?.SelectionManager.Selection as EventTree;
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged.Invoke(this, EventArgs.Empty);
                    }
                    break;
            }
        }

        private void EventTreeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged.Invoke(this, e);
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.eventTree != null;
        }

        public void Execute(object parameter)
        {
            var index = gui.ForestAnalysis.EventTrees.IndexOf(eventTree);
            
            var service = new AnalysisManipulationService(gui.ForestAnalysis);
            service.RemoveEventTree(eventTree);
            
            if (gui.SelectionManager.Selection == eventTree)
            {
                if (index >= gui.ForestAnalysis.EventTrees.Count)
                {
                    index = gui.ForestAnalysis.EventTrees.Count - 1;
                }

                gui.SelectionManager.SetSelection(index > -1 ? gui.ForestAnalysis.EventTrees.ElementAt(index) : null);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}