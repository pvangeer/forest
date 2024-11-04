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
            var analysis = new ForestAnalysis
            {
                Name = "Nieuw project",
                AssessmentSection = "1-1"
            };

            var service = new AnalysisManipulationService(analysis);
            service.AddProbabilityEstimationPerTreeEvent(service.AddEventTree());
            return analysis;
        }
    }
}