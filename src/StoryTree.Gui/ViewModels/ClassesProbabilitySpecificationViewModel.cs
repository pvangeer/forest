using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Properties;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class ClassesProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public ClassesProbabilitySpecificationViewModel([NotNull]TreeEvent treeEvent, Project project) : base(treeEvent)
        {
            ClassesProbabilitySpecification = new ObservableCollection<ExpertClassEstimationViewmodel>(
                TreeEvent.ClassesProbabilitySpecification.Select(e => new ExpertClassEstimationViewmodel(e)));
        }

        public ObservableCollection<ExpertClassEstimationViewmodel> ClassesProbabilitySpecification { get; }
    }
}