using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data.Annotations;

namespace StoryTree.Gui.ViewModels
{
    public class GuiViewModel : INotifyPropertyChanged
    {
        private ProjectViewModel projectViewModel;

        public GuiViewModel() : this(new StoryTreeGui()) { }

        public GuiViewModel(StoryTreeGui gui)
        {
            Gui = gui;
            if (Gui != null)
            {
                Gui.PropertyChanged += GuiPropertyChanged;
                projectViewModel = new ProjectViewModel(Gui.Project);
            }
        }

        public StoryTreeGui Gui { get; }

        public StorageState BusyIndicator
        {
            get => Gui.BusyIndicator;
            set => Gui.BusyIndicator = value;
        }

        public ProjectViewModel ProjectViewModel => projectViewModel;

        public string ProjectFilePath
        {
            get => Gui.ProjectFilePath;
            set => Gui.ProjectFilePath = value;
        }

        private void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(StoryTreeGui.BusyIndicator):
                    OnPropertyChanged(nameof(BusyIndicator));
                    break;
                case nameof(StoryTreeGui.Project):
                    projectViewModel = new ProjectViewModel(Gui.Project);
                    OnPropertyChanged(nameof(ProjectViewModel));
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}