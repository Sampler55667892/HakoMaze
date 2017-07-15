using System;
using System.Windows;
using Microsoft.Win32;
using HakoMaze.Data;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
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

            if (CanvasViewModel.MazeFrameData.SizeX < DataConstraints.MinFrameSize ||
                DataConstraints.MaxFrameSize < CanvasViewModel.MazeFrameData.SizeX) {
                CanvasViewModel.MazeFrameData.Clear();
                CanvasViewModel.MazeContentData.ClearAllBoxes();
                MessageBox.Show( $"SizeX が {DataConstraints.MinFrameSize} ～ {DataConstraints.MaxFrameSize} の間のデータのみ読込み可能です" );
                return;
            }

            if (CanvasViewModel.MazeFrameData.SizeX != CanvasViewModel.MazeFrameData.SizeY) {
                MessageBox.Show( "SizeX は SizeY と一致するデータのみ読込み可能です" );
                return;
            }

            UpdateRenderCanvas();
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
