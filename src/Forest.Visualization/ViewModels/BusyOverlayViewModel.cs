using System.ComponentModel;
using Forest.Gui;

namespace Forest.Visualization.ViewModels
{
    public class BusyOverlayViewModel : GuiViewModelBase
    {
        public BusyOverlayViewModel(ForestGui gui) : base(gui)
        {
        }

        public StorageState BusyIndicator
        {
            get => Gui.BusyIndicator;
            set => Gui.BusyIndicator = value;
        }


        protected override void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.BusyIndicator):
                    OnPropertyChanged(nameof(BusyIndicator));
                    break;
            }
        }
    }
}