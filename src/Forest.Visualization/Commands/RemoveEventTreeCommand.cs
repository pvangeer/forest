using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Forest.Data.Services;
using Forest.Data.Tree;
using Forest.Gui;

namespace Forest.Visualization.Commands
{
    public class RemoveEventTreeCommand : ICommand
    {
        private readonly ForestGui gui;
        private readonly EventTree eventTree;

        public RemoveEventTreeCommand(ForestGui gui, EventTree eventTree)
        {
            this.eventTree = eventTree;
            this.gui = gui;
            this.gui.ForestAnalysis.EventTrees.CollectionChanged += EventTreeCollectionChanged;
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
            return true;
        }

        public void Execute(object parameter)
        {
            var service = new AnalysisManipulationService(gui.ForestAnalysis);
            service.RemoveEventTree(eventTree);
            var index = gui.ForestAnalysis.EventTrees.IndexOf(eventTree);
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