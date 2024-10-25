namespace Forest.Data.Probabilities
{
    public class FragilityCurveElement : NotifyPropertyChangedObject
    {
        public FragilityCurveElement(double waterLevel, Probability probability)
        {
            Probability = probability;
            WaterLevel = waterLevel;
        }

        public double WaterLevel { get; set; }

        public Probability Probability { get; set; }
    }
}