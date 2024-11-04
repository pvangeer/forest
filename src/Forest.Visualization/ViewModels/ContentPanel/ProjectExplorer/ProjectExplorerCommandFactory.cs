using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Forest.Data.Tree;
using Forest.Gui;
using Forest.Visualization.Commands;
using Forest.Visualization.TreeView.Commands;
using Forest.Visualization.TreeView.Data;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerCommandFactory
    {
        private readonly ForestGui gui;

        public ProjectExplorerCommandFactory(ForestGui gui)
        {
            this.gui = gui;
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

        public ICommand CreateRemoveEventTreeCommand(EventTree eventTree)
        {
            return new RemoveEventTreeCommand(gui, eventTree);
        }

        public ICommand CreateAddProbabilityEstimationCommand()
        {
            return new AddProbabilityEstimationPerTreeEventCommand(gui);
        }
    }
}