using System.ComponentModel;
using Forest.Gui;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization
{
    public class GuiViewModelBase : ViewModelBase
    {
        protected readonly ForestGui Gui;

        protected GuiViewModelBase(ViewModelFactory factory, ForestGui gui) : base(factory)
        {
            Gui = gui;
            if (gui != null)
                gui.PropertyChanged += GuiPropertyChanged;
        }

        protected virtual void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}