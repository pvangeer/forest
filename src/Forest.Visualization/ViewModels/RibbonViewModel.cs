using System.ComponentModel;
using System.Windows.Input;
using Forest.Data;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.Commands;

namespace Forest.Visualization.ViewModels
{
    public class RibbonViewModel : NotifyPropertyChangedObject
    {
        private readonly ForestGui gui;
        private readonly CommandFactory commandFactory;

        public RibbonViewModel() : this(new ForestGui())
        {
        }

        public RibbonViewModel(ForestGui gui)
        {
            this.gui = gui;
            if (gui != null)
            {
                gui.PropertyChanged += GuiPropertyChanged;
                gui.SelectionManager.PropertyChanged += SelectionManagerPropertyChanged;

                commandFactory = new CommandFactory(gui);
            }
        }

        public ForestGuiState SelectedState
        {
            get => gui.SelectedState;
            set
            {
                gui.SelectedState = value;
                OnPropertyChanged();
                gui.OnPropertyChanged();
            }
        }

        public StorageState BusyIndicator
        {
            get => gui.BusyIndicator;
            set => gui.BusyIndicator = value;
        }


        public ICommand FileNewCommand => commandFactory.CreateNewProjectCommand();

        public ICommand SaveProjectCommand => commandFactory.CreateSaveProjectCommand();

        public ICommand SaveProjectAsCommand => commandFactory.CreateSaveProjectAsCommand();

        public ICommand OpenProjectCommand => commandFactory.CreateOpenProjectCommand();

        public ICommand ChangeProcessStepCommand => commandFactory.CreateChangeProcessStepCommand();

        public ICommand RemoveTreeEventCommand => commandFactory.CreateRemoveTreeEventCommand();

        public ICommand AddTreeEventCommand => commandFactory.CreateAddTreeEventCommand();

        public TreeEvent SelectedTreeEvent => gui.SelectionManager.SelectedTreeEvent;

        public ICommand EscapeCommand => commandFactory.CreateEscapeCommand();

        private void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.SelectedState):
                    OnPropertyChanged(nameof(SelectedState));
                    break;
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