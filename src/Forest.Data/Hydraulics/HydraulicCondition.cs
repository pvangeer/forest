namespace Forest.Data.Hydraulics
{
    public class HydraulicCondition : FragilityCurveElement
    {
        public HydraulicCondition() : this(0, (Probability)(1 / 1000.0), 0.0, 0.0)
        {
        }

        public HydraulicCondition(double waterLevel, Probability probability, double waveHeight, double wavePeriod)
            : base(waterLevel, probability)
        {
            WaveHeight = waveHeight;
            WavePeriod = wavePeriod;
        }

        public double WavePeriod { get; set; }

        public double WaveHeight { get; set; }
    }
}