using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Estimations;
using Forest.Data.Experts;
using Forest.Data.Hydrodynamics;
using Forest.Data.Properties;
using Forest.Data.Tree;

namespace Forest.Data
{
    public class ForestAnalysis : INotifyPropertyChanged
    {
        private EventTree eventTree;

        public ForestAnalysis()
        {
            var tree = new EventTree();

            Name = "Nieuw project";
            AssessmentSection = "1-1";
            Description = "";
            ProjectInformation = "";
            ProjectLeader = new Person();
            EventTree = tree;
            HydrodynamicConditions = new ObservableCollection<HydrodynamicCondition>();
            ProbabilityEstimations = new ObservableCollection<ProbabilityEstimation>
            {
                new ProbabilityEstimationPerTreeEvent
                {
                    EventTree = tree
                }
            };
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AssessmentSection { get; set; }

        public string ProjectInformation { get; set; }

        public Person ProjectLeader { get; set; }

        // TODO: Make this a list
        public EventTree EventTree
        {
            get => eventTree;
            set
            {
                eventTree = value;
                if (eventTree == null)
                    throw new ArgumentNullException();
            }
        }

        public ObservableCollection<ProbabilityEstimation> ProbabilityEstimations { get; }

        // TODO: Move to ProbabilityEstimationPerTreeEvent
        public ObservableCollection<HydrodynamicCondition> HydrodynamicConditions { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}