using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class TreeEventViewModel
    {
        public TreeEventViewModel(TreeEvent treeEvent)
        {
            this.TreeEvent = treeEvent;
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
        
        public TreeEventViewModel Self => this;
    }
}
