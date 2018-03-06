using System.Collections.ObjectModel;

namespace StoryTree.Data.Tree
{
    public class ExpertClassEstimation
    {
        public ExpertClassEstimation()
        {
            Estimations = new ObservableCollection<ExpertClassEstimationPerWaterLevel>();
        }

        public Expert Expert { get; set; }

        public ObservableCollection<ExpertClassEstimationPerWaterLevel> Estimations { get; }
    }
}