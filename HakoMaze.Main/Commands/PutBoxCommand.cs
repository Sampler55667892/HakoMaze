using System.Windows;
using HakoMaze.Main.Logics;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public abstract class PutBoxCommand : MainWindowCommand
    {
        public PutBoxCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            // サイズの初期設定前
            if (CanvasViewModel.IsFrameSizeZero) {
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

            var hitCellPosition = new SearchHitCellLogic().Search( CanvasViewModel.MazeFrameData.SizeX, physicalPosition, wallLength );
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
