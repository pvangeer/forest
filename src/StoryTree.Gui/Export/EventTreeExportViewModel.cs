using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data;
using StoryTree.Gui.Annotations;

namespace StoryTree.Gui.Export
{
    public class EventTreeExportViewModel : INotifyPropertyChanged
    {
        private EventTree eventTreeViewModel;
        private bool isChecked;

        public EventTreeExportViewModel(EventTree eventTreeViewModel)
        {
            this.eventTreeViewModel = eventTreeViewModel;
            IsChecked = eventTreeViewModel.NeedsSpecification;
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

        public string Name => eventTreeViewModel.Name;

        public EventTree EventTreeViewModel => eventTreeViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}