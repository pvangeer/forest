﻿using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Tree;

namespace Forest.Visualization.ViewModels
{
    public class FixedProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public FixedProbabilitySpecificationViewModel(TreeEvent treeEvent, TreeEventProbabilityEstimation estimation) : base(treeEvent, estimation)
        {
        }

        public Probability FixedProbability
        {
            get => Estimation.FixedProbability;
            set => Estimation.FixedProbability = value;
        }
    }
}