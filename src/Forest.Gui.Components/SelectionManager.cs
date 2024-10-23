using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Tree;

namespace Forest.Gui.Components
{
    public class SelectionManager : INotifyPropertyChanged
    {
        private readonly ForestGui gui;

        public SelectionManager(ForestGui gui)
        {
            this.gui = gui;
            SelectedTreeEvent = gui.EventTreeProject.EventTree.MainTreeEvent;
        }

        public void SelectTreeEvent(TreeEvent treeEvent)
        {
            SelectedTreeEvent = treeEvent;
            OnPropertyChanged(nameof(SelectedTreeEvent));
        }

        public TreeEvent SelectedTreeEvent { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

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