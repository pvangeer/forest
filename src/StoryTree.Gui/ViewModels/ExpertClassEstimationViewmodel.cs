using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data;
using StoryTree.Data.Estimations;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Tree;
using StoryTree.Gui.Annotations;

namespace StoryTree.Gui.ViewModels
{
    public class ExpertClassEstimationViewModel : INotifyPropertyChanged
    {
        private readonly ExpertClassEstimation estimation;
        private bool lastMinEstimationValid = true;
        private bool lastMaxEstimationValid = true;
        private bool lastAverageEstimationValid = true;

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
                OnPropertyChanged(nameof(MinEstimation));
            }
        }

        public ProbabilityClass MaxEstimation
        {
            get => estimation.MaxEstimation;
            set
            {
                estimation.MaxEstimation = value;
                OnPropertyChanged(nameof(MaxEstimation));
            }
        }

        public ProbabilityClass AverageEstimation
        {
            get => estimation.AverageEstimation;
            set
            {
                estimation.AverageEstimation = value;
                OnPropertyChanged(nameof(AverageEstimation));
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