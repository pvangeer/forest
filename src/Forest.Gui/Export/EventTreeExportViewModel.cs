using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data;
using Forest.Gui.Annotations;

namespace Forest.Gui.Export
{
    public class EventTreeExportViewModel : INotifyPropertyChanged
    {
        public EventTreeExportViewModel(EventTree eventTree)
        {
            EventTree = eventTree;
        }

        public EventTree EventTree { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}