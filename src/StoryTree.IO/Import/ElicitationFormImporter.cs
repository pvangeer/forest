using System;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Estimations;
using StoryTree.Data.Tree;
using StoryTree.IO.Import.DotFormValidation;
using StoryTree.Messaging;

namespace StoryTree.IO.Import
{
    public class ElicitationFormImporter
    {
        private readonly StoryTreeLog log = new StoryTreeLog(typeof(ElicitationFormImporter));

        public Project Project { get; }

        public ElicitationFormImporter(Project project)
        {
            this.Project = project;
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
                var validationResult = DotFormValidator.Validate(dotForm, Project);
                if (ValidationFailed(fileName, validationResult, dotForm))
                {
                    return;
                }
            }

            foreach (var dotForm in formContent)
            {
                var eventTree = Project.EventTrees.First(et => et.Name == dotForm.EventTreeName);
                var expert = Project.Experts.First(ex => ex.Name == dotForm.ExpertName);
                
                foreach (var dotFormNode in dotForm.Nodes)
                {
                    var node = eventTree.MainTreeEvent.FindTreeEvent(n => n.Name == dotFormNode.NodeName);
                    foreach (var dotEstimate in dotFormNode.Estimates)
                    {
                        var specification = node.ClassesProbabilitySpecification.First(s =>
                            s.Expert == expert && Math.Abs(s.HydraulicCondition.WaterLevel - dotEstimate.WaterLevel) < 1e-6);
                        specification.MinEstimation = (ProbabilityClass)dotEstimate.LowerEstimate;
                        specification.AverageEstimation = (ProbabilityClass)dotEstimate.BestEstimate;
                        specification.MaxEstimation = (ProbabilityClass)dotEstimate.UpperEstimate;
                    }
                    node.OnPropertyChanged(nameof(TreeEvent.ClassesProbabilitySpecification));
                }
            }
        }

        private bool ValidationFailed(string fileName, DotFormValidationResult validationResult, DotForm dotForm)
        {
            if (validationResult.EventTreesValidation == EventTreesValidationResult.EventTreeNotFound)
            {
                log.Error(
                    $"Fout bij het lezen van bestand {fileName}: De gespecificeerde gebeurtenis ({dotForm.EventTreeName}) kon niet in het project worden gevonden.");
                return true;
            }

            if (validationResult.EventTreesValidation == EventTreesValidationResult.NoEventTrees)
            {
                log.Error(
                    $"Het project bevat nog geen gebeurtenissen. Daardoor is het niet mogelijk om een elicitatieformulier in te lezen.");
                return true;
            }

            if (validationResult.ExpertValidation == ExpertValidationResult.ExpertNotFound)
            {
                log.Error(
                    $"Fout bij het lezen van bestand {fileName}: De gespecificeerde expert ({dotForm.ExpertName}) kon niet in het project worden gevonden.");
                return true;
            }

            if (validationResult.ExpertValidation == ExpertValidationResult.NoExperts)
            {
                log.Error(
                    $"Het project bevat nog geen experts. Daardoor is het niet mogelijk om een elicitatieformulier in te lezen.");
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
                        $"Fout bij het lezen van bestand {fileName}: Een van de waterstanden voor knoop '{nodeValidationResult.Key.NodeName}' heeft een afwijkende frequentie ten opzichte van het project.");
                    return true;
                }

                if (nodeValidationResult.Value == NodeValidationResult.WaterLevelNotFound)
                {
                    log.Error(
                        $"Fout bij het lezen van bestand {fileName}: Een van de waterstanden voor knoop '{nodeValidationResult.Key.NodeName}' kan niet in het project worden gevonden.");
                    return true;
                }
            }

            return false;
        }
    }
}
