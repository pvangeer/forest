using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StoryTree.Data.Annotations;
using StoryTree.Gui.Command;

namespace StoryTree.Gui.ViewModels
{
    public class GuiViewModel : INotifyPropertyChanged
    {
        private ProjectViewModel projectViewModel;

        public GuiViewModel() : this(new StoryTreeGui()) { }

        public GuiViewModel(StoryTreeGui gui)
        {
            GuiProjectSercices = new GuiProjectServices(this);
            Gui = gui;
            if (Gui != null)
            {
                Gui.PropertyChanged += GuiPropertyChanged;
                projectViewModel = new ProjectViewModel(Gui.Project);
            }
        }

        public GuiProjectServices GuiProjectSercices { get; }

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

        public Window Win32Window
        {
            get => GuiProjectSercices.Win32Window;
            set => GuiProjectSercices.Win32Window = value;
        }


        public ICommand FileNewCommand => new FileNewCommnd(this);

        public ICommand SaveProjectCommand => new SaveProjectCommand(this);

        public ICommand SaveProjectAsCommand => new SaveProjectAsCommand(this);

        public ICommand OpenProjectCommand => new OpenProjectCommand(this);

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler OnInvalidateVisual;

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

        public void InvokeInvalidateVisual()
        {
            OnInvalidateVisual?.Invoke(this, null);
        }

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}