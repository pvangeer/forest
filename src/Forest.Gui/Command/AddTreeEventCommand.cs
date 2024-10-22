using Forest.Data.Services;
using Forest.Gui.ViewModels;

namespace Forest.Gui.Command
{
    public class AddTreeEventCommand : EventTreeCommand
    {
        public AddTreeEventCommand(ProjectViewModel projectViewModel) : base(projectViewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return ProjectViewModel?.EventTree != null;
        }

        public override void Execute(object parameter)
        {
            var treeEventType = TreeEventType.Failing;
            if (parameter is TreeEventType treeEventTypeCasted)
                treeEventType = treeEventTypeCasted;
            ProjectViewModel?.EventTree.AddTreeEvent(ProjectViewModel?.SelectedTreeEvent, treeEventType);
        }
    }
}