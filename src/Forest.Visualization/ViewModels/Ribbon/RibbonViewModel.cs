using System;
using System.ComponentModel;
using System.Windows.Input;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.Commands;

namespace Forest.Visualization.ViewModels.Ribbon
{
    public class RibbonViewModel : GuiViewModelBase
    {
        private readonly CommandFactory commandFactory;
        private ICommand saveCanvasCommand;
        public RibbonViewModel(ForestGui gui) : base(gui)
        {
            if (Gui != null)
            {
                Gui.SelectionManager.SelectedTreeEventChanged += SelectedTreeEventChanged;
                Gui.SelectionManager.PropertyChanged += SelectionManagerPropertyChanged;
                commandFactory = new CommandFactory(gui);

                AboutBoxViewModel = ViewModelFactory.CreateAboutBoxViewModel();
            }
        }

        public AboutBoxViewModel AboutBoxViewModel { get; }

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

        public ICommand SaveImageCommand =>
            saveCanvasCommand ?? (saveCanvasCommand = commandFactory.CreateSaveImageCommand());

        public TreeEvent SelectedTreeEvent =>
            Gui.SelectionManager.Selection is EventTree selectedEventTree
                ? Gui.SelectionManager.SelectedTreeEvent[selectedEventTree]
                : null;

        public ICommand EscapeCommand => commandFactory.CreateEscapeCommand();

        public ICommand AddEventTreeCommand => commandFactory.CreateAddEventTreeCommand();

        public ICommand RemoveEventTreeCommand => commandFactory.CreateRemoveEventTreeCommand();

        public bool IsDetailsPanelVisible
        {
            get => Gui.IsDetailsPanelVisible;
            set
            {
                Gui.IsDetailsPanelVisible = value;
                Gui.OnPropertyChanged();
            }
        }

        public bool CanShowDetailsPanel => Gui?.SelectionManager.Selection is EventTree;

        protected override void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.BusyIndicator):
                    OnPropertyChanged(nameof(BusyIndicator));
                    break;
                case nameof(ForestGui.ForestAnalysis):
                    OnPropertyChanged(nameof(AddEventTreeCommand));
                    OnPropertyChanged(nameof(RemoveEventTreeCommand));
                    OnPropertyChanged(nameof(RemoveTreeEventCommand));
                    OnPropertyChanged(nameof(AddTreeEventCommand));
                    OnPropertyChanged(nameof(EscapeCommand));
                    OnPropertyChanged(nameof(FileNewCommand));
                    OnPropertyChanged(nameof(OpenProjectCommand));
                    OnPropertyChanged(nameof(SelectedTreeEvent));
                    OnPropertyChanged(nameof(SaveProjectAsCommand));
                    OnPropertyChanged(nameof(SaveProjectCommand));
                    break;
                case nameof(ForestGui.IsDetailsPanelVisible):
                    OnPropertyChanged(nameof(IsDetailsPanelVisible));
                    break;
            }
        }

        private void SelectedTreeEventChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(SelectedTreeEvent));
        }

        private void SelectionManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectionManager.SelectedTreeEvent):
                    OnPropertyChanged(nameof(SelectedTreeEvent));
                    break;
                case nameof(SelectionManager.Selection):
                    OnPropertyChanged(nameof(CanShowDetailsPanel));
                    break;
            }
        }
    }
}