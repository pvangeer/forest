using System;
using System.ComponentModel;
using System.Windows.Input;
using Forest.Data;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.Commands;

namespace Forest.Visualization.ViewModels.ContentPanel.MainContentPresenter.EventTree
{
    public class TreeEventViewModel : Entity
    {
        private readonly CommandFactory commandFactory;
        private readonly Data.Tree.EventTree eventTree;
        private readonly ForestGui gui;
        private readonly TreeEvent treeEvent;
        private readonly ViewModelFactory viewModelFactory;
        private TreeEventViewModel failingEventViewModel;
        private TreeEventViewModel passingEventViewModel;

        public TreeEventViewModel(TreeEvent treeEvent, Data.Tree.EventTree eventTree, ForestGui gui)
        {
            this.gui = gui;
            this.eventTree = eventTree;
            viewModelFactory = new ViewModelFactory(gui);
            commandFactory = new CommandFactory(gui);
            if (gui != null)
                gui.SelectionManager.SelectedTreeEventChanged += SelectedTreeEventChanged;
            this.treeEvent = treeEvent;

            if (treeEvent != null)
                treeEvent.PropertyChanged += TreeEventPropertyChanged;
        }

        public string Name
        {
            get => treeEvent.Name;
            set
            {
                treeEvent.Name = value;
                treeEvent.OnPropertyChanged();
            }
        }

        public string Summary
        {
            get => treeEvent.Summary;
            set
            {
                treeEvent.Summary = value;
                treeEvent.OnPropertyChanged();
            }
        }

        public string Information
        {
            get => treeEvent.Information;
            set
            {
                treeEvent.Information = value;
                treeEvent.OnPropertyChanged();
            }
        }

        public TreeEventViewModel PassingEvent
        {
            get
            {
                if (treeEvent?.PassingEvent == null)
                    return null;

                return passingEventViewModel ?? (passingEventViewModel =
                    viewModelFactory.CreateTreeEventViewModel(treeEvent.PassingEvent, eventTree));
            }
        }

        public TreeEventViewModel FailingEvent
        {
            get
            {
                if (treeEvent?.FailingEvent == null)
                    return null;

                return failingEventViewModel ?? (failingEventViewModel =
                    viewModelFactory.CreateTreeEventViewModel(treeEvent.FailingEvent, eventTree));
            }
        }

        public bool IsEndPointEvent => treeEvent.PassingEvent == null && treeEvent.FailingEvent == null;

        public bool HasTrueEventOnly => treeEvent.PassingEvent != null && treeEvent.FailingEvent == null;

        public bool HasFalseEventOnly => treeEvent.PassingEvent == null && treeEvent.FailingEvent != null;

        public bool HasTwoEvents => treeEvent.PassingEvent != null && treeEvent.FailingEvent != null;

        public ICommand TreeEventClickedCommand => commandFactory.CreateTreeEventClickedCommand(this);

        public bool IsSelected => treeEvent != null && ReferenceEquals(treeEvent, gui?.SelectionManager.GetSelectedTreeEvent(eventTree));

        private void SelectedTreeEventChanged(object sender, EventArgs eventArgs)
        {
            OnPropertyChanged(nameof(IsSelected));
        }

        public void Select()
        {
            gui.SelectionManager.SelectTreeEvent(eventTree, treeEvent);
        }

        private void TreeEventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(treeEvent.PassingEvent):
                    passingEventViewModel = null;
                    if (PassingEvent == null)
                        // TODO: Shouldn't this be done by the command?
                        Select();
                    OnPropertyChanged(nameof(PassingEvent));
                    OnPropertyChanged(nameof(IsEndPointEvent));
                    OnPropertyChanged(nameof(HasTrueEventOnly));
                    OnPropertyChanged(nameof(HasFalseEventOnly));
                    OnPropertyChanged(nameof(HasTwoEvents));
                    break;
                case nameof(treeEvent.FailingEvent):
                    failingEventViewModel = null;
                    if (FailingEvent == null)
                        // TODO: Shouldn't this be done by the command?
                        Select();
                    OnPropertyChanged(nameof(FailingEvent));
                    OnPropertyChanged(nameof(IsEndPointEvent));
                    OnPropertyChanged(nameof(HasTrueEventOnly));
                    OnPropertyChanged(nameof(HasFalseEventOnly));
                    OnPropertyChanged(nameof(HasTwoEvents));
                    break;
                case nameof(treeEvent.Name):
                    OnPropertyChanged(nameof(Name));
                    break;
                case nameof(treeEvent.Summary):
                    OnPropertyChanged(nameof(Summary));
                    break;
                case nameof(treeEvent.Information):
                    OnPropertyChanged(nameof(Information));
                    break;
            }
        }
    }
}