using Forest.Data.Estimations;
using Forest.Data.Tree;

namespace Forest.Data.Services
{
    public static class TreeEventManipulations
    {
        public static void ChangeProbabilityEstimationType(TreeEvent treeEvent, ProbabilitySpecificationType type)
        {
            if (treeEvent.ProbabilitySpecificationType == type)
                return;

            treeEvent.ProbabilitySpecificationType = type;
            treeEvent.OnPropertyChanged(nameof(TreeEvent.ProbabilitySpecificationType));
        }
    }
}