using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Hydraulics;
using StoryTree.Messaging;

namespace StoryTree.IO
{
    public class ElicitationFormsExporter
    {
        private readonly StoryTreeLog log = new StoryTreeLog(typeof(ElicitationFormsExporter));
        private ExpertFormWriter writer = new ExpertFormWriter();

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
                writer.WriteForm(fileName, eventTreesToExport.First().Name, null, expert.Name, DateTime.Now,
                    hydraulicConditions.Select(hc => hc.WaterLevel).ToArray(),
                    hydraulicConditions.Select(hc => (double) hc.Probability).ToArray(), new[] {"test", "test2"});
                log.Info($"Bestand '{fileName}' geëxporteerd voor expert '{expert.Name}'");
            }
            log.Info($"{expertsToExport.Length} DOT formulieren geëxporteerd naar locatie '{fileLocation}'",true);
        }
    }
}
