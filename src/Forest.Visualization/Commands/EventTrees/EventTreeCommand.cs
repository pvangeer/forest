using System;
using System.ComponentModel;
using System.Windows.Input;
using Forest.Data.Services;
using Forest.Gui;

namespace Forest.Visualization.Commands.EventTrees
{
    public abstract class EventTreeCommand : ICommand
    {
        protected readonly ForestGui Gui;
        protected readonly AnalysisManipulationService ManipulationService;

        protected EventTreeCommand(ForestGui gui)
        {
            this.Gui = gui;
            if (this.Gui != null)
            {
                gui.SelectionManager.PropertyChanged += SelectionManagerPropertyChanged;
                ManipulationService = new AnalysisManipulationService(gui.ForestAnalysis);
            }

        }

        public virtual bool CanExecute(object parameter)
        {
            return Gui.SelectionManager.SelectedTreeEvent != null;
        }

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;

        private void SelectionManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectionManager.SelectedTreeEvent):
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