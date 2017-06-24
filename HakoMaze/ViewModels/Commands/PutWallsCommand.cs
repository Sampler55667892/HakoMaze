using System.Windows;
using HakoMaze.Logics;

namespace HakoMaze.ViewModels
{
    public class PutWallsCommand : MainWindowCommand
    {
        public PutWallsCommand( MainWindowViewModel vm ) : base( vm )
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
            if (ViewModel.MazeFrameData.SizeX == 0 || ViewModel.MazeFrameData.SizeY == 0) {
                MessageBox.Show( "フレームのサイズが 0 です" );
                return;
            }

            // Canvasのマージン補正
            var canvasMargin = ViewModel.CanvasViewModel.Margin;
            var physicalPosition = new Point( Position.X - canvasMargin, Position.Y - canvasMargin );
            var wallLength = (ViewModel.CanvasViewModel.Size - canvasMargin * 2) / (double)ViewModel.MazeFrameData.SizeX;
            var pickMargin = 10;

            var hitIndex = new SearchHitWallLogic().Search( ViewModel.MazeFrameData.SizeX, physicalPosition, wallLength, pickMargin );
            if (!hitIndex.HasValue)
                return;

            var positionString = $"({hitIndex.Value.Item1}, {hitIndex.Value.Item2})";

            if (ViewModel.MazeFrameData.ExistsWallPosition( hitIndex.Value )) {
                ViewModel.MazeFrameData.DeleteWallPosition( hitIndex.Value );
                AddHistoryMessage( $"壁を削除: {positionString}" );
            } else {
                ViewModel.MazeFrameData.AddWallPosition( hitIndex.Value );
                AddHistoryMessage( $"壁を追加: {positionString}" );
            }

            // 再描画
            new DrawMazeFrameLogic().Draw( ViewModel );
        }
    }
}
