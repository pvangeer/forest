using Forest.Data.Services;
using Forest.Data.Tree;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization.Commands
{
    public class RemoveTreeEventCommand : EventTreeCommand
    {
        public RemoveTreeEventCommand(RibbonViewModel viewModel) : base(viewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return ViewModel?.SelectedTreeEvent != null && ViewModel.CanRemoveSelectedTreeEvent();
        }

        public override void Execute(object parameter)
        {
            ViewModel?.RemoveTreeEvent(TreeEventType.Failing);
        }
    }
}