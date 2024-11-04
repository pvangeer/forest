namespace Forest.Visualization.ViewModels.MainContentPanel
{
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