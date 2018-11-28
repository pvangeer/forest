﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using StoryTree.Messaging;

namespace StoryTree.Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private readonly StoryTreeLog log = new StoryTreeLog(typeof(App));
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            log.Error($"Er is iets onverwachts misgegaan: {e.Exception.Message}");
            e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EventManager.RegisterClassHandler(typeof(DataGrid), DataGrid.PreviewMouseLeftButtonDownEvent,
                new RoutedEventHandler(EventHelper.DataGridPreviewMouseLeftButtonDownEvent));

        }
    }

    public static class EventHelper
    {
        internal static void DataGridPreviewMouseLeftButtonDownEvent
                   (object sender, System.Windows.RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            var mbe = e as MouseButtonEventArgs;

            DependencyObject obj = null;
            if (mbe != null)
            {
                obj = mbe.OriginalSource as DependencyObject;
                while (obj != null && !(obj is DataGridCell))
                {
                    obj = VisualTreeHelper.GetParent(obj);
                }
            }

            DataGridCell cell = null;
            DataGrid dataGrid = null;

            if (obj != null)
                cell = obj as DataGridCell;

            if (cell != null && !cell.IsEditing && !cell.IsReadOnly)
            {
                if (!cell.IsFocused)
                {
                    cell.Focus();
                }
                dataGrid = FindVisualParent<DataGrid>(cell);
                if (dataGrid != null)
                {
                    if (dataGrid.SelectionUnit
                        != DataGridSelectionUnit.FullRow)
                    {
                        if (!cell.IsSelected)
                            cell.IsSelected = true;
                    }
                    else
                    {
                        var row = FindVisualParent<DataGridRow>(cell);
                        if (row != null && !row.IsSelected)
                        {
                            row.IsSelected = true;
                        }
                    }
                }
            }

        }
        //http://wpf.codeplex.com/wikipage?title=Single-Click%20Editing
        //http://stackoverflow.com/questions/10027182/how-to-set-an-evenhandler-in-wpf-to-all-windows-entire-application
        //http://www.scottlogic.com/blog/2008/12/02/wpf-datagrid-detecting-clicked-cell-and-row.html

        static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }
    }
}
