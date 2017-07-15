using System;
using System.Windows;
using Microsoft.Win32;
using HakoMaze.Data;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public class SaveCommand : MainWindowCommand
    {
        public SaveCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            if (CanvasViewModel.IsFrameSizeZero) {
                MessageBox.Show( "フレームのサイズが 0 です" );
                Exits = true;
                return;
            }

            var fileName = GetFileName();
            if (string.IsNullOrEmpty( fileName ))
                return;

            var serializer = new MazeDataSerializer( CanvasViewModel.MazeFrameData, CanvasViewModel.MazeContentData );
            serializer.Save( fileName );
        }

        string GetFileName()
        {
            // (cf.) WPF4.5入門 その38 「ファイルダイアログ」
            //     http://blog.okazuki.jp/entry/2014/08/16/112714
            var dialog = new SaveFileDialog { InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Desktop ), FileName = "sample.txt" };
            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value)
                return null;

            return dialog.FileName;
        }
    }
}
