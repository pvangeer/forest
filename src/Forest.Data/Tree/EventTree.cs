using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Properties;

namespace Forest.Data.Tree
{
    public class EventTree : INotifyPropertyChanged
    {
        public TreeEvent MainTreeEvent { get; set; }

        public event EventHandler<TreeEventsChangedEventArgs> TreeEventsChanged;

        public virtual void OnTreeEventsChanged(TreeEventsChangedEventArgs e)
        {
            TreeEventsChanged?.Invoke(this, e);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}