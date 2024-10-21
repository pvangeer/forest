using QuickGraph;

namespace StoryTree.Gui.ViewModels
{
    public class EventTreeGraph : BidirectionalGraph<GraphVertex, TreeEventConnector>
    {
    }

    public class GraphVertex
    {
        public GraphVertex(bool isPassingEndPoint)
        {
            TreeEventViewModel = null;
            IsPassingEndPoint = isPassingEndPoint;
            IsFailingEndPoint = !isPassingEndPoint;
        }

        public GraphVertex(TreeEventViewModel treeEventViewModel)
        {
            IsFailingEndPoint = false;
            IsPassingEndPoint = false;
            TreeEventViewModel = treeEventViewModel;
        }

        public bool IsFailingEndPoint { get; }

        public bool IsPassingEndPoint { get; }

        public TreeEventViewModel TreeEventViewModel { get; }
    }
}