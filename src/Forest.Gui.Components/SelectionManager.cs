using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Tree;

namespace Forest.Gui.Components
{
    public class SelectionManager : INotifyPropertyChanged
    {
        private readonly ForestGui gui;

        public SelectionManager(ForestGui gui)
        {
            this.gui = gui;
            SelectedTreeEvent = gui.ForestAnalysis.EventTree.MainTreeEvent;
            // TODO: Change this once a selection can be made (and estimations can be added/removed)
            Selection = gui.ForestAnalysis.ProbabilityEstimations.OfType<ProbabilityEstimationPerTreeEvent>().First();
        }

        public TreeEvent SelectedTreeEvent { get; private set; }

        // TODO: Maybe merge? Or keep dictionary of selected tree event per eventtree?
        public object Selection { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetSelection(object selection)
        {
            Selection = selection;
            OnPropertyChanged(nameof(Selection));
        }

        public void SelectTreeEvent(TreeEvent treeEvent)
        {
            SelectedTreeEvent = treeEvent;
            OnPropertyChanged(nameof(SelectedTreeEvent));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}