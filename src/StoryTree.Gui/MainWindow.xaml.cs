﻿using System.Windows.Input;
using StoryTree.Data;
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

            var storyTreeGui = new StoryTreeGui
            {
                Project = new Project()
            };
            var guiViewModel = new GuiViewModel(storyTreeGui)
            {
                Win32Window = this
            };
            guiViewModel.OnInvalidateVisual += (o,e) =>
            {
                HostControl.InvalidateVisual();
                InvalidateVisual();
            };
            DataContext = guiViewModel;
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }

    
}
