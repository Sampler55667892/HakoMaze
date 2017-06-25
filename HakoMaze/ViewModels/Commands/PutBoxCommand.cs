using System.Windows;
using HakoMaze.Logics;

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

            // Canvasのマージン補正
            var canvasMargin = ViewModel.CanvasViewModel.Margin;
            var physicalPosition = new Point( Position.X - canvasMargin, Position.Y - canvasMargin );
            var wallLength = (ViewModel.CanvasViewModel.Size - canvasMargin * 2) / (double)CanvasViewModel.MazeFrameData.SizeX;

            var hitCellPosition = new SearchHitCellLogic().Search( CanvasViewModel.Size, physicalPosition, wallLength );
            if (!hitCellPosition.HasValue)
                return;

            // 箱データの追加・メッセージ表示
            if (ModifyContentData( hitCellPosition.Value ))
                UpdateRenderCanvas();
        }

        // 他の箱と重複しないかもチェック
        protected abstract bool ModifyContentData( (int x, int y) hitCellPosition );
    }
}
