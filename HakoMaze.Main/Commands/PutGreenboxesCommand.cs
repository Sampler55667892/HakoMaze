using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public class PutGreenboxesCommand : PutBoxCommand
    {
        public PutGreenboxesCommand( MainWindowViewModel vm ) : base( vm )
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

            // ゴールが既にあれば何もしない
            if (CanvasViewModel.MazeFrameData.GoalPosition.HasValue &&
                CanvasViewModel.MazeFrameData.GoalPosition.Value.Equals( hitCellPosition ))
                return false;

            // 緑箱の追加・更新
            if (CanvasViewModel.MazeContentData.ExistsGreenboxPosition( hitCellPosition )) {
                CanvasViewModel.MazeContentData.DeleteGreenbox( hitCellPosition );
                AddHistoryMessage( $"緑箱の削除：{hitCellPosition}" );
            } else {
                CanvasViewModel.MazeContentData.AddGreenbox( hitCellPosition );
                AddHistoryMessage( $"緑箱の追加：{hitCellPosition}" );
            }

            return true;
        }
    }
}
