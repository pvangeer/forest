using System;
using System.ComponentModel;
using System.Windows.Input;
using Forest.Data.Services;
using Forest.Data.Tree;
using Forest.Gui;

namespace Forest.Visualization.Commands.EventTrees
{
    public abstract class EventTreeCommand : ICommand
    {
        protected readonly ForestGui Gui;
        protected readonly AnalysisManipulationService ManipulationService;

        protected EventTreeCommand(ForestGui gui)
        {
            Gui = gui;
            if (Gui != null)
            {
                gui.SelectionManager.SelectedTreeEventChanged += SelectedTreeEventChanged;
                gui.SelectionManager.PropertyChanged += SelectionManagerPropertyChanged;
                ManipulationService = new AnalysisManipulationService(gui.ForestAnalysis);
            }
        }

        public virtual bool CanExecute(object parameter)
        {
            return Gui.SelectionManager.Selection is EventTree eventTree && Gui.SelectionManager.SelectedTreeEvent[eventTree] != null;
        }

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;

        private void SelectedTreeEventChanged(object sender, EventArgs eventArgs)
        {
            FireCanExecuteChanged();
        }

        private void SelectionManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectionManager.Selection):
                    FireCanExecuteChanged();
                    break;
            }
        }

        private void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }
}