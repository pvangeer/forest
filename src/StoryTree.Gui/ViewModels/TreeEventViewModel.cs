using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StoryTree.Data.Properties;
using StoryTree.Data.Tree;
using StoryTree.Gui.Command;

namespace StoryTree.Gui.ViewModels
{
    public class TreeEventViewModel : INotifyPropertyChanged
    {
        private TreeEventViewModel failingEventViewModel;
        private TreeEventViewModel passingEventViewModel;

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
                case nameof(PassingEvent):
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
                case nameof(FailingEvent):
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
                case nameof(Name):
                    OnPropertyChanged(nameof(Name));
                    break;
                case nameof(Summary):
                    OnPropertyChanged(nameof(Summary));
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

        public bool Selected => TreeEvent != null && Equals(TreeEvent,ParentEventTreeViewModel?.SelectedTreeEvent?.TreeEvent);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FireSelectedStateChangeRecursive()
        {
            OnPropertyChanged(nameof(Selected));
            FailingEvent?.FireSelectedStateChangeRecursive();
            PassingEvent?.FireSelectedStateChangeRecursive();
        }

        public void Select()
        {
            ParentEventTreeViewModel.SelectedTreeEvent = this;
        }
    }
}
