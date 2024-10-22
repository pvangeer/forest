using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Hydraulics;
using Forest.Data.Tree;
using Forest.Gui.Annotations;

namespace Forest.Gui.ViewModels
{
    public class ExpertClassEstimationViewModel : INotifyPropertyChanged
    {
        private readonly ExpertClassEstimation estimation;
        private bool lastAverageEstimationValid = true;
        private bool lastMaxEstimationValid = true;
        private bool lastMinEstimationValid = true;

        public ExpertClassEstimationViewModel(ExpertClassEstimation estimation)
        {
            this.estimation = estimation;
        }

        public HydraulicCondition HydraulicCondition => estimation.HydraulicCondition;

        public Expert Expert => estimation.Expert;

        public ProbabilityClass MinEstimation
        {
            get => estimation.MinEstimation;
            set
            {
                estimation.MinEstimation = value;
                OnPropertyChanged();
            }
        }

        public ProbabilityClass MaxEstimation
        {
            get => estimation.MaxEstimation;
            set
            {
                estimation.MaxEstimation = value;
                OnPropertyChanged();
            }
        }

        public ProbabilityClass AverageEstimation
        {
            get => estimation.AverageEstimation;
            set
            {
                estimation.AverageEstimation = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}