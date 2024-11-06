using System.Linq;
using Forest.Data.Estimations.PerTreeEvent.Experts;
using Forest.Data.Hydrodynamics;
using Forest.Data.Tree;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public static class ProbabilityEstimationPerTreeEventExtensions
    {
        public static void AddHydrodynamicCondition(this ProbabilityEstimationPerTreeEvent probabilityEstimation,
            HydrodynamicCondition hydrodynamicCondition)
        {
            probabilityEstimation.HydrodynamicConditions.Add(hydrodynamicCondition);

            foreach (var treeEvent in probabilityEstimation.EventTree.MainTreeEvent.GetAllEventsRecursive())
            {
                var estimationsForThisTreeEvent = probabilityEstimation.Estimates.FirstOrDefault(e => e.TreeEvent == treeEvent);
                if (estimationsForThisTreeEvent == null)
                    continue;

                foreach (var expert in probabilityEstimation.Experts)
                {
                    estimationsForThisTreeEvent.ClassProbabilitySpecifications.Add(new ExpertClassEstimation
                    {
                        Expert = expert,
                        HydrodynamicCondition = hydrodynamicCondition,
                        AverageEstimation = ProbabilityClass.None,
                        MaxEstimation = ProbabilityClass.None,
                        MinEstimation = ProbabilityClass.None
                    });
                }
            }
        }

        public static void RemoveHydraulicCondition(this ProbabilityEstimationPerTreeEvent probabilityEstimation,
            HydrodynamicCondition hydrodynamicCondition)
        {
            probabilityEstimation.HydrodynamicConditions.Remove(hydrodynamicCondition);

            foreach (var treeEvent in probabilityEstimation.EventTree.MainTreeEvent.GetAllEventsRecursive())
            {
                var estimationsForThisTreeEvent = probabilityEstimation.Estimates.FirstOrDefault(e => e.TreeEvent == treeEvent);
                if (estimationsForThisTreeEvent == null)
                    continue;

                var estimatesToRemove = estimationsForThisTreeEvent.ClassProbabilitySpecifications.Where(e =>
                        e.HydrodynamicCondition == hydrodynamicCondition)
                    .ToArray();
                foreach (var estimationToRemove in estimatesToRemove)
                    estimationsForThisTreeEvent.ClassProbabilitySpecifications.Remove(estimationToRemove);
            }
        }

        public static void AddExpert(this ProbabilityEstimationPerTreeEvent probabilityEstimation, Expert expert)
        {
            probabilityEstimation.Experts.Add(expert);
            foreach (var treeEvent in probabilityEstimation.EventTree.MainTreeEvent.GetAllEventsRecursive())
            {
                var estimationsForThisTreeEvent = probabilityEstimation.Estimates.FirstOrDefault(e => e.TreeEvent == treeEvent);
                if (estimationsForThisTreeEvent == null)
                    continue;

                foreach (var hydraulicCondition in probabilityEstimation.HydrodynamicConditions)
                {
                    estimationsForThisTreeEvent.ClassProbabilitySpecifications.Add(new ExpertClassEstimation
                    {
                        Expert = expert,
                        HydrodynamicCondition = hydraulicCondition,
                        AverageEstimation = ProbabilityClass.None,
                        MaxEstimation = ProbabilityClass.None,
                        MinEstimation = ProbabilityClass.None
                    });
                }
            }
        }

        public static void RemoveExpert(this ProbabilityEstimationPerTreeEvent probabilityEstimation, Expert expert)
        {
            probabilityEstimation.Experts.Remove(expert);

            foreach (var treeEvent in probabilityEstimation.EventTree.MainTreeEvent.GetAllEventsRecursive())
            {
                var estimationsForThisTreeEvent = probabilityEstimation.Estimates.FirstOrDefault(e => e.TreeEvent == treeEvent);
                if (estimationsForThisTreeEvent == null)
                    continue;
                var estimatesToRemove = estimationsForThisTreeEvent.ClassProbabilitySpecifications.Where(e =>
                        e.Expert == expert)
                    .ToArray();
                foreach (var estimationToRemove in estimatesToRemove)
                    estimationsForThisTreeEvent.ClassProbabilitySpecifications.Remove(estimationToRemove);
            }
        }
    }
}