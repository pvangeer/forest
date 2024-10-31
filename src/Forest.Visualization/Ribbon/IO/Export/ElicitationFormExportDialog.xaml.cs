﻿using System.Windows;

namespace Forest.Visualization.Ribbon.IO.Export
{
    /// <summary>
    ///     Interaction logic for ElicitationFormExportDialog.xaml
    /// </summary>
    public partial class ElicitationFormExportDialog : Window
    {
        public ElicitationFormExportDialog()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}