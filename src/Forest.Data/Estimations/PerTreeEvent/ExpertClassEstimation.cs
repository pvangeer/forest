﻿using Forest.Data.Estimations.PerTreeEvent.Experts;
using Forest.Data.Hydrodynamics;

namespace Forest.Data.Estimations.PerTreeEvent
{
    public class ExpertClassEstimation : Entity
    {
        public Expert Expert { get; set; }

        public HydrodynamicCondition HydrodynamicCondition { get; set; }

        public ProbabilityClass MinEstimation { get; set; }

        public ProbabilityClass AverageEstimation { get; set; }

        public ProbabilityClass MaxEstimation { get; set; }
    }
}