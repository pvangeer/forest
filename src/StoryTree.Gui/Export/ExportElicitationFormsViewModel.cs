using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using log4net;
using StoryTree.Data;
using StoryTree.Gui.Annotations;

namespace StoryTree.Gui.Export
{
    public class ExportElicitationFormsViewModel : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExportElicitationFormsViewModel));

        public static EventTreeProject TestEventTreeProject = new EventTreeProject
        {
            Experts =
            {
                new Expert { Name = "Pietje" },
                new Expert { Name = "Jantje" }
            },
            Name = "Testproject"
        };

        private string exportLocation;

        public ExportElicitationFormsViewModel() : this(TestEventTreeProject)
        {
        }

        public ExportElicitationFormsViewModel(EventTreeProject eventTreeProject)
        {
            Experts = new ObservableCollection<ElicitationFormsExportViewModel>(
                eventTreeProject.Experts.Select(e => new ElicitationFormsExportViewModel(e)));
            foreach (var expertExportViewModel in Experts)
                expertExportViewModel.PropertyChanged += ViewModelPropertyChanged;
            EventTree = new EventTreeExportViewModel(eventTreeProject.EventTree);
            EventTree.PropertyChanged += ViewModelPropertyChanged;

            Prefix = DateTime.Now.Date.ToString("yyyy-MM-dd") + " - " + eventTreeProject.Name + " - ";
        }

        public Action<string, string, Expert[], EventTree> OnExport { get; set; }

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

        public EventTreeExportViewModel EventTree { get; }

        public ICommand ExportElicitationFormsCommand => new PerformExportElicitationFormsCommand(this);

        public string Prefix { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnExportHandler()
        {
            var expertsToExport = Experts.Where(e => e.IsChecked).Select(e => e.Expert).ToArray();
            var eventTreeToExport = EventTree.EventTree;
            var location = ExportLocation;
            var prefix = Prefix;

            if (OnExport == null)
            {
                Log.Error("Er is iets onverwachts misgegaan tijdens het exporteren");
                return;
            }

            OnExport(location, prefix, expertsToExport, eventTreeToExport);
        }

        public event EventHandler CanExportChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ElicitationFormsExportViewModel.IsChecked))
                CanExportChanged?.Invoke(this, null);
        }
    }
}