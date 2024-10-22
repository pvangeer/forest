using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Properties;

namespace Forest.Data
{
    public class Person : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}