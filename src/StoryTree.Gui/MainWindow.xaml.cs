using System.Windows;
using StoryTree.Data;
using StoryTree.Data.Tree;
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
                var trueEvent = new TreeEvent
                {
                    Name = string.Format("Event no. {0}",i+1)
                };
                currentTreeEvent.TrueEvent = trueEvent;
                currentTreeEvent = trueEvent;
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
    }
}
