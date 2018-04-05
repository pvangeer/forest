using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Estimations;
using StoryTree.Data.Properties;

namespace StoryTree.Data.Tree
{
    public class TreeEvent : INotifyPropertyChanged
    {
        public TreeEvent()
        {
            ProbabilitySpecificationType = ProbabilitySpecificationType.FixedValue;
            FixedProbability = (Probability)1;
            ClassesProbabilitySpecification = new ObservableCollection<ExpertClassEstimation>();
        }

        public string Name { get; set; }

        public TreeEvent FailingEvent { get; set; }

        public TreeEvent PassingEvent { get; set; }

        public string Summary { get; set; }

        public string Details { get; set; }

        public ObservableCollection<ExpertClassEstimation> ClassesProbabilitySpecification { get; }

        public Probability FixedProbability { get; set; }

        public FragilityCurve FixedFragilityCurve { get; set; }

        public ProbabilitySpecificationType ProbabilitySpecificationType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
