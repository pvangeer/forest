using System;
using System.Collections.ObjectModel;
using Forest.Data.Estimations;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Experts;
using Forest.Data.Tree;

namespace Forest.Data
{
    public class ForestAnalysis : Entity
    {
        public ForestAnalysis()
        {
            var tree = new EventTree
            {
                Name = "Nieuw faalpad"
            };

            Name = "Nieuw project";
            AssessmentSection = "1-1";
            Description = "";
            ProjectInformation = "";
            ProjectLeader = new Person();
            EventTrees = new ObservableCollection<EventTree>
            {
                new EventTree
                {
                    Name = "Nieuw faalpad"
                }
            };
            ProbabilityEstimations = new ObservableCollection<ProbabilityEstimation>
            {
                new ProbabilityEstimationPerTreeEvent
                {
                    EventTree = tree,
                    Name = "Faalkansinschatting per gebeurtenis"
                }
            };
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AssessmentSection { get; set; }

        public string ProjectInformation { get; set; }

        public Person ProjectLeader { get; set; }

        public ObservableCollection<EventTree> EventTrees { get; }

        public ObservableCollection<ProbabilityEstimation> ProbabilityEstimations { get; }
    }
}