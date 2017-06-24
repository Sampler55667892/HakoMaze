using System.Windows;

namespace HakoMaze.ViewModels
{
    public abstract class PutBoxCommand : MainWindowCommand
    {
        public PutBoxCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override void OnMove()
        {
            base.OnMove();

            ViewModel.TempMessageLineText = $"(position.X, position.Y) = ({Position.X}, {Position.Y})\r\n";
        }

        public override void OnAct()
        {
            base.OnAct();

            // サイズの初期設定前
            if (CanvasViewModel.MazeFrameData.SizeX == 0 || CanvasViewModel.MazeFrameData.SizeY == 0) {
                MessageBox.Show( "フレームのサイズが 0 です" );
                StopsAct = true;
            }
        }
    }
}
