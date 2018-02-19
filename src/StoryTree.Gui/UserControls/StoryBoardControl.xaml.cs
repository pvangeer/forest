using System.Windows.Controls;
using StoryTree.Gui.ViewModels;

namespace StoryTree.Gui.UserControls
{
    /// <summary>
    /// Interaction logic for StoryBoardControl.xaml
    /// </summary>
    public partial class StoryBoardControl
    {
        public StoryBoardControl()
        {
            InitializeComponent();
        }

        public ProjectViewModel ProjectViewModel => DataContext as ProjectViewModel;

        private void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectViewModel == null)
            {
                return;
            }

            ProjectViewModel.SelectedEventTree = ListView.SelectedItem as EventTreeViewModel;
        }
    }
}
