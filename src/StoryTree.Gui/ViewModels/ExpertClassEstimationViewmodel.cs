using System;
using System.ComponentModel;
using System.Globalization;
using StoryTree.Data;
using StoryTree.Data.Estimations;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class ExpertClassEstimationViewmodel : IDataErrorInfo
    {
        private readonly ExpertClassEstimation estimation;
        private bool lastMinEstimationValid = true;
        private bool lastMaxEstimationValid = true;
        private bool lastAverageEstimationValid = true;

        public ExpertClassEstimationViewmodel(ExpertClassEstimation estimation)
        {
            this.estimation = estimation;
        }

        public HydraulicCondition HydraulicCondition => estimation.HydraulicCondition;

        public Expert Expert => estimation.Expert;

        public ProbabilityClass MinEstimation
        {
            get => estimation.MinEstimation;
            set => estimation.MinEstimation = value;
        }
        public ProbabilityClass MaxEstimation
        {
            get => estimation.MaxEstimation;
            set => estimation.MaxEstimation = value;
        }
        public ProbabilityClass AverageEstimation
        {
            get => estimation.AverageEstimation;
            set => estimation.AverageEstimation = value;
        }

        private bool TryParseToProbabilityClass(string value, out ProbabilityClass probabilityClass)
        {
            if (!(value is string stringValue))
            {
                probabilityClass = ProbabilityClass.None;
                return false;
            }

            if (!(int.TryParse(stringValue, out int intValue)))
            {
                probabilityClass = ProbabilityClass.None;
                return false;
            }

            if (!Enum.IsDefined(typeof(ProbabilityClass), intValue))
            {
                probabilityClass = ProbabilityClass.None;
                return false;
            }

            probabilityClass = (ProbabilityClass)intValue;
            return true;
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(MinEstimation):
                        return lastMinEstimationValid
                            ? string.Empty
                            : "De gespecificeerde waarde kan niet worden vertaald naar een klasse";
                    case nameof(MaxEstimation):
                        return lastMaxEstimationValid
                            ? string.Empty
                            : "De gespecificeerde waarde kan niet worden vertaald naar een klasse";
                    case nameof(AverageEstimation):
                        return lastAverageEstimationValid
                            ? string.Empty
                            : "De gespecificeerde waarde kan niet worden vertaald naar een klasse";
                    default:
                        return string.Empty;
                }
            }
        }

        public string Error => null;
    }
}