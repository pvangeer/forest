using System.ComponentModel;
using Forest.Gui;

namespace Forest.Visualization.ViewModels
{
    public class GuiViewModelBase : ViewModelBase
    {
        protected readonly ForestGui Gui;

        protected GuiViewModelBase(ForestGui gui) : base(new ViewModelFactory(gui))
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