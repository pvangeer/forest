using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Properties;
using Forest.Data.Services;

namespace Forest.Data.Tree
{
    public class EventTree : INotifyPropertyChanged
    {
        public TreeEvent MainTreeEvent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<TreeEventsChangedEventArgs> TreeEventsChanged;

        public virtual void OnTreeEventsChanged(TreeEventsChangedEventArgs e)
        {
            TreeEventsChanged?.Invoke(this, e);
        }

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}