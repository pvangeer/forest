using System.Windows.Input;

namespace Forest.Visualization.TreeView.ViewModels
{
    public class ContextMenuItemViewModel
    {
        public bool IsEnabled { get; set; }

        public string Header { get; set; }

        public ICommand Command { get; set; }

        public string IconReference { get; set; }
    }
}