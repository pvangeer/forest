using System.Windows;
using System.Windows.Controls;

namespace Forest.Visualization
{
    /// <summary>
    ///     Grid splitter that show or hides the following row when the visibility of the splitter is changed.
    /// </summary>
    public class HidableGridSplitter : GridSplitter
    {
        private GridLength height;
        private GridLength width;

        public HidableGridSplitter()
        {
            IsVisibleChanged += HidableGridSplitter_IsVisibleChanged;
            height = new GridLength(0, GridUnitType.Auto);
            width = new GridLength(0, GridUnitType.Auto);
        }

        public HideDirection HideDirection { get; set; }

        private void HidableGridSplitter_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var parent = Parent as Grid;
            if (parent == null) return;

            int columnIndex;
            ColumnDefinition columnToHide;
            int rowIndex;
            RowDefinition rowToHide;
            switch (HideDirection)
            {
                case HideDirection.Left:
                    columnIndex = Grid.GetColumn(this);
                    if (columnIndex - 1 < 0) return;

                    columnToHide = parent.ColumnDefinitions[columnIndex - 1];

                    if (Visibility == Visibility.Visible)
                    {
                        columnToHide.Width = width;
                    }
                    else
                    {
                        width = columnToHide.Width;
                        columnToHide.Width = new GridLength(0);
                    }

                    break;
                case HideDirection.Right:
                    columnIndex = Grid.GetColumn(this);
                    if (columnIndex + 1 >= parent.ColumnDefinitions.Count) return;

                    columnToHide = parent.ColumnDefinitions[columnIndex + 1];

                    if (Visibility == Visibility.Visible)
                    {
                        columnToHide.Width = width;
                    }
                    else
                    {
                        width = columnToHide.Width;
                        columnToHide.Width = new GridLength(0);
                    }

                    break;
                case HideDirection.Up:
                    rowIndex = Grid.GetRow(this);

                    if (rowIndex - 1 < 0) return;

                    rowToHide = parent.RowDefinitions[rowIndex - 1];

                    if (Visibility == Visibility.Visible)
                    {
                        rowToHide.Height = height;
                    }
                    else
                    {
                        height = rowToHide.Height;
                        rowToHide.Height = new GridLength(0);
                    }

                    break;
                case HideDirection.Down:
                    rowIndex = Grid.GetRow(this);

                    if (rowIndex + 1 >= parent.RowDefinitions.Count) return;

                    rowToHide = parent.RowDefinitions[rowIndex + 1];

                    if (Visibility == Visibility.Visible)
                    {
                        rowToHide.Height = height;
                    }
                    else
                    {
                        height = rowToHide.Height;
                        rowToHide.Height = new GridLength(0);
                    }

                    break;
            }
        }
    }

    public enum HideDirection
    {
        Left,
        Right,
        Up,
        Down
    }
}