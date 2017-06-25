using System;
using System.Windows;
using Microsoft.Win32;
using HakoMaze.Logics;
using HakoMaze.Models;

namespace HakoMaze.ViewModels
{
    // TODO: Implements
    public class OpenCommand : MainWindowCommand
    {
        public OpenCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            var fileName = GetFileName();
            if (string.IsNullOrEmpty( fileName ))
                return;

            var serializer = new MazeDataSerializer( CanvasViewModel.MazeFrameData, CanvasViewModel.MazeContentData );
            if (!serializer.Load( fileName )) {
                MessageBox.Show( $"{fileName} の読込みに失敗しました" );
                return;
            }

            UpdateCanvas();
        }

        string GetFileName()
        {
            var dialog = new OpenFileDialog { InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Desktop ) };
            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value)
                return null;

            return dialog.FileName;
        }
    }
}
