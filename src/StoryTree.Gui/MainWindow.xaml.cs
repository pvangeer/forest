using System.Windows;
using StoryTree.Data;
using StoryTree.Data.Tree;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Testcode
            DataContext = new ProjectViewModel(new Project
            {
                MainTreeEvent =
                {
                    Name = "First element",
                    TrueEvent = new TreeEvent
                    {
                        Name = "Second element",
                        TrueEvent = new TreeEvent
                        {
                            Name = "Third element",
                            TrueEvent = new TreeEvent
                            {
                                Name = "EndNode"
                            }
                        }
                    }
                }
            });
        }
    }
}
