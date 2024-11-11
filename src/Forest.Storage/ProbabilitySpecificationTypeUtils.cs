using System;
using System.ComponentModel;
using Forest.Data.Estimations.PerTreeEvent;

namespace Forest.Storage
{
    public static class ProbabilitySpecificationTypeUtils
    {
        public static string ToStorageName(this ProbabilitySpecificationType modelProbabilitySpecificationType)
        {
            switch (modelProbabilitySpecificationType)
            {
                case ProbabilitySpecificationType.Classes:
                    return "classes";
                case ProbabilitySpecificationType.FixedFrequency:
                    return "fixedprobability";
                case ProbabilitySpecificationType.FixedValue:
                    return "fragilitycurve";
                default:
                    throw new InvalidEnumArgumentException(nameof(modelProbabilitySpecificationType));
            }
        }

        public static ProbabilitySpecificationType FromStorageName(string storageName)
        {
            switch (storageName)
            {
                case "classes":
                    return ProbabilitySpecificationType.Classes;
                case "fixedprobability":
                    return ProbabilitySpecificationType.FixedFrequency;
                case "fragilitycurve":
                    return ProbabilitySpecificationType.FixedValue;
                default:
                    throw new ArgumentException();
            }
        }
    }
}