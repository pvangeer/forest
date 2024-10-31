﻿using System;
using System.Windows.Input;
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
    }
}