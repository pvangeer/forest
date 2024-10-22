using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forest.Data.Hydraulics;
using Forest.Data.Properties;

namespace Forest.Data
{
    public class EventTreeProject : INotifyPropertyChanged
    {
        private EventTree eventTree;

        public EventTreeProject()
        {
            Name = "Nieuw project";
            AssessmentSection = "1-1";
            ProjectLeader = new Person();
            EventTree = new EventTree();
            Experts = new ObservableCollection<Expert>();
            HydraulicConditions = new ObservableCollection<HydraulicCondition>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AssessmentSection { get; set; }

        public string ProjectInformation { get; set; }

        public Person ProjectLeader { get; set; }

        public EventTree EventTree
        {
            get => eventTree;
            set
            {
                eventTree = value;
                if (eventTree == null)
                    throw new ArgumentNullException();
            }
        }

        public ObservableCollection<Expert> Experts { get; }

        public ObservableCollection<HydraulicCondition> HydraulicConditions { get; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}