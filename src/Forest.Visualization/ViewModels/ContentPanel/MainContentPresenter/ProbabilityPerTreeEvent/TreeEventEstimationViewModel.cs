using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Tree;
using Forest.Gui;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.ProbabilityPerTreeEvent
{
    public class TreeEventEstimationViewModel : ViewModelBase
    {
        private static readonly Dictionary<ProbabilitySpecificationType, string> ProbabilitySpecificationTypes =
            Enum.GetValues(typeof(ProbabilitySpecificationType))
                .Cast<ProbabilitySpecificationType>()
                .ToDictionary(t => t, GetEstimationSpecificationTypeDisplayName);

        private readonly TreeEventProbabilityEstimate estimation;
        private readonly ForestGui gui;
        private readonly ProbabilityEstimationPerTreeEvent parentEstimation;

        public TreeEventEstimationViewModel(TreeEventProbabilityEstimate estimation,
            ProbabilityEstimationPerTreeEvent parentEstimation,
            ForestGui gui,
            ViewModelFactory viewModelFactory) : base(viewModelFactory)
        {
            this.gui = gui;
            gui.SelectionManager.SelectedTreeEventChanged += (sender, args) => OnPropertyChanged(nameof(IsSelected));

            this.estimation = estimation;
            this.estimation.TreeEvent.PropertyChanged += TreeEventPropertyChanged;
            this.estimation.PropertyChanged += EstimationPropertyChanged;
            this.parentEstimation = parentEstimation;
        }

        public string Name => estimation.TreeEvent.Name;

        public string Summary => estimation.TreeEvent.Summary;

        public int ProbabilityEstimationTypeIndex
        {
            get => ProbabilitySpecificationTypes.Keys.ToList().IndexOf(EstimationSpecification.Type);
            set
            {
                var selectedType = ProbabilitySpecificationTypes.ElementAt(value).Key;
                if (EstimationSpecification.Type != selectedType)
                    EstimationSpecification.Estimate.ChangeProbabilityEstimationType(selectedType);
            }
        }

        public IEnumerable<string> EstimationSpecificationOptions => ProbabilitySpecificationTypes.Values;

        public ProbabilitySpecificationViewModelBase EstimationSpecification
        {
            get
            {
                switch (estimation.ProbabilitySpecificationType)
                {
                    case ProbabilitySpecificationType.Classes:
                        return ViewModelFactory.CreateClassesProbabilitySpecificationViewModel(estimation);
                    case ProbabilitySpecificationType.FixedFrequency:
                        return ViewModelFactory.CreateFragilityCurveSpecificationTypeViewModel(estimation, parentEstimation);
                    case ProbabilitySpecificationType.FixedValue:
                        return ViewModelFactory.CreateFixedProbabilitySpecificationTypeViewModel(estimation);
                    default:
                        throw new InvalidEnumArgumentException();
                }
            }
        }

        public bool IsSelected => ReferenceEquals(estimation.TreeEvent, gui?.SelectionManager.GetSelectedTreeEvent(parentEstimation.EventTree));

        private void EstimationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TreeEventProbabilityEstimate.ProbabilitySpecificationType):
                    OnPropertyChanged(nameof(EstimationSpecification));
                    break;
            }
        }

        private void TreeEventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TreeEvent.Name):
                    OnPropertyChanged(nameof(Name));
                    break;
                case nameof(TreeEvent.Summary):
                    OnPropertyChanged(nameof(Summary));
                    break;
            }
        }

        private static string GetEstimationSpecificationTypeDisplayName(ProbabilitySpecificationType t)
        {
            switch (t)
            {
                case ProbabilitySpecificationType.Classes:
                    return "Klassen";
                case ProbabilitySpecificationType.FixedValue:
                    return "Vaste kans";
                case ProbabilitySpecificationType.FixedFrequency:
                    return "Vaste freqeuentielijn";
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public TreeEvent GetTreeEvent()
        {
            return estimation.TreeEvent;
        }

        public bool IsViewModelFor(TreeEvent otherTreeEvent)
        {
            return ReferenceEquals(estimation.TreeEvent, otherTreeEvent);
        }
    }
}