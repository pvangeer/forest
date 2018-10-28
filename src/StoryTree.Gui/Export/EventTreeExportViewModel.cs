using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data;
using StoryTree.Gui.Annotations;

namespace StoryTree.Gui.Export
{
    public class EventTreeExportViewModel : INotifyPropertyChanged
    {
        private EventTree eventTree;
        private bool isChecked;

        public EventTreeExportViewModel(EventTree eventTree)
        {
            this.eventTree = eventTree;
            IsChecked = eventTree.NeedsSpecification;
        }

        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public string Name => eventTree.Name;

        public EventTree EventTree => eventTree;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}