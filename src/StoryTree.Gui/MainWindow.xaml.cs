﻿using System.Windows;
using System.Windows.Input;
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
            CreateTestViewModel();
        }

        #region TestCode
        private void CreateTestViewModel()
        {
            DataContext = new ProjectViewModel(new Project
            {
                EventTrees =
                {
                    CreateEventTree("First event tree", 3),
                    CreateEventTree("Second event tree", 2),
                    CreateEventTree("3", 4)
                },
                Experts =
                {
                    new Expert
                    {
                        Name = "Klaas",
                        Email = "email@domein.nl",
                        Expertise = "Alles",
                        Organisation = "Eigen bedrijf",
                        Telephone = "088-3358339"
                    },
                    new Expert
                    {
                        Name = "Piet",
                        Email = "piet@email.nl",
                        Expertise = "Niets",
                        Organisation = "Ander bedrijf",
                        Telephone = "088-3358339"
                    },
                }
            });
        }

        private static EventTree CreateEventTree(string treeDescription, int numberTreeEvents)
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
                currentTreeEvent.FailingEvent = falseEvent;
                currentTreeEvent = falseEvent;
            }

            return tree;
        }
        #endregion

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

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
