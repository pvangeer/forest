using System.Collections.ObjectModel;
using Forest.Data.Estimations;
using Forest.Data.Experts;
using Forest.Data.Tree;

namespace Forest.Data
{
    public class ForestAnalysis : Entity
    {
        public ForestAnalysis()
        {
            ProjectLeader = new Person();
            EventTrees = new ObservableCollection<EventTree>();
            ProbabilityEstimations = new ObservableCollection<ProbabilityEstimation>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AssessmentSection { get; set; }

        public string ProjectInformation { get; set; }

        public Person ProjectLeader { get; }

        public ObservableCollection<EventTree> EventTrees { get; }

        public ObservableCollection<ProbabilityEstimation> ProbabilityEstimations { get; }
    }
}