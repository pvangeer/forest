using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Experts;
using Forest.Data.Properties;

namespace Forest.Visualization.ViewModels
{
    public class ExpertViewModel : INotifyPropertyChanged
    {
        public ExpertViewModel()
        {
            Expert = new Expert { Name = "Naam" };
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

        public string Organization
        {
            get => Expert.Organization;
            set => Expert.Organization = value;
        }

        public string Telephone
        {
            get => Expert.Telephone;
            set => Expert.Telephone = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}