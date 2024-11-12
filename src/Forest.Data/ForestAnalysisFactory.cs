using Forest.Data.Services;

namespace Forest.Data
{
    public static class ForestAnalysisFactory
    {
        public static ForestAnalysis CreateEmptyAnalysis()
        {
            return new ForestAnalysis();
        }

        public static ForestAnalysis CreateStandardNewAnalysis()
        {
            return new ForestAnalysis
            {
                Name = "Nieuw project",
                AssessmentSection = "1-1"
            };
        }
    }
}