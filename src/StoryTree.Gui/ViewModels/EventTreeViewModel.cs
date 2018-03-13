using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data;
using StoryTree.Data.Properties;
using StoryTree.Data.Services;

namespace StoryTree.Gui.ViewModels
{
    public class EventTreeViewModel : INotifyPropertyChanged
    {
        private TreeEventViewModel selectedTreeEvent;
        private TreeEventViewModel mainTreeEventViewModel;
        private bool selected;

        private EventTree EventTree { get; }

        public EventTreeViewModel():this(new EventTree()) { }

        public EventTreeViewModel([NotNull]EventTree eventTree)
        {
            EventTree = eventTree;
            eventTree.PropertyChanged += EventTreePropertyChanged;
        }

        private void EventTreePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EventTree.MainTreeEvent):
                    mainTreeEventViewModel = null;
                    OnPropertyChanged(nameof(MainTreeEventViewModel));
                    break;
                case nameof(EventTree.Name):
                    OnPropertyChanged(nameof(Name));
                    break;
                case nameof(EventTree.Summary):
                    OnPropertyChanged(nameof(Summary));
                    break;
            }
        }

        public string Name
        {
            get => EventTree.Name;
            set
            {
                EventTree.Name= value;
                EventTree.OnPropertyChanged(nameof(EventTree.Name));
            }
        }

        public TreeEventViewModel MainTreeEventViewModel
        {
            get
            {
                if (mainTreeEventViewModel == null && EventTree.MainTreeEvent == null)
                {
                    return null;
                }

                return mainTreeEventViewModel ??
                       (mainTreeEventViewModel = new TreeEventViewModel(EventTree.MainTreeEvent, this));
            }
        }

        public TreeEventViewModel SelectedTreeEvent
        {
            get => selectedTreeEvent;
            set
            {
                selectedTreeEvent = value;
                MainTreeEventViewModel?.FireSelectedStateChangeRecursive();
                OnPropertyChanged(nameof(SelectedTreeEvent));
            }
        }

        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        public string Summary
        {
            get => EventTree.Summary;
            set => EventTree.Summary = value;
        }

        public string Details
        {
            get => EventTree.Details;
            set => EventTree.Details = value;
        }

        public bool IsViewModelFor(EventTree eventTree)
        {
            return Equals(EventTree, eventTree);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddTreeEvent(TreeEventViewModel treeEventViewModel, TreeEventType treeEventType)
        {
            EventTreeManipulationService.AddTreeEvent(EventTree,treeEventViewModel?.TreeEvent,treeEventType);
            SelectedTreeEvent = treeEventViewModel == null ? MainTreeEventViewModel : treeEventType == TreeEventType.Failing ? treeEventViewModel.FailingEvent : treeEventViewModel.PassingEvent;
        }

        public void RemoveTreeEvent(TreeEventViewModel treeEventViewModel, TreeEventType eventType)
        {
            var parent = EventTreeManipulationService.RemoveTreeEvent(EventTree, treeEventViewModel.TreeEvent);
            SelectedTreeEvent = parent == null ? MainTreeEventViewModel : FindLastEventViewModel(MainTreeEventViewModel, eventType);
        }

        private static TreeEventViewModel FindLastEventViewModel(TreeEventViewModel mainTreeEventViewModel, TreeEventType type)
        {
            switch (type)
            {
                case TreeEventType.Failing:
                    if (mainTreeEventViewModel.FailingEvent == null)
                    {
                        return mainTreeEventViewModel;
                    }

                    return FindLastEventViewModel(mainTreeEventViewModel.FailingEvent, type);
                case TreeEventType.Passing:
                    if (mainTreeEventViewModel.PassingEvent == null)
                    {
                        return mainTreeEventViewModel;
                    }

                    return FindLastEventViewModel(mainTreeEventViewModel.PassingEvent, type);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}