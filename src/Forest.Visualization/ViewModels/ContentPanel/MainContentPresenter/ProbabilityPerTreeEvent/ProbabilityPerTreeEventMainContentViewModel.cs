using System.ComponentModel;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Gui;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class ProbabilityPerTreeEventMainContentViewModel : GuiViewModelBase
    {
        private readonly ProbabilityEstimationPerTreeEvent estimation;

        public ProbabilityPerTreeEventMainContentViewModel(ProbabilityEstimationPerTreeEvent probabilityEstimation, ForestGui gui) : base(gui)
        {
            estimation = probabilityEstimation;
            estimation.PropertyChanged += EstimationPropertyChanged;
        }

        public string Name => estimation.Name;

        public ExpertsViewModel ExpertsViewModel => ViewModelFactory.CreateExpertsViewModel(estimation);

        public HydrodynamicsViewModel HydrodynamicsViewModel => ViewModelFactory.CreateHydrodynamicsViewModel(estimation);

        public EstimationPerTreeEventSpecificationViewModel ProbabilityEstimationsViewModel => ViewModelFactory.CreateEstimationPerTreeEventSpecificationViewModel(estimation);

        private void EstimationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ProbabilityEstimationPerTreeEvent.Name):
                    OnPropertyChanged(nameof(Name));
                    break;
            }
        }
    }
}