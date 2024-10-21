using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Estimations;
using StoryTree.Data.Tree;
using StoryTree.Gui.Annotations;

namespace StoryTree.Gui.ViewModels
{
    public class ProbabilitySpecificationViewModelBase : INotifyPropertyChanged
    {
        public ProbabilitySpecificationViewModelBase([NotNull] TreeEvent treeEvent)
        {
            TreeEvent = treeEvent;
            TreeEvent.PropertyChanged += TreeEventPropertyChanged;
        }

        public TreeEvent TreeEvent { get; }

        public ProbabilitySpecificationType Type => TreeEvent.ProbabilitySpecificationType;
        public event PropertyChangedEventHandler PropertyChanged;

        private void TreeEventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnTreeEventPropertyChanged(e.PropertyName);
        }

        protected virtual void OnTreeEventPropertyChanged(string propertyName)
        {
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}