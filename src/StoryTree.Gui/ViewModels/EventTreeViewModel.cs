using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryTree.Data;
using StoryTree.Data.Annotations;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class EventTreeViewModel : INotifyPropertyChanged
    {
        public EventTreeViewModel()
        {
            
        }

        public EventTreeViewModel(EventTree eventTree)
        {
            EventTree = eventTree;
        }

        private EventTree EventTree { get; }

        public string Name => EventTree?.Description;

        public TreeEventViewModel MainTreeEventViewModel => EventTree?.MainTreeEvent == null
            ? null
            : new TreeEventViewModel(EventTree.MainTreeEvent);

        public bool IsViewModelFor(EventTree eventTree)
        {
            return Equals(EventTree, eventTree);
        }

        public void RemoveTreeEvent()
        {
            var lastEvent = FindTreeEvent(EventTree.MainTreeEvent,treeEvent => treeEvent.FalseEvent == null);
            var parent = FindTreeEvent(EventTree.MainTreeEvent, treeEvent => treeEvent.FalseEvent == lastEvent);
            parent.FalseEvent = null;
            parent.OnPropertyChanged(nameof(parent.FalseEvent));
        }

        public void AddTreeEvent()
        {
            if (EventTree.MainTreeEvent == null)
            {
                EventTree.MainTreeEvent = new TreeEvent {Name = "Nieuwe gebeurtenis"};
                OnPropertyChanged(nameof(MainTreeEventViewModel));
                return;
            }

            var lastEvent = FindTreeEvent(EventTree.MainTreeEvent, treeEvent => treeEvent.FalseEvent == null);
            lastEvent.FalseEvent = new TreeEvent {Name = "Nieuwe gebeurtenis"};
            lastEvent.OnPropertyChanged(nameof(lastEvent.FalseEvent));
        }

        private TreeEvent FindTreeEvent(TreeEvent treeEvent, Func<TreeEvent,bool> findAction)
        {
            return findAction(treeEvent) ? treeEvent : FindTreeEvent(treeEvent.FalseEvent, findAction);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}