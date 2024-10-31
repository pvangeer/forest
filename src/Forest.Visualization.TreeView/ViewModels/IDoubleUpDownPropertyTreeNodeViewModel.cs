using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.TreeView.ViewModels
{
    public interface IDoubleUpDownPropertyTreeNodeViewModel : ITreeNodeViewModel
    {
        double DoubleValue { get; set; }

        double MinValue { get; }

        double MaxValue { get; }

        double Increment { get; }

        string StringFormat { get; }
    }
}