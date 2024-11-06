using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Forest.Data.Estimations;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Gui;
using Forest.Visualization.Commands;
using Forest.Visualization.TreeView.Data;
using Forest.Visualization.TreeView.ViewModels;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerProbabilityEstimationCollectionViewModelBase : ItemsCollectionViewModelBase
    {
        protected readonly CommandFactory CommandFactory;
        protected readonly ForestGui Gui;

        public ProjectExplorerProbabilityEstimationCollectionViewModelBase(ForestGui gui) : base(new ViewModelFactory(gui))
        {
            Gui = gui;
            if (Gui != null)
                Gui.PropertyChanged += GuiPropertyChanged;

            Items = new ObservableCollection<ITreeNodeViewModel>();
            if (Gui?.ForestAnalysis?.ProbabilityEstimationsPerTreeEvent != null)
            {
                Gui.ForestAnalysis.ProbabilityEstimationsPerTreeEvent.CollectionChanged += EstimationsPerEventCollectionChanged;
                foreach (var estimation in Gui.ForestAnalysis.ProbabilityEstimationsPerTreeEvent)
                    Items.Add(ViewModelFactory.CreateProjectExplorerEstimationItemViewModel(estimation));
            }

            CommandFactory = new CommandFactory(gui);
            ContextMenuItems = new ObservableCollection<ContextMenuItemViewModel>();
        }


        public override bool IsExpandable => true;

        public override ICommand ToggleIsExpandedCommand => CommandFactory.CreateToggleIsExpandedCommand(this);

        public override bool CanAdd => true;

        public override ICommand AddItemCommand => CommandFactory.CreateAddProbabilityEstimationCommand();

        private void EstimationsPerEventCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var estimation in e.NewItems.OfType<ProbabilityEstimationPerTreeEvent>())
                {
                    var viewModel = ViewModelFactory.CreateProjectExplorerEstimationItemViewModel(estimation);
                    Items.Add(viewModel);
                    if (IsExpanded)
                        viewModel.SelectItemCommand?.Execute(null);
                }

            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (var estimation in e.OldItems.OfType<ProbabilityEstimationPerTreeEvent>())
                {
                    var estimationToRemove = Items.FirstOrDefault(i => i.IsViewModelFor(estimation));
                    if (estimationToRemove != null)
                        Items.Remove(estimationToRemove);
                }
        }

        protected void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.ForestAnalysis):
                    Items.Clear();
                    if (Gui.ForestAnalysis != null)
                        Gui.ForestAnalysis.ProbabilityEstimationsPerTreeEvent.CollectionChanged += EstimationsPerEventCollectionChanged;
                    break;
            }
        }
    }
}