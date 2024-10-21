using System.Collections.ObjectModel;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Properties;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class ClassesProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public ClassesProbabilitySpecificationViewModel([NotNull] TreeEvent treeEvent, EventTreeProject eventTreeProject) : base(treeEvent)
        {
            ClassesProbabilitySpecification = new ObservableCollection<ExpertClassEstimationViewModel>(
                TreeEvent.ClassesProbabilitySpecification.Select(e => new ExpertClassEstimationViewModel(e)));
        }

        public ObservableCollection<ExpertClassEstimationViewModel> ClassesProbabilitySpecification { get; }
    }
}