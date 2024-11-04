using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.TreeView.Commands;
using Forest.Visualization.TreeView.Data;
using System;
using System.Windows.Input;

namespace Forest.Visualization.Commands
{
    public class CommandFactory
    {
        private readonly ForestGui gui;

        public CommandFactory(ForestGui gui)
        {
            this.gui = gui;
        }

        public RelayCommand CreateSaveImageCommand()
        {
            return new RelayCommand(() =>
                {
                    if (gui != null)
                    {
                        gui.IsSaveToImage = true;
                        gui.OnPropertyChanged(nameof(ForestGui.IsSaveToImage));
                        gui.IsSaveToImage = false;
                        gui.OnPropertyChanged(nameof(ForestGui.IsSaveToImage));
                    }
                },
                () => false); // TODO: Check whether there is something to export
        }

        public ICommand CreateEscapeCommand()
        {
            return new EscapeCurrentActionCommand(gui);
        }

        public ICommand CreateOpenProjectCommand()
        {
            return new OpenProjectCommand(gui);
        }

        public ICommand CreateSaveProjectCommand()
        {
            return new SaveProjectCommand(gui);
        }

        public ICommand CreateSaveProjectAsCommand()
        {
            return new SaveProjectAsCommand(gui);
        }

        public ICommand CreateNewProjectCommand()
        {
            return new FileNewCommand(gui);
        }

        public ICommand CreateRemoveTreeEventCommand()
        {
            return new RemoveTreeEventCommand(gui);
        }

        public ICommand CreateAddTreeEventCommand()
        {
            return new AddTreeEventCommand(gui);
        }

        public ICommand CreateToggleIsExpandedCommand(IExpandable expandableContentViewModel)
        {
            return new ToggleIsExpandedCommand(expandableContentViewModel);
        }

        public ICommand CreateCanAlwaysExecuteActionCommand(Action<object> action)
        {
            return new CanAlwaysExecuteActionCommand
            {
                ExecuteAction = action
            };
        }

        public ICommand CreateSelectItemCommand(ISelectable selectable)
        {
            return new SelectItemCommand(gui.SelectionManager, selectable);
        }

        public ICommand CreateAddEventTreeCommand()
        {
            return new AddEventTreeCommand(gui);
        }

        public ICommand CreateRemoveEventTreeCommand(EventTree eventTree = null)
        {
            return new RemoveEventTreeCommand(gui, eventTree);
        }

        public ICommand CreateAddProbabilityEstimationCommand()
        {
            return new AddProbabilityEstimationPerTreeEventCommand(gui);
        }

    }
}
