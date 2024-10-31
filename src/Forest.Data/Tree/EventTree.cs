using System;
using Forest.Data.Services;

namespace Forest.Data.Tree
{
    public class EventTree : Entity
    {
        public string Name { get; set; }

        public TreeEvent MainTreeEvent { get; set; }

        public event EventHandler<TreeEventsChangedEventArgs> TreeEventsChanged;

        public virtual void OnTreeEventsChanged(TreeEventsChangedEventArgs e)
        {
            TreeEventsChanged?.Invoke(this, e);
        }
    }
}