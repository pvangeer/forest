using System.Collections.ObjectModel;
using System.Linq;
using Forest.Data;
using Forest.Data.Properties;
using Forest.Data.Tree;

namespace Forest.Visualization.ViewModels
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