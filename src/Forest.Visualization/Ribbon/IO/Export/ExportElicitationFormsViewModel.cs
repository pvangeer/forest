using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Forest.Data;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Experts;
using log4net;

namespace Forest.Visualization.Ribbon.IO.Export
{
    public class ExportElicitationFormsViewModel : NotifyPropertyChangedObject
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExportElicitationFormsViewModel));

        // TODO: Remove this code. No test data in the code base please.
        public static ProbabilityEstimationPerTreeEvent TestEstimation = new ProbabilityEstimationPerTreeEvent
        {
            Experts =
            {
                new Expert { Name = "Pietje" },
                new Expert { Name = "Jantje" }
            }
        };

        private readonly ProbabilityEstimationPerTreeEvent estimation;

        private string exportLocation;

        public ExportElicitationFormsViewModel() : this(TestEstimation)
        {
        }

        public ExportElicitationFormsViewModel(ProbabilityEstimationPerTreeEvent estimation)
        {
            this.estimation = estimation;
            Experts = new ObservableCollection<ElicitationFormsExportViewModel>(
                estimation.Experts.Select(e => new ElicitationFormsExportViewModel(e)));
            foreach (var expertExportViewModel in Experts)
                expertExportViewModel.PropertyChanged += ViewModelPropertyChanged;

            Prefix = DateTime.Now.Date.ToString("yyyy-MM-dd") + " - " + estimation.Name + " - ";
        }

        public Action<string, string, Expert[], ProbabilityEstimationPerTreeEvent> OnExport { get; set; }

        public ObservableCollection<ElicitationFormsExportViewModel> Experts { get; }

        public string ExportLocation
        {
            get => exportLocation;
            set
            {
                exportLocation = value;
                OnPropertyChanged();
            }
        }

        public ICommand ExportElicitationFormsCommand => new PerformExportElicitationFormsCommand(this);

        public string Prefix { get; set; }

        public void OnExportHandler()
        {
            var expertsToExport = Experts.Where(e => e.IsChecked).Select(e => e.Expert).ToArray();
            var location = ExportLocation;
            var prefix = Prefix;

            if (OnExport == null)
            {
                Log.Error("Er is iets onverwachts misgegaan tijdens het exporteren");
                return;
            }

            OnExport(location, prefix, expertsToExport, estimation);
        }

        public event EventHandler CanExportChanged;

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ElicitationFormsExportViewModel.IsChecked))
                CanExportChanged?.Invoke(this, null);
        }
    }
}