﻿using Forest.Data.Hydrodynamics;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class HydrodynamicConditionViewModel : FragilityCurveElementViewModel
    {
        public HydrodynamicConditionViewModel() : this(new HydrodynamicCondition())
        {
        }

        public HydrodynamicConditionViewModel(HydrodynamicCondition condition) : base(condition)
        {
            HydrodynamicCondition = condition;
        }

        public HydrodynamicCondition HydrodynamicCondition { get; }

        public double WavePeriod
        {
            get => HydrodynamicCondition.WavePeriod;
            set
            {
                HydrodynamicCondition.WavePeriod = value;
                HydrodynamicCondition.OnPropertyChanged();
                OnPropertyChanged();
            }
        }

        public double WaveHeight
        {
            get => HydrodynamicCondition.WaveHeight;
            set
            {
                HydrodynamicCondition.WaveHeight = value;
                OnPropertyChanged();
                HydrodynamicCondition.OnPropertyChanged();
            }
        }
    }
}