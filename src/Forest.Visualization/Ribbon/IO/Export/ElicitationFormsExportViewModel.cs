using Forest.Data;
using Forest.Data.Experts;

namespace Forest.Visualization.Ribbon.IO.Export
{
    public class ElicitationFormsExportViewModel : NotifyPropertyChangedObject
    {
        private bool isChecked;

        public ElicitationFormsExportViewModel(Expert expert)
        {
            Expert = expert;
            IsChecked = true;
        }

        public string Name => Expert.Name;

        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged();
            }
        }

        public Expert Expert { get; }
    }
}