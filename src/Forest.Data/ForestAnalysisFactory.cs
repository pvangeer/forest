namespace Forest.Data
{
    public static class ForestAnalysisFactory
    {
        public static ForestAnalysis CreateEmptyProject()
        {
            return new ForestAnalysis();
        }

        public static ForestAnalysis CreateStandardNewProject()
        {
            return new ForestAnalysis();
        }
    }
}