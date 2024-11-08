﻿using Forest.Gui;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerProbabilityEstimationCollectionViewModel : ProjectExplorerProbabilityEstimationCollectionViewModelBase
    {
        public ProjectExplorerProbabilityEstimationCollectionViewModel(ForestGui gui) : base(gui)
        {
        }

        public override string DisplayName => "Faalkansinschattingen";
    }
}