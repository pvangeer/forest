using System.Collections.ObjectModel;
using Forest.Data.Estimations;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Estimations.PerTreeEvent.Experts;
using Forest.Data.Tree;

namespace Forest.Data
{
    public class ForestAnalysis : Entity
    {
        public ForestAnalysis()
        {
            ProjectLeader = new Person();
            EventTrees = new ObservableCollection<EventTree>();
            ProbabilityEstimationsPerTreeEvent = new ObservableCollection<ProbabilityEstimationPerTreeEvent>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AssessmentSection { get; set; }

        public string ProjectInformation { get; set; }

        public Person ProjectLeader { get; }

        public ObservableCollection<EventTree> EventTrees { get; }

        public ObservableCollection<ProbabilityEstimationPerTreeEvent> ProbabilityEstimationsPerTreeEvent { get; }

        public ObservableCollection<ProbabilityEstimation> ProbabilityEstimationsPerCondition { get; }
    }
}