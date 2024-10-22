using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Forest.Data;
using Forest.Data.Hydraulics;
using Forest.IO.Import;
using Forest.Messaging;
using Forest.Data.Tree;

namespace Forest.IO.Export
{
    public class ElicitationFormsExporter
    {
        private readonly StoryTreeLog log = new StoryTreeLog(typeof(ElicitationFormsExporter));
        private readonly ElicitationFormWriter writer = new ElicitationFormWriter();

        public ElicitationFormsExporter(EventTreeProject eventTreeProject)
        {
            EventTreeProject = eventTreeProject;
        }

        public EventTreeProject EventTreeProject { get; }

        public void Export(string fileLocation, string prefix, Expert[] expertsToExport, EventTree eventTreeToExport)
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

            if (expertsToExport.Any(e => !EventTreeProject.Experts.Contains(e)))
            {
                log.Error("Er is iets misgegaan bij het exporteren. Niet alle experts konden in het eventTreeProject worden gevonden.");
                return;
            }

            var hydraulicConditions = EventTreeProject.HydraulicConditions.Distinct(new HydraulicConditionsWaterLevelComparer())
                .OrderBy(hc => hc.WaterLevel)
                .ToArray();
            if (!hydraulicConditions.Any())
                log.Error("Er moet minimaal 1 hydraulische conditie zijn gespecificeerd om te kunnen exporteren.");

            foreach (var expert in expertsToExport)
            {
                var fileName = Path.Combine(fileLocation, prefix + expert.Name + ".xlsx");

                writer.WriteForm(fileName, EventTreeToDotForm(eventTreeToExport, expert.Name, hydraulicConditions));
                log.Info($"Bestand '{fileName}' geëxporteerd voor expert '{expert.Name}'");
            }

            log.Info($"{expertsToExport.Length} DOT formulieren geëxporteerd naar locatie '{fileLocation}'", true);
        }

        private DotForm EventTreeToDotForm(EventTree eventTree, string expertName, HydraulicCondition[] hydraulicConditions)
        {
            var nodes = new List<DotNode>();
            foreach (var treeEvent in eventTree.MainTreeEvent.GetAllEventsRecursive())
                nodes.Add(new DotNode
                {
                    NodeName = treeEvent.Name,
                    Estimates = treeEvent.ClassesProbabilitySpecification.Where(e => e.Expert.Name == expertName).Select(s =>
                        new DotEstimate
                        {
                            WaterLevel = s.HydraulicCondition.WaterLevel,
                            Frequency = s.HydraulicCondition.Probability,
                            BestEstimate = (int)s.AverageEstimation,
                            LowerEstimate = (int)s.MinEstimation,
                            UpperEstimate = (int)s.MaxEstimation
                        }).ToArray()
                });

            return new DotForm
            {
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