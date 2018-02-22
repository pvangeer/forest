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
        private bool selected;

        public TreeEventViewModel(TreeEvent treeEvent, EventTreeViewModel parentEventTreeViewModel)
        {
            TreeEvent = treeEvent;
            ParentEventTreeViewModel = parentEventTreeViewModel;
            if (treeEvent != null)
            {
                treeEvent.PropertyChanged += TreeEventPropertyChanged;
            }
        }

        private EventTreeViewModel ParentEventTreeViewModel { get; }

        private void TreeEventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "PassingEvent":
                    if (PassingEvent == null)
                    {
                        ParentEventTreeViewModel.SelectedTreeEvent = this;
                    }
                    OnPropertyChanged(nameof(PassingEvent));
                    OnPropertyChanged(nameof(IsEndPointEvent));
                    OnPropertyChanged(nameof(HasTrueEventOnly));
                    OnPropertyChanged(nameof(HasFalseEventOnly));
                    OnPropertyChanged(nameof(HasTwoEvents));
                    break;
                case "FailingEvent":
                    if (FailingEvent == null)
                    {
                        ParentEventTreeViewModel.SelectedTreeEvent = this;
                    }
                    OnPropertyChanged(nameof(FailingEvent));
                    OnPropertyChanged(nameof(IsEndPointEvent));
                    OnPropertyChanged(nameof(HasTrueEventOnly));
                    OnPropertyChanged(nameof(HasFalseEventOnly));
                    OnPropertyChanged(nameof(HasTwoEvents));
                    break;
                case "Name":
                    OnPropertyChanged(nameof(Name));
                    break;
                case "Description":
                    OnPropertyChanged(nameof(Description));
                    break;
            }
        }

        public TreeEvent TreeEvent { get; }

        public string Name => TreeEvent.Name;

        public string Description => TreeEvent.Description;

        public TreeEventViewModel PassingEvent => TreeEvent.PassingEvent == null ? null : new TreeEventViewModel(TreeEvent.PassingEvent, ParentEventTreeViewModel);

        public TreeEventViewModel FailingEvent => TreeEvent.FailingEvent == null ? null : new TreeEventViewModel(TreeEvent.FailingEvent, ParentEventTreeViewModel);

        public bool IsEndPointEvent => TreeEvent.PassingEvent == null && TreeEvent.FailingEvent == null;

        public bool HasTrueEventOnly => TreeEvent.PassingEvent != null && TreeEvent.FailingEvent == null;

        public bool HasFalseEventOnly => TreeEvent.PassingEvent == null && TreeEvent.FailingEvent != null;

        public bool HasTwoEvents => TreeEvent.PassingEvent != null && TreeEvent.FailingEvent != null;

        public ICommand TreeEventClickedCommand => new TreeEventClickedCommand(this);

        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                ParentEventTreeViewModel.SelectedTreeEvent = this;
                OnPropertyChanged(nameof(Selected));
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
