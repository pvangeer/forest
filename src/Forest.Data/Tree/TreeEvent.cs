using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Estimations;
using Forest.Data.Properties;

namespace Forest.Data.Tree
{
    public class TreeEvent : INotifyPropertyChanged
    {
        public TreeEvent(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public TreeEvent FailingEvent { get; set; }

        public TreeEvent PassingEvent { get; set; }

        public string Summary { get; set; }

        public string Information { get; set; }

        public string Discussion { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}