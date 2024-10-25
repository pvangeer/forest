using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Forest.Gui.Components;
using Forest.Messaging;
using Forest.Visualization.Commands;

namespace Forest.Visualization.ViewModels
{
    public class StatusBarViewModel : INotifyPropertyChanged
    {
        private readonly ForestGui gui;
        private MessageListViewModel messageListViewModel;

        public StatusBarViewModel() : this(new ForestGui())
        {
        }

        public StatusBarViewModel(ForestGui gui)
        {
            this.gui = gui;
            if (this.gui != null)
            {
                this.gui.PropertyChanged += GuiPropertyChanged;
                this.gui.Messages.CollectionChanged += GuiMessagesCollectionChanged;
            }
        }

        public string ProjectFileName => string.IsNullOrEmpty(gui.ProjectFilePath)
            ? "Nieuw bestand*"
            : Path.GetFileNameWithoutExtension(gui.ProjectFilePath);

        public ICommand RemoveLastMessageCommand => new RemovePriorityMessageCommand(this);

        public ICommand ShowMessageListCommand => new ShowMessageListCommand(this);

        public bool ShowMessages { get; set; }

        public LogMessage PriorityMessage { get; set; }

        public StorageState BusyIndicator
        {
            get => gui.BusyIndicator;
            set => gui.BusyIndicator = value;
        }

        public MessageListViewModel MessagesViewModel =>
            messageListViewModel ?? (messageListViewModel = new MessageListViewModel(gui.Messages));

        public event PropertyChangedEventHandler PropertyChanged;

        private void GuiMessagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newItem = e.NewItems.OfType<LogMessage>().First();
                if (newItem.HasPriority)
                {
                    PriorityMessage = newItem;
                    OnPropertyChanged(nameof(PriorityMessage));
                }

                OnPropertyChanged(nameof(MessagesViewModel));
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var items = e.OldItems.OfType<LogMessage>();
                foreach (var logMessage in items)
                    if (PriorityMessage == logMessage)
                    {
                        PriorityMessage = null;
                        OnPropertyChanged(nameof(PriorityMessage));
                    }

                if (!MessagesViewModel.MessageList.Any())
                {
                    ShowMessages = false;
                    OnPropertyChanged(nameof(ShowMessages));
                }

                OnPropertyChanged(nameof(MessagesViewModel));
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                PriorityMessage = null;
                OnPropertyChanged(nameof(PriorityMessage));
                OnPropertyChanged(nameof(MessagesViewModel));
                ShowMessages = false;
                OnPropertyChanged(nameof(ShowMessages));
            }
        }

        private void GuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ForestGui.BusyIndicator):
                    OnPropertyChanged(nameof(BusyIndicator));
                    break;
                case nameof(ForestGui.Messages):
                    messageListViewModel = null;
                    OnPropertyChanged(nameof(MessagesViewModel));
                    break;
                case nameof(ForestGui.ProjectFilePath):
                    OnPropertyChanged(nameof(ProjectFileName));
                    break;
            }
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}