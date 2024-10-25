using System;
using System.Windows;
using Forest.Gui;
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

        public static bool ShouldMigrateProject()
        {
            var messageBoxText =
                "U wilt een verouderd bestand openen. Wilt u dit bestand migreren naar het nieuwe format om het te kunnen openen?";
            var caption = "Bestand migreren naar nieuwste versie";
            var messageBoxResult =
                MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

            return messageBoxResult == MessageBoxResult.Yes;
        }

        public static ShouldProceedState ShouldSaveOpenChanges()
        {
            var messageBoxText = "U heeft aanpassingen aan uw project nog niet opgeslagen. Wilt u dat alsnog doen?";
            var caption = "Aanpassingen opslaan";
            var messageBoxResult =
                MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            return messageBoxResult == MessageBoxResult.Yes ? ShouldProceedState.Yes :
                messageBoxResult == MessageBoxResult.No ? ShouldProceedState.No :
                ShouldProceedState.Cancel;
        }
    }
}