using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Forest.Data.Services;
using Forest.Data.Tree;
using Forest.Gui.Components;
using Forest.Visualization.Commands;

namespace Forest.Visualization.ViewModels
{
    public class RibbonViewModel : INotifyPropertyChanged
    {
        private readonly AnalysisManipulationService analysisManipulationService;
        private readonly ForestGui gui;

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
            }

            analysisManipulationService = new AnalysisManipulationService(gui.ForestAnalysis);

            AddTreeEventCommand = new AddTreeEventCommand(this);
            RemoveTreeEventCommand = new RemoveTreeEventCommand(this);
        }

        public ForestGuiState SelectedState
        {
            // TODO: Move to ForestGui as a property?
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


        public ICommand FileNewCommand => new FileNewCommand(this);

        public ICommand SaveProjectCommand => new SaveProjectCommand(this);

        public ICommand SaveProjectAsCommand => new SaveProjectAsCommand(this);

        public ICommand OpenProjectCommand => new OpenProjectCommand(this);

        public ICommand ChangeProcessStepCommand => new ChangeProcessStepCommand(this);

        public ICommand RemoveTreeEventCommand { get; }

        public ICommand AddTreeEventCommand { get; }

        public TreeEvent SelectedTreeEvent => gui.SelectionManager.SelectedTreeEvent;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NewProject()
        {
            gui.GuiProjectServices.NewProject();
        }

        public void OpenProject()
        {
            gui.GuiProjectServices.OpenProject();
        }

        public bool CanSaveProject()
        {
            return gui.ForestAnalysis != null;
        }

        public void SaveProject()
        {
            gui.GuiProjectServices.SaveProject();
        }

        public void SaveProjectAs()
        {
            gui.GuiProjectServices.SaveProjectAs();
        }

        public void AddTreeEvent(TreeEventType treeEventType)
        {
            var newTreeEvent = analysisManipulationService.AddTreeEvent(gui.ForestAnalysis.EventTree, SelectedTreeEvent, treeEventType);
            gui.SelectionManager.SelectTreeEvent(newTreeEvent);
        }

        public void RemoveTreeEvent(TreeEventType eventType)
        {
            var parent = analysisManipulationService.RemoveTreeEvent(gui.ForestAnalysis.EventTree, gui.SelectionManager.SelectedTreeEvent);
            gui.SelectionManager.SelectTreeEvent(parent ?? gui.ForestAnalysis.EventTree.MainTreeEvent);
        }

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public bool CanRemoveSelectedTreeEvent()
        {
            return SelectedTreeEvent != gui.ForestAnalysis.EventTree.MainTreeEvent;
        }
    }
}