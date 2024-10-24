using Forest.Data.Estimations;

namespace Forest.Data.Tree
{
    public static class TreeEventManipulations
    {
        public static void ChangeProbabilityEstimationType(TreeEventProbabilityEstimation estimation, ProbabilitySpecificationType type)
        {
            if (estimation.ProbabilitySpecificationType == type)
                return;

            estimation.ProbabilitySpecificationType = type;
            estimation.OnPropertyChanged(nameof(TreeEventProbabilityEstimation.ProbabilitySpecificationType));
        }
    }
}