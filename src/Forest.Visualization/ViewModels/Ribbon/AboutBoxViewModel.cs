using System.Diagnostics;
using System.Windows.Input;
using Forest.Storage;
using Forest.Visualization.Commands;

namespace Forest.Visualization.ViewModels.Ribbon
{
    public class AboutBoxViewModel : ViewModelBase
    {
        public AboutBoxViewModel(ViewModelFactory factory) : base(factory)
        {
        }

        public string Version => $"{VersionInfo.Year}.{VersionInfo.MajorVersion}.{VersionInfo.MinorVersion}";

        public ICommand ExecuteHyperlinkCommand => new CanAlwaysExecuteActionCommand
        {
            ExecuteAction = OnExecuteHyperlink
        };

        private void OnExecuteHyperlink(object obj)
        {
            Process.Start((string)obj);
        }
    }
}