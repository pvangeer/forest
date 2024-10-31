namespace Forest.Visualization.TreeView.Data
{
    public interface ISliderPropertyTreeNodeViewModel
    {
        double Value { get; set; }

        double MinValue { get; }

        double MaxValue { get; }
    }
}