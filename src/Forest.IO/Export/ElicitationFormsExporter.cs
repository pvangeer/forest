using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Forest.Data.Estimations;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Experts;
using Forest.Data.Hydrodynamics;
using Forest.Data.Tree;
using Forest.IO.Import;
using Forest.Messaging;

namespace Forest.IO.Export
{
    public class ElicitationFormsExporter
    {
        private readonly ForestLog log = new ForestLog(typeof(ElicitationFormsExporter));
        private readonly ProbabilityEstimationPerTreeEvent probabilityEstimation;
        private readonly ElicitationFormWriter writer = new ElicitationFormWriter();

        public ElicitationFormsExporter(ProbabilityEstimationPerTreeEvent probabilityEstimation)
        {
            this.probabilityEstimation = probabilityEstimation;
        }

        public void Export(string fileLocation, string prefix, Expert[] expertsToExport,
            ProbabilityEstimationPerTreeEvent estimationToExport)
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

            if (probabilityEstimation == null)
            {
                log.Error("Er is iets fout gegaan bij het exporteren. Er konden geen objecten worden gevonden om te exporteren.");
                return;
            }

            if (expertsToExport.Any(e => !probabilityEstimation.Experts.Contains(e)))
            {
                log.Error("Er is iets misgegaan bij het exporteren. Niet alle experts konden in het forestAnalysis worden gevonden.");
                return;
            }

            var hydraulicConditions = probabilityEstimation.HydrodynamicConditions.Distinct(new HydraulicConditionsWaterLevelComparer())
                .OrderBy(hc => hc.WaterLevel)
                .ToArray();
            if (!hydraulicConditions.Any())
                log.Error("Er moet minimaal 1 hydraulische conditie zijn gespecificeerd om te kunnen exporteren.");

            foreach (var expert in expertsToExport)
            {
                var fileName = Path.Combine(fileLocation, prefix + expert.Name + ".xlsx");

                writer.WriteForm(fileName,
                    EventTreeToDotForm(estimationToExport.EventTree, expert.Name, hydraulicConditions, probabilityEstimation.Estimations));
                log.Info($"Bestand '{fileName}' geëxporteerd voor expert '{expert.Name}'");
            }

            log.Info($"{expertsToExport.Length} DOT formulieren geëxporteerd naar locatie '{fileLocation}'", true);
        }

        private DotForm EventTreeToDotForm(EventTree eventTree, string expertName, HydrodynamicCondition[] hydraulicConditions,
            ObservableCollection<TreeEventProbabilityEstimation> estimates)
        {
            var nodes = new List<DotNode>();
            foreach (var treeEvent in eventTree.MainTreeEvent.GetAllEventsRecursive())
            {
                var treeEventEstimate = estimates.FirstOrDefault(e => e.TreeEvent == treeEvent);
                if (treeEventEstimate == null)
                    throw new ArgumentNullException();

                nodes.Add(new DotNode
                {
                    NodeName = treeEvent.Name,
                    Estimates = treeEventEstimate.ClassProbabilitySpecification.Where(e => e.Expert.Name == expertName).Select(s =>
                        new DotEstimate
                        {
                            WaterLevel = s.HydrodynamicCondition.WaterLevel,
                            Frequency = s.HydrodynamicCondition.Probability,
                            BestEstimate = (int)s.AverageEstimation,
                            LowerEstimate = (int)s.MinEstimation,
                            UpperEstimate = (int)s.MaxEstimation
                        }).ToArray()
                });
            }

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