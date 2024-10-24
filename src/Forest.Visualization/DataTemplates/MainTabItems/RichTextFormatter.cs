using System.Windows.Documents;
using Xceed.Wpf.Toolkit;

namespace Forest.Visualization.DataTemplates.MainTabItems
{
    public class RichTextFormatter : ITextFormatter
    {
        private readonly RtfFormatter rtfFormatter = new RtfFormatter();
        private string emptyText;

        public string GetText(FlowDocument document)
        {
            if (emptyText == null)
                emptyText = rtfFormatter.GetText(document);

            var text = rtfFormatter.GetText(document);
            return text == emptyText ? "" : text;
        }

        public void SetText(FlowDocument document, string text)
        {
            rtfFormatter.SetText(document, text);
        }
    }
}