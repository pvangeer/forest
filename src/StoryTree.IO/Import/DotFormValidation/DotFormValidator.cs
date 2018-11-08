using System;
using System.Collections.Generic;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Tree;

namespace StoryTree.IO.Import.DotFormValidation
{
    public static class DotFormValidator
    {
        public static DotFormValidationResult Validate(DotForm form, Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var nodesValidationResult = ValidateNodes(form,project);

            var validationResult = new DotFormValidationResult
            {
                EventTreesValidation = ValidateEventTrees(form, project),
                ExpertValidation = ValidateExperts(form, project),
            };

            if (validationResult.EventTreesValidation == EventTreesValidationResult.Valid &&
                validationResult.ExpertValidation == ExpertValidationResult.Valid)
            {
                validationResult.NodesValidationResult = nodesValidationResult;
            }

            return validationResult;
        }

        private static Dictionary<DotNode, NodeValidationResult> ValidateNodes(DotForm form, Project project)
        {
            var results = new Dictionary<DotNode, NodeValidationResult>();

            if (form.Nodes == null || !form.Nodes.Any())
            {
                // TODO: Return validation result instead of null
                return null;
            }

            var eventTree = project.EventTrees.First(et => et.Name == form.EventTreeName);

            foreach (var formNode in form.Nodes)
            {
                var estimatedTreeEvent = eventTree.MainTreeEvent.FindTreeEvent(treeEvent => treeEvent.Name == formNode.NodeName);
                if (estimatedTreeEvent == null)
                {
                    results[formNode] = NodeValidationResult.NodeNotFound;
                }

                foreach (var estimate in formNode.Estimates)
                {
                    var condition =
                        project.HydraulicConditions.FirstOrDefault(hc =>
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

        private static ExpertValidationResult ValidateExperts(DotForm form, Project project)
        {
            if (project.Experts == null || !project.Experts.Any())
            {
                return ExpertValidationResult.NoExperts;
            }

            if (project.Experts.Any(e => e.Name == form.ExpertName))
            {
                return ExpertValidationResult.Valid;
            }

            return ExpertValidationResult.ExpertNotFound;
        }

        private static EventTreesValidationResult ValidateEventTrees(DotForm form, Project project)
        {
            if (project.EventTrees == null || !project.EventTrees.Any())
            {
                return EventTreesValidationResult.NoEventTrees;
            }

            if (project.EventTrees.Any(e => e.Name == form.EventTreeName))
            {
                return EventTreesValidationResult.Valid;
            }

            return EventTreesValidationResult.EventTreeNotFound;
        }
    }
}
