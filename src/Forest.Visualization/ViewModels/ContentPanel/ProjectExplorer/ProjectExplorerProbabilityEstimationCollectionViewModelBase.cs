using Forest.Gui;
using Forest.Visualization.TreeView.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Forest.Data.Estimations;
using Forest.Visualization.TreeView.ViewModels;
using Forest.Data.Services;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerProbabilityEstimationCollectionViewModelBase : ItemsCollectionViewModelBase
    {
        protected readonly ProjectExplorerCommandFactory CommandFactory;
        protected readonly ForestGui Gui;

        public ProjectExplorerProbabilityEstimationCollectionViewModelBase(ForestGui gui) : base(new ViewModelFactory(gui))
        {
            this.Gui = gui;
            if (Gui != null)
            {
                Gui.PropertyChanged += GuiPropertyChanged;
            }

            Items = new ObservableCollection<ITreeNodeViewModel>();
            if (Gui?.ForestAnalysis?.ProbabilityEstimations != null)
            {
                Gui.ForestAnalysis.ProbabilityEstimations.CollectionChanged += EstimationsCollectionChanged;
                foreach (var estimation in Gui.ForestAnalysis.ProbabilityEstimations)
                {
                    Items.Add(ViewModelFactory.CreateProjectExplorerEstimationItemViewModel(estimation));
                }
            }

            CommandFactory = new ProjectExplorerCommandFactory(gui);
            ContextMenuItems = new ObservableCollection<ContextMenuItemViewModel>();
        }

        
        public override bool IsExpandable => true;

        public override ICommand ToggleIsExpandedCommand => CommandFactory.CreateToggleIsExpandedCommand(this);

        public override bool CanAdd => true;

        // TODO: Can not execute in case there is no eventtree..
        public override ICommand AddItemCommand => CommandFactory.CreateCanAlwaysExecuteActionCommand(p =>
        {
            var service = new AnalysisManipulationService(Gui.ForestAnalysis);
            service.AddProbabilityEstimationPerTreeEvent(Gui.ForestAnalysis.EventTrees.FirstOrDefault());
        });

        private void EstimationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var estimation in e.NewItems.OfType<ProbabilityEstimation>())
                {
                    var viewModel = ViewModelFactory.CreateProjectExplorerEstimationItemViewModel(estimation);
                    Items.Add(viewModel);
                    if (IsExpanded)
                    {
                        viewModel.SelectItemCommand?.Execute(null);
                    }
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var estimation in e.OldItems.OfType<ProbabilityEstimation>())
                {
                    var estimationToRemove = Items.FirstOrDefault(i => i.IsViewModelFor(estimation));
                    if (estimationToRemove != null)
                    {
                        Items.Remove(estimationToRemove);
                    }
                }
            }
        }

        protected void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.ForestAnalysis):
                    Items.Clear();
                    if (Gui.ForestAnalysis != null)
                        Gui.ForestAnalysis.ProbabilityEstimations.CollectionChanged += EstimationsCollectionChanged;
                    break;
            }
        }
    }
}