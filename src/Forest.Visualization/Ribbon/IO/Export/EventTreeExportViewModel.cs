using Forest.Data;
using Forest.Data.Tree;

namespace Forest.Visualization.Ribbon.IO.Export
{
    public class EventTreeExportViewModel : NotifyPropertyChangedObject
    {
        public EventTreeExportViewModel(EventTree eventTree)
        {
            EventTree = eventTree;
        }

        public EventTree EventTree { get; }
    }
}