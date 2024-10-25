using System;
using System.Linq;
using Forest.Data.Estimations;
using Forest.Data.Estimations.PerTreeEvet;
using Forest.Data.Tree;
using Forest.IO.Import.DotFormValidation;
using Forest.Messaging;

namespace Forest.IO.Import
{
    public class ElicitationFormImporter
    {
        private readonly ForestLog log = new ForestLog(typeof(ElicitationFormImporter));
        private readonly ProbabilityEstimationPerTreeEvent probabilityEstimation;

        public ElicitationFormImporter(ProbabilityEstimationPerTreeEvent probabilityEstimation)
        {
            this.probabilityEstimation = probabilityEstimation;
        }

        public void Import(string fileName)
        {
            DotForm[] formContent;
            try
            {
                formContent = ElicitationFormReader.ReadElicitationForm(fileName).ToArray();
            }
            catch (Exception e)
            {
                log.Error($"Er is iets onverwachts misgegaan bij het lezen van bestand '{fileName}':{e.Message} \t {e.StackTrace}");
                return;
            }

            foreach (var dotForm in formContent)
            {
                var validationResult = DotFormValidator.Validate(dotForm, probabilityEstimation);
                if (ValidationFailed(fileName, validationResult, dotForm))
                    return;
            }

            foreach (var dotForm in formContent)
            {
                var expert = probabilityEstimation.Experts.First(ex => ex.Name == dotForm.ExpertName);

                foreach (var dotFormNode in dotForm.Nodes)
                {
                    var treeEvent = probabilityEstimation.EventTree.MainTreeEvent.FindTreeEvent(n => n.Name == dotFormNode.NodeName);
                    var estimations = probabilityEstimation.Estimations.First(e => e.TreeEvent == treeEvent);

                    foreach (var dotEstimate in dotFormNode.Estimates)
                    {
                        var specification = estimations.ClassProbabilitySpecification.First(s =>
                            s.Expert == expert && Math.Abs(s.HydrodynamicCondition.WaterLevel - dotEstimate.WaterLevel) < 1e-6);
                        specification.MinEstimation = (ProbabilityClass)dotEstimate.LowerEstimate;
                        specification.AverageEstimation = (ProbabilityClass)dotEstimate.BestEstimate;
                        specification.MaxEstimation = (ProbabilityClass)dotEstimate.UpperEstimate;
                    }

                    // TODO: Notify?
                }
            }
        }

        private bool ValidationFailed(string fileName, DotFormValidationResult validationResult, DotForm dotForm)
        {
            if (validationResult.ExpertValidation == ExpertValidationResult.ExpertNotFound)
            {
                log.Error(
                    $"Fout bij het lezen van bestand {fileName}: De gespecificeerde expert ({dotForm.ExpertName}) kon niet in het forestAnalysis worden gevonden.");
                return true;
            }

            if (validationResult.ExpertValidation == ExpertValidationResult.NoExperts)
            {
                log.Error(
                    "Het forestAnalysis bevat nog geen experts. Daardoor is het niet mogelijk om een elicitatieformulier in te lezen.");
                return true;
            }

            foreach (var nodeValidationResult in validationResult.NodesValidationResult)
            {
                if (nodeValidationResult.Value == NodeValidationResult.NodeNotFound)
                {
                    log.Error(
                        $"Fout bij het lezen van bestand {fileName}: Knoop met de naam '{nodeValidationResult.Key.NodeName}' kon niet worden gevonden.");
                    return true;
                }

                if (nodeValidationResult.Value == NodeValidationResult.InvalidEstimationValue)
                {
                    log.Error(
                        $"Fout bij het lezen van bestand {fileName}: Een waarschijnlijkheidsschatting voor knoop '{nodeValidationResult.Key.NodeName}' heeft een ongeldige waarde.");
                    return true;
                }

                if (nodeValidationResult.Value == NodeValidationResult.InvalidFrequencyForWaterLevel)
                {
                    log.Error(
                        $"Fout bij het lezen van bestand {fileName}: Een van de waterstanden voor knoop '{nodeValidationResult.Key.NodeName}' heeft een afwijkende frequentie ten opzichte van het forestAnalysis.");
                    return true;
                }

                if (nodeValidationResult.Value == NodeValidationResult.WaterLevelNotFound)
                {
                    log.Error(
                        $"Fout bij het lezen van bestand {fileName}: Een van de waterstanden voor knoop '{nodeValidationResult.Key.NodeName}' kan niet in het forestAnalysis worden gevonden.");
                    return true;
                }
            }

            return false;
        }
    }
}