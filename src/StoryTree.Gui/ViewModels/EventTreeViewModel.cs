using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using StoryTree.Data;
using StoryTree.Data.Properties;
using StoryTree.Data.Services;

namespace StoryTree.Gui.ViewModels
{
    public class EventTreeViewModel : INotifyPropertyChanged
    {
        private TreeEventViewModel selectedTreeEvent;
        private TreeEventViewModel mainTreeEventViewModel;
        private bool isSelected;

        private EventTree EventTree { get; }

        public EventTreeViewModel():this(new EventTree()) { }

        public EventTreeViewModel([NotNull]EventTree eventTree)
        {
            EventTree = eventTree;
            eventTree.PropertyChanged += EventTreePropertyChanged;
        }

        private void EventTreePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EventTree.MainTreeEvent):
                    mainTreeEventViewModel = null;
                    OnPropertyChanged(nameof(MainTreeEventViewModel));
                    break;
                case nameof(EventTree.Name):
                    OnPropertyChanged(nameof(Name));
                    break;
                case nameof(EventTree.Summary):
                    OnPropertyChanged(nameof(Summary));
                    break;
                case nameof(EventTree.Color):
                    OnPropertyChanged(nameof(Color));
                    break;
            }
        }

        public string Name
        {
            get => EventTree.Name;
            set
            {
                EventTree.Name= value;
                EventTree.OnPropertyChanged(nameof(EventTree.Name));
            }
        }

        public string Summary
        {
            get => EventTree.Summary;
            set
            {
                EventTree.Summary = value;
                EventTree.OnPropertyChanged(nameof(EventTree.Summary));
            }
        }

        public string Details
        {
            get => EventTree.Details;
            set => EventTree.Details = value;
        }

        public Color Color
        {
            get => EventTree.Color;
            set
            {
                EventTree.Color = value;
                EventTree.OnPropertyChanged(nameof(EventTree.Color));
            }
        }

        public TreeEventViewModel SelectedTreeEvent
        {
            get => selectedTreeEvent;
            set
            {
                selectedTreeEvent = value;
                MainTreeEventViewModel?.FireSelectedStateChangeRecursive();
                OnPropertyChanged(nameof(SelectedTreeEvent));
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public TreeEventViewModel MainTreeEventViewModel
        {
            get
            {
                if (mainTreeEventViewModel == null && EventTree.MainTreeEvent == null)
                {
                    return null;
                }

                return mainTreeEventViewModel ??
                       (mainTreeEventViewModel = new TreeEventViewModel(EventTree.MainTreeEvent, this));
            }
        }

        public IEnumerable<TreeEventViewModel> AllTreeEvents => GetAllEventsRecursive(MainTreeEventViewModel);

        public bool IsViewModelFor(EventTree eventTree)
        {
            return Equals(EventTree, eventTree);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddTreeEvent(TreeEventViewModel treeEventViewModel, TreeEventType treeEventType)
        {
            EventTreeManipulationService.AddTreeEvent(EventTree,treeEventViewModel?.TreeEvent,treeEventType);
            SelectedTreeEvent = treeEventViewModel == null ? MainTreeEventViewModel : treeEventType == TreeEventType.Failing ? treeEventViewModel.FailingEvent : treeEventViewModel.PassingEvent;
            OnPropertyChanged(nameof(AllTreeEvents));
        }

        public void RemoveTreeEvent(TreeEventViewModel treeEventViewModel, TreeEventType eventType)
        {
            var parent = EventTreeManipulationService.RemoveTreeEvent(EventTree, treeEventViewModel.TreeEvent);
            SelectedTreeEvent = parent == null ? MainTreeEventViewModel : FindLastEventViewModel(MainTreeEventViewModel, eventType);
            OnPropertyChanged(nameof(AllTreeEvents));
        }

        private static IEnumerable<TreeEventViewModel> GetAllEventsRecursive(TreeEventViewModel treeEventViewModel)
        {
            var list = new[]{treeEventViewModel};

            if (treeEventViewModel.FailingEvent != null)
            {
                list = list.Concat(GetAllEventsRecursive(treeEventViewModel.FailingEvent)).ToArray();
            }

            if (treeEventViewModel.PassingEvent != null)
            {
                list = list.Concat(GetAllEventsRecursive(treeEventViewModel.PassingEvent)).ToArray();
            }

            return list;
        }

        private static TreeEventViewModel FindLastEventViewModel(TreeEventViewModel mainTreeEventViewModel, TreeEventType type)
        {
            switch (type)
            {
                case TreeEventType.Failing:
                    if (mainTreeEventViewModel.FailingEvent == null)
                    {
                        return mainTreeEventViewModel;
                    }

                    return FindLastEventViewModel(mainTreeEventViewModel.FailingEvent, type);
                case TreeEventType.Passing:
                    if (mainTreeEventViewModel.PassingEvent == null)
                    {
                        return mainTreeEventViewModel;
                    }

                    return FindLastEventViewModel(mainTreeEventViewModel.PassingEvent, type);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}