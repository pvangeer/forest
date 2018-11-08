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
        private string exportLocation;

        private static readonly ILog Log = LogManager.GetLogger(typeof(ExportElicitationFormsViewModel));

        public static Project TestProject = new Project
        {
            Experts =
            {
                new Expert {Name = "Pietje"},
                new Expert {Name = "Jantje"}
            },
            EventTrees =
            {
                new EventTree {Name = "Gebeurtenis 1", NeedsSpecification = true},
                new EventTree {Name = "Gebeurtenis 2", NeedsSpecification = false},
            },
            Name = "Testproject"
        };

        public ExportElicitationFormsViewModel() : this(TestProject) { }

        public ExportElicitationFormsViewModel(Project project)
        {
            Experts = new ObservableCollection<ElicitationFormsExportViewModel>(project.Experts.Select(e => new ElicitationFormsExportViewModel(e)));
            foreach (var expertExportViewModel in Experts)
            {
                expertExportViewModel.PropertyChanged += ViewModelPropertyChanged;
            }
            EventTrees = new ObservableCollection<EventTreeExportViewModel>(project.EventTrees.Select(eventTree => new EventTreeExportViewModel(eventTree)));
            foreach (var eventTreeExportViewModel in EventTrees)
            {
                eventTreeExportViewModel.PropertyChanged += ViewModelPropertyChanged;
            }

            Prefix = DateTime.Now.Date.ToString("yyyy-MM-dd") + " - " + project.Name + " - ";
        }

        public void OnExportHandler()
        {
            Expert[] expertsToExport = Experts.Where(e => e.IsChecked).Select(e => e.Expert).ToArray();
            EventTree[] eventTreesToExport = EventTrees.Where(e => e.IsChecked).Select(e => e.EventTree).ToArray();
            string location = ExportLocation;
            string prefix = Prefix;

            if (OnExport == null)
            {
                Log.Error("Er is iets onverwachts misgegaan tijdens het exporteren");
                return;
            }

            OnExport(location, prefix, expertsToExport, eventTreesToExport);
        }

        public Action<string, string, Expert[], EventTree[]> OnExport { get; set; }

        public ObservableCollection<ElicitationFormsExportViewModel> Experts { get; }

        public event EventHandler CanExportChanged;

        public string ExportLocation
        {
            get => exportLocation;
            set
            {
                exportLocation = value;
                OnPropertyChanged(nameof(ExportLocation));
            }
        }

        public ObservableCollection<EventTreeExportViewModel> EventTrees { get; }

        public ICommand ExportElicitationFormsCommand => new PerformExportElicitationFormsCommand(this);

        public string Prefix { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ElicitationFormsExportViewModel.IsChecked) ||
                e.PropertyName == nameof(EventTreeExportViewModel.IsChecked))
            {
                CanExportChanged?.Invoke(this, null);
            }
        }

    }
}