using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data;
using StoryTree.Gui.Annotations;

namespace StoryTree.Gui.Export
{
    public class ElicitationFormsExportViewModel : INotifyPropertyChanged
    {
        private Expert expert;
        private bool isChecked;

        public ElicitationFormsExportViewModel(Expert expert)
        {
            this.expert = expert;
            IsChecked = true;
        }

        public string Name => expert.Name;

        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public Expert Expert => expert;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}