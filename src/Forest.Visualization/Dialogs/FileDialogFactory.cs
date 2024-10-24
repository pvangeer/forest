using System;
using System.Windows;
using Forest.Gui.Components;
using Microsoft.Win32;

namespace Forest.Visualization.Dialogs
{
    public static class FileDialogFactory
    {
        public static Func<string, FileNameQuestionResult> AskUserForFileNameToOpenFunc()
        {
            return fileName =>
            {
                var dialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Filter = "Faalpadenproject bestand (*.xml)|*.xml",
                    FileName = fileName
                };
                var proceed = (bool)dialog.ShowDialog(Application.Current.MainWindow);
                return new FileNameQuestionResult(proceed, dialog.FileName);
            };
        }

        public static Func<string, FileNameQuestionResult> AskUserForFileNameToSaveToFunc()
        {
            return fileName =>
            {
                var dialog = new SaveFileDialog
                {
                    CheckPathExists = true,
                    FileName = fileName,
                    OverwritePrompt = true,
                    Filter = "Faalpadenproject bestand (*.xml)|*.xml"
                };
                var proceed = (bool)dialog.ShowDialog(Application.Current.MainWindow);
                return new FileNameQuestionResult(proceed, dialog.FileName);
            };
        }
    }
}