using System.ComponentModel;
using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Tree;

namespace Forest.Gui.ViewModels
{
    public class EstimationSpecificationViewModelFactory
    {
        public EstimationSpecificationViewModelFactory(EventTreeProject eventTreeProject)
        {
            EventTreeProject = eventTreeProject;
        }

        public EventTreeProject EventTreeProject { get; }

        public ProbabilitySpecificationViewModelBase CreateViewModel(TreeEvent treeEvent)
        {
            switch (treeEvent.ProbabilitySpecificationType)
            {
                case ProbabilitySpecificationType.Classes:
                    return new ClassesProbabilitySpecificationViewModel(treeEvent, EventTreeProject);
                case ProbabilitySpecificationType.FixedValue:
                    return new FixedProbabilitySpecificationViewModel(treeEvent);
                case ProbabilitySpecificationType.FixedFrequency:
                    return new FixedFragilityCurveSpecificationViewModel(treeEvent, EventTreeProject);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}