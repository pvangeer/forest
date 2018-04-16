using StoryTree.Gui.ViewModels;
using StoryTree.Storage;

namespace StoryTree.Gui
{
    public partial class GuiProjectServices
    {
        private class BackgroundWorkerArguments
        {
            public BackgroundWorkerArguments(StorageSqLite storageSqLite, GuiViewModel guiViewModel)
            {
                StorageSqLite = storageSqLite;
                Gui = guiViewModel.Gui;
                ProjectFilePath = guiViewModel.ProjectFilePath;
            }

            public string ProjectFilePath { get; }

            public StorageSqLite StorageSqLite { get; }

            public StoryTreeGui Gui { get; }
        }
    }
}
