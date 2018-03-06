using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data;

namespace StoryTree.Gui.ViewModels
{
    public class ExpertViewModel : INotifyPropertyChanged
    {
        public ExpertViewModel()
        {
            Expert = new Expert {Name = "Naam"};
        }

        public ExpertViewModel(Expert expert)
        {
            Expert = expert;
        }

        public Expert Expert { get; }

        public string Name
        {
            get => Expert.Name;
            set => Expert.Name = value;
        }

        public string Email
        {
            get => Expert.Email;
            set => Expert.Email = value;
        }

        public string Expertise
        {
            get => Expert.Expertise;
            set => Expert.Expertise = value;
        }

        public string Organisation
        {
            get => Expert.Organisation;
            set => Expert.Organisation = value;
        }

        public string Telephone
        {
            get => Expert.Telephone;
            set => Expert.Telephone = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [Data.Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}