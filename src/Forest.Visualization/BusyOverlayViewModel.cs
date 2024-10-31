using System.ComponentModel;
using Forest.Gui;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization
{
    public class BusyOverlayViewModel : GuiViewModelBase
    {
        public BusyOverlayViewModel(ViewModelFactory factory, ForestGui gui) : base(factory, gui)
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