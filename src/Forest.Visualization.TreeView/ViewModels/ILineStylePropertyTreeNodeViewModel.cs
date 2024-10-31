using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.TreeView.ViewModels
{
    public interface ILineStylePropertyTreeNodeViewModel : ITreeNodeViewModel
    {
        LineStyle LineStyleValue { get; set; }
    }
}