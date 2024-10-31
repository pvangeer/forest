using Forest.Data;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization
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