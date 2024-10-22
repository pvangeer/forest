using System;
using System.Globalization;
using System.Windows.Controls;
using Forest.Data.Estimations;

namespace Forest.Gui
{
    public class StringToProbabilityClassValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is ProbabilityClass)
                return new ValidationResult(true, null);

            if (!(value is string stringValue))
                return new ValidationResult(false, "De gespecificeerde waarde kon niet worden vertaald naar een klasse.");

            if (!int.TryParse(stringValue, out var intValue))
            {
                if (Enum.TryParse<ProbabilityClass>(stringValue, true, out var probabilityClass))
                    return new ValidationResult(true, null);
                return new ValidationResult(false, "De gespecificeerde waarde kon niet worden vertaald naar een klasse.");
            }

            if (!Enum.IsDefined(typeof(ProbabilityClass), intValue))
                return new ValidationResult(false, "De gespecificeerde waarde kon niet worden vertaald naar een klasse.");

            return new ValidationResult(true, null);
        }
    }
}