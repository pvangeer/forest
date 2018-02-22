using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data;
using StoryTree.Data.Annotations;
using StoryTree.Gui.Services;

namespace StoryTree.Gui.ViewModels
{
    public class EventTreeViewModel : INotifyPropertyChanged
    {
        private TreeEventViewModel selectedTreeEvent;
        private EventTree EventTree { get; }

        public EventTreeViewModel()
        {
            
        }

        public EventTreeViewModel(EventTree eventTree)
        {
            EventTree = eventTree;
        }

        public string Name => EventTree?.Description;

        public TreeEventViewModel MainTreeEventViewModel => EventTree?.MainTreeEvent == null
            ? null
            : new TreeEventViewModel(EventTree.MainTreeEvent, this);

        public TreeEventViewModel SelectedTreeEvent
        {
            get => selectedTreeEvent;
            set
            {
                selectedTreeEvent = value;
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
            var newTreeEvent = EventTreeManipulationService.AddTreeEvent(EventTree,treeEventViewModel,TreeEventType.Failing);
            if (Equals(EventTree.MainTreeEvent, newTreeEvent))
            {
                OnPropertyChanged(nameof(MainTreeEventViewModel));
            }

            SelectedTreeEvent = treeEventViewModel.FailingEvent;
            OnPropertyChanged(nameof(SelectedTreeEvent));
        }

        public void RemoveTreeEvent(TreeEventViewModel treeEventViewModel)
        {
            var parentEvent = EventTreeManipulationService.RemoveTreeEvent(EventTree,treeEventViewModel);
            // TODO: Select parent event

        }
    }
}