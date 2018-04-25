using System.Windows;
using System.Windows.Threading;
using StoryTree.Messaging;

namespace StoryTree.Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private readonly StoryTreeLog log = new StoryTreeLog(typeof(App));
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            log.Error($"Er is iets onverwachts misgegaan: {e.Exception.Message}");
            e.Handled = true;
        }
    }
}
