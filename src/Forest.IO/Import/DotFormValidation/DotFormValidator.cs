using System;
using System.Collections.Generic;
using System.Linq;
using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Tree;

namespace Forest.IO.Import.DotFormValidation
{
    public static class DotFormValidator
    {
        public static DotFormValidationResult Validate(DotForm form, ProbabilityEstimationPerTreeEvent probabilityEstimation)
        {
            if (probabilityEstimation == null)
                throw new ArgumentNullException(nameof(probabilityEstimation));

            var nodesValidationResult = ValidateNodes(form, probabilityEstimation);

            var validationResult = new DotFormValidationResult
            {
                ExpertValidation = ValidateExperts(form, probabilityEstimation)
            };

            if (validationResult.ExpertValidation == ExpertValidationResult.Valid)
                validationResult.NodesValidationResult = nodesValidationResult;

            return validationResult;
        }

        private static Dictionary<DotNode, NodeValidationResult> ValidateNodes(DotForm form, ProbabilityEstimationPerTreeEvent probabilityEstimation)
        {
            var results = new Dictionary<DotNode, NodeValidationResult>();

            if (form.Nodes == null || !form.Nodes.Any())
                // TODO: Return validation result instead of null
                return null;

            foreach (var formNode in form.Nodes)
            {
                var estimatedTreeEvent =
                    probabilityEstimation.EventTree.MainTreeEvent.FindTreeEvent(treeEvent => treeEvent.Name == formNode.NodeName);
                if (estimatedTreeEvent == null)
                    results[formNode] = NodeValidationResult.NodeNotFound;

                foreach (var estimate in formNode.Estimates)
                {
                    var condition =
                        probabilityEstimation.HydrodynamicConditions.FirstOrDefault(hc =>
                            Math.Abs(hc.WaterLevel - estimate.WaterLevel) < 1e-6);

                    if (condition == null)
                    {
                        results[formNode] = NodeValidationResult.WaterLevelNotFound;
                        break;
                    }

                    if (!(Math.Abs(condition.Probability - estimate.Frequency) < 1e-8))
                        results[formNode] = NodeValidationResult.InvalidFrequencyForWaterLevel;

                    if (estimate.LowerEstimate < 0 || estimate.LowerEstimate > 7 ||
                        estimate.BestEstimate < 0 || estimate.BestEstimate > 7 ||
                        estimate.UpperEstimate < 0 || estimate.UpperEstimate > 7)
                    {
                        results[formNode] = NodeValidationResult.InvalidEstimationValue;
                        break;
                    }
                }

                if (!results.ContainsKey(formNode))
                    results[formNode] = NodeValidationResult.Valid;
            }

            return results;
        }

        private static ExpertValidationResult ValidateExperts(DotForm form, ProbabilityEstimationPerTreeEvent probabilityEstimation)
        {
            if (probabilityEstimation.Experts == null || !probabilityEstimation.Experts.Any())
                return ExpertValidationResult.NoExperts;

            if (probabilityEstimation.Experts.Any(e => e.Name == form.ExpertName))
                return ExpertValidationResult.Valid;

            return ExpertValidationResult.ExpertNotFound;
        }
    }
}