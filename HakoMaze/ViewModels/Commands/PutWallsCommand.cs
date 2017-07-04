using System.Windows;
using HakoMaze.Logics;

namespace HakoMaze.ViewModels
{
    public class PutWallsCommand : MainWindowCommand
    {
        public PutWallsCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            // サイズの初期設定前
            if (CanvasViewModel.MazeFrameData.SizeX == 0 || CanvasViewModel.MazeFrameData.SizeY == 0) {
                MessageBox.Show( "フレームのサイズが 0 です" );
                Exits = true;
            }
        }

        public override void OnMove()
        {
            base.OnMove();

            ViewModel.TempMessageLineText = $"(position.X, position.Y) = ({Position.X}, {Position.Y})\r\n";
        }

        public override void OnAct()
        {
            base.OnAct();

            // Canvasのマージン補正
            var canvasMargin = ViewModel.CanvasViewModel.Margin;
            var physicalPosition = new Point( Position.X - canvasMargin, Position.Y - canvasMargin );
            var wallLength = (ViewModel.CanvasViewModel.Size - canvasMargin * 2) / (double)CanvasViewModel.MazeFrameData.SizeX;
            var pickMargin = 10;

            var hitWallPosition = new SearchHitWallLogic().Search( CanvasViewModel.MazeFrameData.SizeX, physicalPosition, wallLength, pickMargin );
            if (!hitWallPosition.HasValue)
                return;

            var positionString = $"({hitWallPosition.Value.Item1}, {hitWallPosition.Value.Item2})";

            if (CanvasViewModel.MazeFrameData.ExistsWallPosition( hitWallPosition.Value )) {
                CanvasViewModel.MazeFrameData.DeleteWallPosition( hitWallPosition.Value );
                AddHistoryMessage( $"壁を削除: {positionString}" );
            } else {
                CanvasViewModel.MazeFrameData.AddWallPosition( hitWallPosition.Value );
                AddHistoryMessage( $"壁を追加: {positionString}" );
            }

            UpdateRenderCanvas();
        }
    }
}
