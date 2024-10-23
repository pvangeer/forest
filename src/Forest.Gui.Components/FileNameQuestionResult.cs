namespace Forest.Gui.Components
{
    public class FileNameQuestionResult
    {
        public FileNameQuestionResult(bool proceed, string fileName)
        {
            Proceed = proceed;
            FileName = fileName;
        }
        public string FileName { get; }

        public bool Proceed { get; }
    }
}