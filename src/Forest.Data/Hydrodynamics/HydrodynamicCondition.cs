using Forest.Data.Probabilities;

namespace Forest.Data.Hydrodynamics
{
    public class HydrodynamicCondition : FragilityCurveElement
    {
        public HydrodynamicCondition() : this(0, (Probability)(1 / 1000.0), 0.0, 0.0)
        {
        }

        public HydrodynamicCondition(double waterLevel, Probability probability, double waveHeight, double wavePeriod)
            : base(waterLevel, probability)
        {
            WaveHeight = waveHeight;
            WavePeriod = wavePeriod;
        }

        public double WavePeriod { get; set; }

        public double WaveHeight { get; set; }
    }
}