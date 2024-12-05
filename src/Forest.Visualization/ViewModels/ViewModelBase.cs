using Forest.Data;

namespace Forest.Visualization.ViewModels
{
    public class ViewModelBase : Entity
    {
        protected readonly ViewModelFactory ViewModelFactory;

        protected ViewModelBase(ViewModelFactory viewModelFactory)
        {
            ViewModelFactory = viewModelFactory;
        }
    }
}