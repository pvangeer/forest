using Forest.Data.Probabilities;

namespace Forest.Data.Hydrodynamics
{
    public class HydrodynamicCondition : FragilityCurveElement
    {
        public HydrodynamicCondition() : this(0, (Probability)(1 / 1000.0))
        {
        }

        public HydrodynamicCondition(double waterLevel, Probability probability)
            : base(waterLevel, probability)
        {
        }
    }
}