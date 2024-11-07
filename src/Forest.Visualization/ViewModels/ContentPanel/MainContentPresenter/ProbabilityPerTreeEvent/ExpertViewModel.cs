using Forest.Data;
using Forest.Data.Estimations.PerTreeEvent.Experts;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class ExpertViewModel : Entity
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
    }
}