using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StoryTree.Calculators;
using StoryTree.Data.Estimations;
using StoryTree.Data.Properties;
using StoryTree.Data.Services;
using StoryTree.Data.Tree;
using StoryTree.Gui.Command;

namespace StoryTree.Gui.ViewModels
{
    public class TreeEventViewModel : INotifyPropertyChanged
    {
        private TreeEventViewModel failingEventViewModel;
        private TreeEventViewModel passingEventViewModel;

        private static readonly Dictionary<ProbabilitySpecificationType, string> ProbabilitySpecificationTypes =
            Enum.GetValues(typeof(ProbabilitySpecificationType)).Cast<ProbabilitySpecificationType>()
                .ToDictionary(t => t, GetDisplayName);

        private static string GetDisplayName(ProbabilitySpecificationType t)
        {
            switch (t)
            {
                case ProbabilitySpecificationType.Classes:
                    return "Klassen";
                case ProbabilitySpecificationType.FixedValue:
                    return "Vaste kans";
                case ProbabilitySpecificationType.FixedFreqeuncy:
                    return "Vaste freqeuentielijn";
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        private ProbabilitySpecificationViewModelBase probabilityEstimationViewModel;

        public TreeEventViewModel([NotNull]TreeEvent treeEvent, [NotNull]EventTreeViewModel parentEventTreeViewModel)
        {
            TreeEvent = treeEvent;
            ParentEventTreeViewModel = parentEventTreeViewModel;
            treeEvent.PropertyChanged += TreeEventPropertyChanged;
        }

        private EventTreeViewModel ParentEventTreeViewModel { get; }

        private void TreeEventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TreeEvent.PassingEvent):
                    passingEventViewModel = null;
                    if (PassingEvent == null)
                    {
                        // TODO: Shouldn't this be done by the command?
                        Select();
                    }
                    OnPropertyChanged(nameof(PassingEvent));
                    OnPropertyChanged(nameof(IsEndPointEvent));
                    OnPropertyChanged(nameof(HasTrueEventOnly));
                    OnPropertyChanged(nameof(HasFalseEventOnly));
                    OnPropertyChanged(nameof(HasTwoEvents));
                    break;
                case nameof(TreeEvent.FailingEvent):
                    failingEventViewModel = null;
                    if (FailingEvent == null)
                    {
                        // TODO: Shouldn't this be done by the command?
                        Select();
                    }
                    OnPropertyChanged(nameof(FailingEvent));
                    OnPropertyChanged(nameof(IsEndPointEvent));
                    OnPropertyChanged(nameof(HasTrueEventOnly));
                    OnPropertyChanged(nameof(HasFalseEventOnly));
                    OnPropertyChanged(nameof(HasTwoEvents));
                    break;
                case nameof(TreeEvent.Name):
                    OnPropertyChanged(nameof(Name));
                    break;
                case nameof(TreeEvent.Summary):
                    OnPropertyChanged(nameof(Summary));
                    break;
                case nameof(TreeEvent.ProbabilitySpecificationType):
                    probabilityEstimationViewModel = null;
                    OnPropertyChanged(nameof(ProbabilityEstimationTypeIndex));
                    OnPropertyChanged(nameof(EstimationSpecification));
                    break;
                case nameof(TreeEvent.Information):
                    OnPropertyChanged(nameof(Information));
                    break;
            }
        }

        public TreeEvent TreeEvent { get; }

        public string Name
        {
            get => TreeEvent.Name;
            set
            {
                TreeEvent.Name = value;
                TreeEvent.OnPropertyChanged(nameof(TreeEvent.Name));
            }
        }

        public string Summary
        {
            get => TreeEvent.Summary;
            set
            {
                TreeEvent.Summary = value;
                TreeEvent.OnPropertyChanged(nameof(TreeEvent.Summary));
            }
        }

        public string Details
        {
            get => TreeEvent.Details;
            set
            {
                TreeEvent.Details = value;
                TreeEvent.OnPropertyChanged(nameof(TreeEvent.Details));
            }
        }

        public TreeEventViewModel PassingEvent
        {
            get
            {
                if (TreeEvent?.PassingEvent == null)
                {
                    return null;
                }
                return passingEventViewModel ?? new TreeEventViewModel(TreeEvent.PassingEvent, ParentEventTreeViewModel);
            }
        }

        public TreeEventViewModel FailingEvent
        {
            get
            {
                if (TreeEvent?.FailingEvent == null)
                {
                    return null;
                }

                return failingEventViewModel ?? (failingEventViewModel =
                           new TreeEventViewModel(TreeEvent.FailingEvent, ParentEventTreeViewModel));
            }
        }

        public bool IsEndPointEvent => TreeEvent.PassingEvent == null && TreeEvent.FailingEvent == null;

        public bool HasTrueEventOnly => TreeEvent.PassingEvent != null && TreeEvent.FailingEvent == null;

        public bool HasFalseEventOnly => TreeEvent.PassingEvent == null && TreeEvent.FailingEvent != null;

        public bool HasTwoEvents => TreeEvent.PassingEvent != null && TreeEvent.FailingEvent != null;

        public ICommand TreeEventClickedCommand => new TreeEventClickedCommand(this);

        public bool IsSelected => TreeEvent != null && Equals(TreeEvent,ParentEventTreeViewModel?.SelectedTreeEvent?.TreeEvent);

        public int ProbabilityEstimationTypeIndex
        {
            get => ProbabilitySpecificationTypes.Keys.ToList().IndexOf(TreeEvent.ProbabilitySpecificationType);
            set
            {
                var selectedType = ProbabilitySpecificationTypes.ElementAt(value).Key;
                if (TreeEvent.ProbabilitySpecificationType != selectedType)
                {
                    TreeEventManipulations.ChangeProbabilityEstimationType(TreeEvent, selectedType);
                }
            }
        }

        public IEnumerable<string> EstimationSpecificationOptions => ProbabilitySpecificationTypes.Values;

        public ProbabilitySpecificationViewModelBase EstimationSpecification =>
            probabilityEstimationViewModel ?? (probabilityEstimationViewModel =
                ParentEventTreeViewModel.EstimationSpecificationViewModelFactory.CreateViewModel(TreeEvent));

        public TreeEvent[] CriticalPath => TreeEvent == null ? null :
            ParentEventTreeViewModel.MainTreeEventViewModel == null ? null :
            CriticalPathCalculator.GetCriticalPath(ParentEventTreeViewModel.MainTreeEventViewModel.TreeEvent, TreeEvent).ToArray();

        public string Information
        {
            get => TreeEvent.Information;
            set
            {
                TreeEvent.Information = value;
                TreeEvent.OnPropertyChanged(nameof(TreeEvent.Information));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FireSelectedStateChangeRecursive()
        {
            OnPropertyChanged(nameof(IsSelected));
            FailingEvent?.FireSelectedStateChangeRecursive();
            PassingEvent?.FireSelectedStateChangeRecursive();
        }

        public void Select()
        {
            ParentEventTreeViewModel.SelectedTreeEvent = this;
        }
    }
}
