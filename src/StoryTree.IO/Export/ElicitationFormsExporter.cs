using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Office2013.Word;
using StoryTree.Data;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Tree;
using StoryTree.IO.Import;
using StoryTree.Messaging;

namespace StoryTree.IO.Export
{
    public class ElicitationFormsExporter
    {
        private readonly StoryTreeLog log = new StoryTreeLog(typeof(ElicitationFormsExporter));
        private readonly ElicitationFormWriter writer = new ElicitationFormWriter();

        public Project Project { get; }

        public ElicitationFormsExporter(Project project)
        {
            this.Project = project;
        }

        public void Export(string fileLocation, string prefix, Expert[] expertsToExport, EventTree[] eventTreesToExport)
        {
            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                log.Error("De doellocatie moet zijn gespecificeerd om te kunnen exporteren.");
                return;
            }

            if (!Directory.Exists(fileLocation))
            {
                log.Error("Bestandslocatie om naar te exporteren kon niet worden gevonden.");
                return;
            }

            if (expertsToExport.Length == 0)
            {
                log.Error("Er moet minimaal 1 expert zijn geselecteerd om te kunnen exporteren.");
                return;
            }
            if (expertsToExport.Any(e => !Project.Experts.Contains(e)))
            {
                log.Error("Er is iets misgegaan bij het exporteren. Niet alle experts konden in het project worden gevonden.");
                return;
            }

            if (eventTreesToExport.Length == 0)
            {
                log.Error("Er moet minimaal 1 gebeurtenis zijn geselecteerd om te kunnen exporteren.");
                return;
            }
            if (eventTreesToExport.Any(e => !Project.EventTrees.Contains(e)))
            {
                log.Error("Er is iets misgegaan bij het exporteren. Niet alle experts konden in het project worden gevonden.");
                return;
            }

            var hydraulicConditions = Project.HydraulicConditions.Distinct(new HydraulicConditionsWaterLevelComparer())
                .OrderBy(hc => hc.WaterLevel)
                .ToArray();
            if (!hydraulicConditions.Any())
            {
                log.Error("Er moet minimaal 1 hydraulische conditie zijn gespecificeerd om te kunnen exporteren.");
            }

            foreach (var expert in expertsToExport)
            {
                var fileName = Path.Combine(fileLocation,prefix + expert.Name + ".xlsx");

                writer.WriteForm(fileName, EventTreesToDotForms(eventTreesToExport, expert.Name, hydraulicConditions));
                log.Info($"Bestand '{fileName}' geëxporteerd voor expert '{expert.Name}'");
            }
            log.Info($"{expertsToExport.Length} DOT formulieren geëxporteerd naar locatie '{fileLocation}'",true);
        }

        private DotForm[] EventTreesToDotForms(EventTree[] eventTreeToExport, string expertName,
            HydraulicCondition[] hydraulicConditions)
        {
            var forms = new List<DotForm>();

            foreach (var eventTree in eventTreeToExport)
            {
                forms.Add(EventTreeToDotForm(eventTree, expertName, hydraulicConditions));
            }

            return forms.ToArray();
        }

        private DotForm EventTreeToDotForm(EventTree eventTree, string expertName, HydraulicCondition[] hydraulicConditions)
        {
            var nodes = new List<DotNode>();
            foreach (var treeEvent in eventTree.MainTreeEvent.GetAllEventsRecursive())
            {
                nodes.Add(new DotNode
                {
                    NodeName = treeEvent.Name,
                    Estimates = treeEvent.ClassesProbabilitySpecification.Where(e => e.Expert.Name == expertName).Select(s => new DotEstimate
                    {
                        WaterLevel = s.WaterLevel,
                        Frequency = hydraulicConditions.FirstOrDefault(hc => Math.Abs(hc.WaterLevel - s.WaterLevel) < 1e-6).Probability,
                        BestEstimate = (int)s.AverageEstimation,
                        LowerEstimate = (int)s.MinEstimation,
                        UpperEstimate = (int)s.MaxEstimation,
                    }).ToArray()
                });
            }

            return new DotForm
            {
                EventTreeName = eventTree.Name,
                ExpertName = expertName,
                GetFileStream = () => EventTreeToImageStream(eventTree),
                Date = DateTime.Today,
                Nodes = nodes.ToArray()
            };
        }

        private FileStream EventTreeToImageStream(EventTree eventTree)
        {
            return null;
        }
    }
}
