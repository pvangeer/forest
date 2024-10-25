﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Tree;

namespace Forest.Visualization.ViewModels
{
    public class FixedFragilityCurveSpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        private readonly ObservableCollection<FragilityCurveElementViewModel> fixedFragilityCurveViewModels;

        public FixedFragilityCurveSpecificationViewModel(TreeEvent treeEvent, ForestAnalysis forestAnalysis,
            TreeEventProbabilityEstimation estimation) : base(treeEvent, estimation)
        {
            ForestAnalysis = forestAnalysis;

            fixedFragilityCurveViewModels =
                new ObservableCollection<FragilityCurveElementViewModel>(
                    estimation.FragilityCurve.Select(e => new FragilityCurveElementViewModel(e)));
            fixedFragilityCurveViewModels.CollectionChanged += FragilityCurveViewModelsCollectionChanged;
        }

        public ForestAnalysis ForestAnalysis { get; }

        public ObservableCollection<FragilityCurveElementViewModel> FixedFragilityCurve => FixedFragilityCurveViewModels();

        private ObservableCollection<FragilityCurveElementViewModel> FixedFragilityCurveViewModels()
        {
            var estimatedWaterLevels = fixedFragilityCurveViewModels.Select(vm => vm.WaterLevel).ToArray();
            var currentWaterLevels = ForestAnalysis.HydrodynamicConditions.Select(hc => hc.WaterLevel).Distinct().OrderBy(w => w).ToArray();
            var missingWaterLevels = currentWaterLevels.Except(estimatedWaterLevels).ToArray();
            var waterLevelsToRemove = estimatedWaterLevels.Except(currentWaterLevels).ToArray();
            foreach (var waterLevel in waterLevelsToRemove)
                fixedFragilityCurveViewModels.Remove(
                    fixedFragilityCurveViewModels.FirstOrDefault(vm => Math.Abs(vm.WaterLevel - waterLevel) < 1e-8));

            foreach (var waterLevel in missingWaterLevels)
            {
                var firstElementWithHigherWater = fixedFragilityCurveViewModels.FirstOrDefault(vm => vm.WaterLevel > waterLevel);
                var indexOfFirstElementWithHigherWater = fixedFragilityCurveViewModels.IndexOf(firstElementWithHigherWater);
                fixedFragilityCurveViewModels.Insert(Math.Max(0, indexOfFirstElementWithHigherWater),
                    new FragilityCurveElementViewModel(new FragilityCurveElement(waterLevel, (Probability)1.0)));
            }

            return fixedFragilityCurveViewModels;
        }

        private void FragilityCurveViewModelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var item in e.NewItems.OfType<FragilityCurveElementViewModel>())
                    Estimation.FragilityCurve.Add(item.FragilityCurveElement);

            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (var item in e.OldItems.OfType<FragilityCurveElementViewModel>())
                    Estimation.FragilityCurve.Remove(item.FragilityCurveElement);
        }
    }
}