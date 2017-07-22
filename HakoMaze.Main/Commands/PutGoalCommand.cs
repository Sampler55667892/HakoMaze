using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public class PutGoalCommand : PutBoxCommand
    {
        public PutGoalCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        protected override bool ModifyContentData( (int x, int y) hitCellPosition )
        {
            // 赤箱が既にあれば何もしない
            if (CanvasViewModel.MazeContentData.RedboxPosition.HasValue &&
                CanvasViewModel.MazeContentData.RedboxPosition.Value.Equals( hitCellPosition ))
                return false;

            // 黄箱が既にあれば何もしない
            if (CanvasViewModel.MazeContentData.YellowboxPosition.HasValue &&
                CanvasViewModel.MazeContentData.YellowboxPosition.Value.Equals( hitCellPosition ))
                return false;

            // 緑箱が既にあれば何もしない
            if (CanvasViewModel.MazeContentData.ExistsGreenboxPosition( hitCellPosition ))
                return false;

            // 同じ位置のゴールをクリック → ゴールを削除
            // ゴールがない or 異なる位置に存在 → ゴールの位置を更新
            if (CanvasViewModel.MazeFrameData.GoalPosition.HasValue &&
                CanvasViewModel.MazeFrameData.GoalPosition.Value.Equals( hitCellPosition )) {
                CanvasViewModel.MazeFrameData.GoalPosition = null;
                AddHistoryMessage( $"ゴールの削除：{hitCellPosition}" );
            } else {
                CanvasViewModel.MazeFrameData.GoalPosition = hitCellPosition;
                AddHistoryMessage( $"ゴールの追加/更新：{hitCellPosition}" );
            }

            return true;
        }
    }
}
