using StoryTree.Data;

namespace StoryTree.Gui.ViewModels
{
    public class EventTreeViewModel
    {
        public EventTreeViewModel()
        {
            
        }

        public EventTreeViewModel(EventTree eventTree)
        {
            EventTree = eventTree;
        }

        private EventTree EventTree { get; }

        public TreeEventViewModel MainTreeEventViewModel => EventTree?.MainTreeEvent == null
            ? null
            : new TreeEventViewModel(EventTree.MainTreeEvent);
    }
}