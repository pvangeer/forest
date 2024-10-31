using System.Windows.Media;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.TreeView.ViewModels
{
    public interface IColorPropertyTreeNodeViewModel : ITreeNodeViewModel
    {
        Color ColorValue { get; set; }
    }
}