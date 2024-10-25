using System;
using Forest.Data.Tree;

namespace Forest.Data.Services
{
    public class TreeEventsChangedEventArgs : EventArgs
    {
        public TreeEventsChangedEventArgs(EventTreeModification modification, TreeEvent parentEvent, TreeEvent addedEvent = null)
        {
            Modification = modification;
            ParentEvent = parentEvent;
            AddedEvent = addedEvent;
        }

        public EventTreeModification Modification { get; }

        public TreeEvent ParentEvent { get; }

        public TreeEvent AddedEvent { get; }
    }
}