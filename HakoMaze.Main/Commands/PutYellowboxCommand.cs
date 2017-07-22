using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public class PutYellowboxCommand : PutBoxCommand
    {
        public PutYellowboxCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        protected override bool ModifyContentData( (int x, int y) hitCellPosition )
        {
            // 赤箱が既にあれば何もしない
            if (CanvasViewModel.MazeContentData.RedboxPosition.HasValue &&
                CanvasViewModel.MazeContentData.RedboxPosition.Value.Equals( hitCellPosition ))
                return false;
            // 緑箱が既にあれば何もしない
            if (CanvasViewModel.MazeContentData.ExistsGreenboxPosition( hitCellPosition ))
                return false;

            // ゴールが既にあれば何もしない
            if (CanvasViewModel.MazeFrameData.GoalPosition.HasValue &&
                CanvasViewModel.MazeFrameData.GoalPosition.Value.Equals( hitCellPosition ))
                return false;

            // 同じ位置の黄箱をクリック → 黄箱を削除
            // 黄箱がない or 異なる位置に存在 → 黄箱の位置を更新
            if (CanvasViewModel.MazeContentData.YellowboxPosition.HasValue &&
                CanvasViewModel.MazeContentData.YellowboxPosition.Value.Equals( hitCellPosition )) {
                CanvasViewModel.MazeContentData.YellowboxPosition = null;
                AddHistoryMessage( $"黄箱の削除：{hitCellPosition}" );
            } else {
                CanvasViewModel.MazeContentData.YellowboxPosition = hitCellPosition;
                AddHistoryMessage( $"黄箱の追加/更新：{hitCellPosition}" );
            }

            return true;
        }
    }
}
