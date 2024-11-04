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
            SelectedTreeEvent = gui.ForestAnalysis.EventTrees.FirstOrDefault()?.MainTreeEvent;
            Selection = gui.ForestAnalysis.EventTrees.FirstOrDefault();
        }

        public TreeEvent SelectedTreeEvent { get; private set; }

        public object Selection { get; private set; }

        public void SelectTreeEvent(TreeEvent treeEvent)
        {
            SelectedTreeEvent = treeEvent;
            OnPropertyChanged(nameof(SelectedTreeEvent));
        }

        public void SetSelection(object selection)
        {
            Selection = selection;
            OnPropertyChanged(nameof(Selection));
        }
    }
}