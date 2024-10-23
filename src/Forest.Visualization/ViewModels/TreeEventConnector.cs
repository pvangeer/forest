using QuickGraph;

namespace Forest.Visualization.ViewModels
{
    public class TreeEventConnector : Edge<GraphVertex>
    {
        public TreeEventConnector(GraphVertex sourceEvent, GraphVertex targetEvent) : base(sourceEvent, targetEvent)
        {
        }

        public bool PassingEvent => Target.TreeEventViewModel != null &&
                                    ReferenceEquals(Source.TreeEventViewModel.PassingEvent, Target.TreeEventViewModel);
    }
}