using System.Windows.Media;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.TreeView.ViewModels
{
    internal interface IFontFamilyPropertyTreeNodeViewModel : ITreeNodeViewModel
    {
        FontFamily SelectedValue { get; set; }
    }
}