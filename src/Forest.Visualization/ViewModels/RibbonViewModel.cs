using System.ComponentModel;
using System.Windows.Input;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.Commands;

namespace Forest.Visualization.ViewModels
{
    public class RibbonViewModel : GuiViewModelBase
    {
        private readonly CommandFactory commandFactory;

        public RibbonViewModel() : this(new ForestGui())
        {
        }

        public RibbonViewModel(ForestGui gui) : base(gui)
        {
            if (Gui != null)
            {
                Gui.SelectionManager.PropertyChanged += SelectionManagerPropertyChanged;
                commandFactory = new CommandFactory(gui);
            }
        }

        public StorageState BusyIndicator
        {
            get => Gui.BusyIndicator;
            set
            {
                Gui.BusyIndicator = value;
                Gui.OnPropertyChanged();
            }
        }


        public ICommand FileNewCommand => commandFactory.CreateNewProjectCommand();

        public ICommand SaveProjectCommand => commandFactory.CreateSaveProjectCommand();

        public ICommand SaveProjectAsCommand => commandFactory.CreateSaveProjectAsCommand();

        public ICommand OpenProjectCommand => commandFactory.CreateOpenProjectCommand();

        public ICommand RemoveTreeEventCommand => commandFactory.CreateRemoveTreeEventCommand();

        public ICommand AddTreeEventCommand => commandFactory.CreateAddTreeEventCommand();

        public TreeEvent SelectedTreeEvent => Gui.SelectionManager.SelectedTreeEvent;

        public ICommand EscapeCommand => commandFactory.CreateEscapeCommand();

        public ICommand AddEventTreeCommand => commandFactory.CreateAddEventTreeCommand();

        public ICommand RemoveEventTreeCommand => commandFactory.CreateRemoveEventTreeCommand();

        protected override void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.BusyIndicator):
                    OnPropertyChanged(nameof(BusyIndicator));
                    break;
            }
        }

        private void SelectionManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectionManager.SelectedTreeEvent):
                    OnPropertyChanged(nameof(SelectedTreeEvent));
                    break;
            }
        }
    }
}