using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Annotations;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class TreeEventViewModel : INotifyPropertyChanged
    {
        public TreeEventViewModel(TreeEvent treeEvent)
        {
            if (treeEvent != null)
            {
                treeEvent.PropertyChanged -= TreeEventPropertyChanged;
            }
            TreeEvent = treeEvent;
            if (treeEvent != null)
            {
                treeEvent.PropertyChanged += TreeEventPropertyChanged;
            }
        }

        private void TreeEventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TrueEvent":
                    OnPropertyChanged(nameof(TrueEvent));
                    OnPropertyChanged(nameof(IsEndPointEvent));
                    OnPropertyChanged(nameof(HasTrueEventOnly));
                    OnPropertyChanged(nameof(HasFalseEventOnly));
                    OnPropertyChanged(nameof(HasTwoEvents));
                    break;
                case "FalseEvent":
                    OnPropertyChanged(nameof(FalseEvent));
                    OnPropertyChanged(nameof(IsEndPointEvent));
                    OnPropertyChanged(nameof(HasTrueEventOnly));
                    OnPropertyChanged(nameof(HasFalseEventOnly));
                    OnPropertyChanged(nameof(HasTwoEvents));
                    break;
            }
        }

        private TreeEvent TreeEvent { get; }

        public string Name => TreeEvent.Name;

        public string Description => TreeEvent.Description;

        public TreeEventViewModel TrueEvent => TreeEvent.TrueEvent == null ? null : new TreeEventViewModel(TreeEvent.TrueEvent);

        public TreeEventViewModel FalseEvent => TreeEvent.FalseEvent == null ? null : new TreeEventViewModel(TreeEvent.FalseEvent);

        public bool IsEndPointEvent => TreeEvent.TrueEvent == null && TreeEvent.FalseEvent == null;

        public bool HasTrueEventOnly => TreeEvent.TrueEvent != null && TreeEvent.FalseEvent == null;

        public bool HasFalseEventOnly => TreeEvent.TrueEvent == null && TreeEvent.FalseEvent != null;

        public bool HasTwoEvents => TreeEvent.TrueEvent != null && TreeEvent.FalseEvent != null;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
