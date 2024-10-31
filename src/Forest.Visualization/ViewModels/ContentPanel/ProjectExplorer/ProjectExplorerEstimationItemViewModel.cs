using System.Collections.ObjectModel;
using System.Windows.Input;
using Forest.Data.Estimations;
using Forest.Data.Services;
using Forest.Gui;
using Forest.Visualization.TreeView.Data;
using Forest.Visualization.TreeView.ViewModels;

namespace Forest.Visualization.ViewModels.ContentPanel.ProjectExplorer
{
    public class ProjectExplorerEstimationItemViewModel : ProjectExplorerItemViewModelBase
    {
        private readonly ProbabilityEstimation estimation;
        private readonly ProjectExplorerCommandFactory commandFactory;
        private readonly ForestGui gui;

        public ProjectExplorerEstimationItemViewModel(ProbabilityEstimation estimation, ForestGui gui) : base(new ViewModelFactory(gui))
        {
            this.estimation = estimation;
            this.gui = gui;
            commandFactory = new ProjectExplorerCommandFactory(gui);
        }

        public override ICommand SelectItemCommand => commandFactory.CreateSelectItemCommand(this);

        public override object GetSelectableObject()
        {
            return estimation;
        }

        public override string DisplayName => estimation.Name;

        public override string IconSourceString =>
            "pack://application:,,,/Forest.Visualization;component/Resources/ProjectExplorer/probability_estimation.ico";

        public override bool IsViewModelFor(object item)
        {
            return item is ProbabilityEstimation probability && probability == estimation;
        }

        public override ICommand RemoveItemCommand => commandFactory.CreateCanAlwaysExecuteActionCommand(o =>
        {
            var service = new AnalysisManipulationService(gui.ForestAnalysis);
            service.RemoveProbabilityEstimation(estimation);
        });
    }

    public abstract class ProjectExplorerItemViewModelBase : ViewModelBase, ITreeNodeCollectionViewModel
    {
        protected ProjectExplorerItemViewModelBase(ViewModelFactory viewModelFactory) : base(viewModelFactory)
        {
            ContextMenuItems = new ObservableCollection<ContextMenuItemViewModel>();
        }

        public bool IsExpandable => false;

        public bool IsExpanded { get; set; }

        public ICommand ToggleIsExpandedCommand => null;

        public bool CanSelect => true;

        public bool IsSelected { get; set; }

        public abstract ICommand SelectItemCommand { get; }

        public abstract object GetSelectableObject();

        public abstract string DisplayName { get; }

        public abstract string IconSourceString { get; }

        public bool CanRemove => true;

        public abstract ICommand RemoveItemCommand { get; }

        public bool CanAdd => false;

        public ICommand AddItemCommand => null;

        public bool CanOpen => false;

        // TODO: Remove and always open view on selection change.
        public ICommand OpenViewCommand => null;

        public ObservableCollection<ContextMenuItemViewModel> ContextMenuItems { get; protected set; }

        public abstract bool IsViewModelFor(object item);

        public ObservableCollection<ITreeNodeViewModel> Items => new ObservableCollection<ITreeNodeViewModel>();

        public CollectionType CollectionType => CollectionType.PropertyItemsCollection;
    }
}