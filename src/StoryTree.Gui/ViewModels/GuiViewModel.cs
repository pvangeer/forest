using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StoryTree.Data.Annotations;
using StoryTree.Gui.Command;
using StoryTree.Messaging;

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
                Gui.Messages.CollectionChanged += GuiMessagesCollectionChanged;
                projectViewModel = new ProjectViewModel(Gui.Project);
            }
        }

        private void GuiMessagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newItem = e.NewItems.OfType<LogMessage>().First();
                if (newItem.HasPriority)
                {
                    LastErrorMessage = newItem;
                    OnPropertyChanged(nameof(LastErrorMessage));
                }
                OnPropertyChanged(nameof(Messages));
            }
        }

        public bool ShowMessages { get; set; }

        public LogMessage LastErrorMessage { get; set; }

        public GuiProjectServices GuiProjectSercices { get; }

        public StoryTreeGui Gui { get; }

        public StorageState BusyIndicator
        {
            get => Gui.BusyIndicator;
            set => Gui.BusyIndicator = value;
        }

        public ProjectViewModel ProjectViewModel => projectViewModel;

        public ObservableCollection<LogMessage> Messages => Gui.Messages;

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

        public ICommand RemoveLastMessageCommand => new RemoveLastMessageCommand(this);

        public ICommand ShowMessageListCommand => new ShowMessageListCommand(this);

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