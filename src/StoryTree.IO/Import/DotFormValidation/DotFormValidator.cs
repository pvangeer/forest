using System;
using System.Collections.Generic;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Tree;

namespace StoryTree.IO.Import.DotFormValidation
{
    public static class DotFormValidator
    {
        public static DotFormValidationResult Validate(DotForm form, EventTreeProject eventTreeProject)
        {
            if (eventTreeProject == null)
            {
                throw new ArgumentNullException(nameof(eventTreeProject));
            }

            var nodesValidationResult = ValidateNodes(form,eventTreeProject);

            var validationResult = new DotFormValidationResult
            {
                ExpertValidation = ValidateExperts(form, eventTreeProject),
            };

            if (validationResult.ExpertValidation == ExpertValidationResult.Valid)
            {
                validationResult.NodesValidationResult = nodesValidationResult;
            }

            return validationResult;
        }

        private static Dictionary<DotNode, NodeValidationResult> ValidateNodes(DotForm form, EventTreeProject eventTreeProject)
        {
            var results = new Dictionary<DotNode, NodeValidationResult>();

            if (form.Nodes == null || !form.Nodes.Any())
            {
                // TODO: Return validation result instead of null
                return null;
            }

            foreach (var formNode in form.Nodes)
            {
                var estimatedTreeEvent = eventTreeProject.EventTree.MainTreeEvent.FindTreeEvent(treeEvent => treeEvent.Name == formNode.NodeName);
                if (estimatedTreeEvent == null)
                {
                    results[formNode] = NodeValidationResult.NodeNotFound;
                }

                foreach (var estimate in formNode.Estimates)
                {
                    var condition =
                        eventTreeProject.HydraulicConditions.FirstOrDefault(hc =>
                            Math.Abs(hc.WaterLevel - estimate.WaterLevel) < 1e-6);

                    if (condition == null)
                    {
                        results[formNode] = NodeValidationResult.WaterLevelNotFound;
                        break;
                    }

                    if (!(Math.Abs(condition.Probability - estimate.Frequency) < 1e-8))
                    {
                        results[formNode] = NodeValidationResult.InvalidFrequencyForWaterLevel;
                    }

                    if (estimate.LowerEstimate < 0 || estimate.LowerEstimate > 7 ||
                        estimate.BestEstimate < 0 || estimate.BestEstimate > 7 ||
                        estimate.UpperEstimate < 0 || estimate.UpperEstimate > 7)
                    {
                        results[formNode] = NodeValidationResult.InvalidEstimationValue;
                        break;
                    }
                }

                if (!results.ContainsKey(formNode))
                {
                    results[formNode] = NodeValidationResult.Valid;
                }
            }

            return results;
        }

        private static ExpertValidationResult ValidateExperts(DotForm form, EventTreeProject eventTreeProject)
        {
            if (eventTreeProject.Experts == null || !eventTreeProject.Experts.Any())
            {
                return ExpertValidationResult.NoExperts;
            }

            if (eventTreeProject.Experts.Any(e => e.Name == form.ExpertName))
            {
                return ExpertValidationResult.Valid;
            }

            return ExpertValidationResult.ExpertNotFound;
        }
    }
}
