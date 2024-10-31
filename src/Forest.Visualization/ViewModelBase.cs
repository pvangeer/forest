using Forest.Data;
using Forest.Visualization.ViewModels;

namespace Forest.Visualization
{
    public class ViewModelBase : Entity
    {
        protected readonly ViewModelFactory Factory;

        protected ViewModelBase(ViewModelFactory factory)
        {
            Factory = factory;
        }
    }
}