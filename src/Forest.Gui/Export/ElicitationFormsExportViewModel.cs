using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data;
using Forest.Gui.Annotations;

namespace Forest.Gui.Export
{
    public class ElicitationFormsExportViewModel : INotifyPropertyChanged
    {
        private bool isChecked;

        public ElicitationFormsExportViewModel(Expert expert)
        {
            Expert = expert;
            IsChecked = true;
        }

        public string Name => Expert.Name;

        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged();
            }
        }

        public Expert Expert { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}