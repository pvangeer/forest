using Forest.Data.Services;
using Forest.Gui.ViewModels;

namespace Forest.Gui.Command
{
    public class RemoveTreeEventCommand : EventTreeCommand
    {
        public RemoveTreeEventCommand(ProjectViewModel projectViewModel) : base(projectViewModel)
        {
        }

        public override void Execute(object parameter)
        {
            ProjectViewModel.EventTree.RemoveTreeEvent(ProjectViewModel.SelectedTreeEvent, TreeEventType.Failing);
        }
    }
}