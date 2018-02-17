using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StoryTree.Data;
using StoryTree.Data.Tree;
using StoryTree.Gui.UserControls;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // Testcode
            DataContext = new ProjectViewModel(new Project
            {
                EventTrees =
                {
                    GetEventTree("First event tree",3),
                    GetEventTree("Second event tree",2),
                    GetEventTree("3",4)
                }
            });
        }

        private static EventTree GetEventTree(string treeDescription, int numberTreeEvents)
        {
            var mainTreeEvent = new TreeEvent
            {
                Name = "First element"
            };

            var tree = new EventTree
            {
                Description = treeDescription,
                MainTreeEvent = mainTreeEvent
            };

            var currentTreeEvent = mainTreeEvent;
            for (int i = 0; i < numberTreeEvents - 1; i++)
            {
                var falseEvent = new TreeEvent
                {
                    Name = string.Format("Event no. {0}",i+1)
                };
                currentTreeEvent.FalseEvent = falseEvent;
                currentTreeEvent = falseEvent;
            }

            return tree;
        }

        private void OnFileNewClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnFileSaveClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnFileSaveAsClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnFileOpenClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnFileExitClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Ribbon_OnSelectedTabChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO: This can be achieved via bindings and datatemplates, but passing DataContext was a big problem for some reason
            HostControl.Content = null;
            if (Equals(Ribbon.SelectedTabItem, GeneralDataTabItem))
            {
                // Show general data control
            }

            if (Equals(Ribbon.SelectedTabItem, ParticipatntsTabItem))
            {
                // Show participants control
            }

            if (Equals(Ribbon.SelectedTabItem, EventsTreeTabItem))
            {
                HostControl.Content = new StoryBoardControl {DataContext = DataContext};
            }
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
