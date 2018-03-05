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
        private TreeEventViewModel mainTreeEventViewModel = null;

        private EventTree EventTree { get; }

        public EventTreeViewModel() { }

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
                case nameof(EventTree.Description):
                    OnPropertyChanged(nameof(Name));
                    break;
            }
        }

        public string Name => EventTree?.Description;

        public TreeEventViewModel MainTreeEventViewModel
        {
            get
            {
                if (EventTree == null)
                {
                    return null;
                }

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

        public bool IsViewModelFor(EventTree eventTree)
        {
            return Equals(EventTree, eventTree);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddTreeEvent(TreeEventViewModel treeEventViewModel)
        {
            EventTreeManipulationService.AddTreeEvent(EventTree,treeEventViewModel?.TreeEvent,TreeEventType.Failing);
            SelectedTreeEvent = treeEventViewModel == null ? MainTreeEventViewModel : treeEventViewModel.FailingEvent;
        }

        public void RemoveTreeEvent(TreeEventViewModel treeEventViewModel)
        {
            var parent = EventTreeManipulationService.RemoveTreeEvent(EventTree, treeEventViewModel.TreeEvent);
            SelectedTreeEvent = parent == null ? MainTreeEventViewModel : FindLastEventViewModel(MainTreeEventViewModel, TreeEventType.Failing);
        }

        private TreeEventViewModel FindLastEventViewModel(TreeEventViewModel mainTreeEventViewModel, TreeEventType type)
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