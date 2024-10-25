using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Estimations;
using Forest.Data.Estimations.PerTreeEvet;
using Forest.Data.Experts;
using Forest.Data.Hydrodynamics;
using Forest.Data.Tree;

namespace Forest.Visualization.ViewModels
{
    public class ExpertClassEstimationViewModel : INotifyPropertyChanged
    {
        private readonly ExpertClassEstimation estimation;

        public ExpertClassEstimationViewModel(ExpertClassEstimation estimation)
        {
            this.estimation = estimation;
        }

        public HydrodynamicCondition HydrodynamicCondition => estimation.HydrodynamicCondition;

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}