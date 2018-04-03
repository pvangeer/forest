using System.Collections.ObjectModel;
using StoryTree.Data.Tree;

namespace StoryTree.Data
{
    public class FragilityCurve : ObservableCollection<FragilityCurveElement>
    {
        public FragilityCurve(TreeEvent treeEvent)
        {
            TreeEvent = treeEvent;
        }

        public TreeEvent TreeEvent { get; }
    }
}